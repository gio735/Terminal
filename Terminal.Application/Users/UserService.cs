using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Terminal.Application.Definitions.Repositories;
using Terminal.Application.Definitions.Requests;
using Terminal.Application.Definitions.Responses;
using Terminal.Application.Helpers;
using Terminal.Application.Reports.Repositories;
using Terminal.Application.Reports.Requests;
using Terminal.Application.Users.Repositories;
using Terminal.Application.Users.Requests;
using Terminal.Application.Votes.Repositories;
using Terminal.Domain.Models;
using Terminal.Domain.Utilities;

namespace Terminal.Application.Users
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly IDefinitionRepository _definitionRepository;
        private readonly IVoteRepository _voteRepository;
        private readonly IReportRepository _reportRepository;
        private readonly SMTPConfiguration _SMTPConfiguration;
        private readonly string _userId;
        private readonly ILogger<UserService> _logger;
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        //In both cases dictionary consists of Email and (Token, CreationDate) <Email, (Token>
        private static Dictionary<string, (string Token, DateTime CreationDate, UserCreationRequest User)> _registrationRequests = new();
        private static Dictionary<string, (string Token, DateTime CreationDate)> _passwordResetRequests = new();

        public UserService(IHttpContextAccessor httpContextAccessor, 
                            IUserRepository userRepository,
                            IDefinitionRepository definitionRepository,
                            IVoteRepository voteRepository,
                            IReportRepository reportRepository,
                            IOptions<SMTPConfiguration> sMTPConfiguration,
                            ILogger<UserService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _definitionRepository = definitionRepository;
            _voteRepository = voteRepository;
            _reportRepository = reportRepository;
            _SMTPConfiguration = sMTPConfiguration.Value;
            _userId = _httpContextAccessor.HttpContext.User.Claims?.FirstOrDefault(x => x.Type == "UserId")?.Value;
            _logger = logger;
        }

        #region User
        public async Task<User> AuthenticateAsync(UserLoginRequest entity, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByMailAsync(entity.Mail, cancellationToken);
            if (user == null)
            {
                throw new("Incorrect Log In Credentials.");
            }
            var result = _passwordHasher.VerifyHashedPassword(null, user.Password, entity.Password);
            switch(result)
            {
                case PasswordVerificationResult.SuccessRehashNeeded:
                    user.Password = _passwordHasher.HashPassword(null, entity.Password);
                    await _userRepository.UpdateAsync(user, cancellationToken);
                    return user;
                case PasswordVerificationResult.Success:
                    return user;
                default:
                    throw new("Incorrect Log In Credentials.");
                
            }
        }

        public async Task ConfirmRegistration(UserCreationConfirmationRequest entity, CancellationToken cancellationToken)
        {
            var tokenExists = _registrationRequests.TryGetValue(entity.Email, out var token);
            if (tokenExists)
            {
                var timeNow = DateTime.UtcNow.AddHours(4);
                var tokenAge = timeNow - token.CreationDate;
                if (tokenAge > TimeSpan.FromMinutes(5))
                {
                    _registrationRequests.Remove(entity.Email);
                    throw new("Registration Token Timed Out.");
                }
                if (token.Token != entity.RegistrationToken)
                {
                    throw new("Invalid Registration Token.");
                }
                var newUser = token.User.Adapt<User>();
                newUser.Password = _passwordHasher.HashPassword(null, newUser.Password);
                newUser.UserRank = Domain.Rank.User;
                newUser.UserStatus = Domain.Status.User;
                await _userRepository.AddAsync(newUser, cancellationToken);
                _logger.LogInformation($"[{DateTime.UtcNow.AddHours(4)}] User With Email {newUser.Email} Registered successfully");
            }
        }

        public Task DeleteAsync(CancellationToken cancellationToken, params object[] id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetByIdAsync(CancellationToken cancellationToken, params object[] key)
        {
            return await _userRepository.GetByIdAsync(cancellationToken, int.Parse(_userId));
        }

        public async Task RequestRegistration(UserCreationRequest entity, CancellationToken cancellationToken)
        {
            var requestExists = _registrationRequests.TryGetValue(entity.Email, out var registrationRequest);
            var exists = await _userRepository.Exists(entity.Email, entity.UserName);
            if (exists)
            {
                throw new("Email or username is already registered.");
            }
            if (requestExists)
            {
                var timeNow = DateTime.UtcNow.AddHours(4);
                var requestAge = timeNow - registrationRequest.CreationDate;
                if (requestAge > TimeSpan.FromMinutes(5))
                {
                    var newToken = Guid.NewGuid().ToString("N");
                    SMTPHelper.SendRegistrationToken(_SMTPConfiguration, entity.Email, newToken);
                    _registrationRequests[entity.Email] = (newToken, timeNow, registrationRequest.User);
                    return;
                }
                else return;
            }
            else
            {
                var newToken = Guid.NewGuid().ToString("N");
                SMTPHelper.SendRegistrationToken(_SMTPConfiguration, entity.Email, newToken);
                _registrationRequests.TryAdd(entity.Email, (newToken, DateTime.UtcNow.AddHours(4), entity));
            }
        }

        public async Task UpdateAsync(UserUpdateRequest entity, CancellationToken cancellationToken)
        {
            if(entity.NewPassword != entity.ConfirmPassword)
            {
                throw new("Confirmed password is not the same as new password.");
            }
            var user = await _userRepository.GetByIdAsync(cancellationToken, int.Parse(_userId));
            var result = _passwordHasher.VerifyHashedPassword(null, user.Password, entity.OldPassword);
            if(result == PasswordVerificationResult.Failed)
            {
                throw new("Invalid old password.");
            }
            user.Password = _passwordHasher.HashPassword(null, entity.NewPassword);
            await _userRepository.UpdateAsync(user, cancellationToken);
        }

        #endregion

        #region Definitions
        //Gets 10 random definitions (if available) 
        //Returns tuple (definition, isLikedByUser)
        public async Task<List<(DefinitionResponseModel, bool?)>> GetRandomDefinitionsForMainPage(CancellationToken cancellationToken)
        {
            var definitions = (await _definitionRepository.GetAll(cancellationToken)).Where(e => e.DefinitionState == Domain.DefinitionState.Approved);
            var definitonCount = definitions.Count();
            var result = new List<(DefinitionResponseModel, bool?)>();
            if(definitonCount < 10)
            {
                var adaptedDefinitions = definitions.ToList().Adapt<List<DefinitionResponseModel>>();
                if(_userId == null)
                {
                    foreach(var definition in adaptedDefinitions)
                    {
                        result.Add((definition, null));
                    }
                    return result;
                }
                else
                {
                    foreach(var definition in adaptedDefinitions)
                    {
                        var votes = (await _voteRepository.GetAll(cancellationToken)).Where(e => e.DefinitionId == definition.Id);
                        definition.UpvoteCount = votes.Where(e => e.Likes == true).Count();
                        definition.DownvoteCount = votes.Where(e => e.Likes == false).Count();
                        var userId = int.Parse(_userId);
                        result.Add((definition, (await _voteRepository.GetByIdAsync(cancellationToken, userId, definition.Id))?.Likes));
                    }
                    return result;
                }
            }
            var randomRange = definitonCount / 10 + 1;
            int skipCount = 0;
            List<DefinitionResponseModel> randomDefinitions = new();
            for(int i = 0; i < 10; i++)
            {
                skipCount += Random.Shared.Next(0, randomRange);
                randomDefinitions.Add(definitions.Skip(skipCount).Take(1).Adapt<DefinitionResponseModel>());
                skipCount++;
            }
            if (_userId == null)
            {
                foreach (var definition in randomDefinitions)
                {
                    result.Add((definition, null));
                }
                return result;
            }
            else
            {
                foreach (var definition in randomDefinitions)
                {
                    var userId = int.Parse(_userId);
                    result.Add((definition, (await _voteRepository.GetByIdAsync(cancellationToken, userId, definition.Id))?.Likes));
                }
                return result;
            }
        }
        public async Task<(DefinitionResponseModel, bool?)> GetRandomDefinition(CancellationToken cancellationToken)
        {
            var definitions = (await _definitionRepository.GetAll(cancellationToken)).Where(e => e.DefinitionState == Domain.DefinitionState.Approved);
            var definitonCount = definitions.Count();
            var definition = definitions.Skip(Random.Shared.Next(0, definitonCount)).Take(1).Adapt<DefinitionResponseModel>();
            var votes = (await _voteRepository.GetAll(cancellationToken)).Where(e => e.DefinitionId == definition.Id);
            definition.UpvoteCount = votes.Where(e => e.Likes == true).Count();
            definition.DownvoteCount = votes.Where(e => e.Likes == false).Count();
            if (_userId == null)
            {
                return (definition, null);
            }
            else
            {
                return (definition, (await _voteRepository.GetByIdAsync(cancellationToken, int.Parse(_userId), definition.Id))?.Likes);
            }
        }
        public async Task SuggestDefinition(DefinitionCreationRequest entity, CancellationToken cancellationToken)
        {
            var exists = await _definitionRepository.Exists(entity.GeorgianTitle, entity.EnglishTitle);
            if (exists == null)
            {
                throw new("Definition with such title already exists.");
            }
            var user = await _userRepository.GetByIdAsync(cancellationToken, int.Parse(_userId));
            var userSuggestionsCount = (await _definitionRepository.GetAll(cancellationToken)).Where(e => e.Author.Id == user.Id && e.DefinitionState == Domain.DefinitionState.Pending).Count();
            if(userSuggestionsCount >= 5) 
            {
                throw new("Can't create more than 5 definition suggestions at the same time.");
            }
            var definition = entity.Adapt<Definition>();
            definition.Author = user;
            definition.DefinitionState = Domain.DefinitionState.Pending;
            await _definitionRepository.AddAsync(definition, cancellationToken);
        }
        public async Task SetVote(Vote vote, CancellationToken cancellationToken)
        {
            var existingVote = await _voteRepository.GetByIdAsync(cancellationToken, vote.UserId, vote.DefinitionId);
            if (existingVote == null)
            {
                if(vote.Likes != null)
                {
                    await _voteRepository.AddAsync(vote, cancellationToken);
                }
                else
                {
                    throw new("Can't remove vote that doesn't exist.");
                }
            }
            else
            {
                if(vote.Likes == null)
                {
                    await _voteRepository.DeleteAsync(cancellationToken, vote.UserId, vote.DefinitionId);
                }
                else
                {
                    if(vote.Likes != existingVote.Likes)
                    {
                        await _voteRepository.UpdateAsync(vote, cancellationToken);
                    }
                }
            }
        }
        public async Task SendReport(ReportCreationRequest report, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(cancellationToken, int.Parse(_userId));
            var userReportsCount = (await _reportRepository.GetAll(cancellationToken)).Where(e => e.Author.Id == user.Id).Count();
            if(userReportsCount > 5)
            {
                throw new("Can't have more than 5 reports at the same time.");
            }
            Report newReport = report.Adapt<Report>();
            newReport.ReportState = Domain.ReportState.Pending;
            newReport.Author = user;
            user.Reports.Add(newReport);
            await _userRepository.UpdateAsync(user, cancellationToken);
        }

        public async Task<(DefinitionResponseModel, bool?)> GetDefinition(int definitionId, CancellationToken cancellationToken)
        {
            var definition = (await _definitionRepository.GetByIdAsync(cancellationToken, definitionId)).Adapt<DefinitionResponseModel>();
            var votes = (await _voteRepository.GetAll(cancellationToken)).Where(e => e.DefinitionId == definitionId);
            definition.UpvoteCount = votes.Where(e => e.Likes == true).Count();
            definition.DownvoteCount = votes.Where(e => e.Likes == false).Count();

            if (_userId == null)
            {
                return (definition, null);
            }
            else
            {
                return (definition, (await _voteRepository.GetByIdAsync(cancellationToken, int.Parse(_userId), definition.Id))?.Likes);
            }
        }
        #endregion
    }
}
