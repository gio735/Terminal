using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Application.Definitions.Repositories;
using Terminal.Application.Definitions.Responses;
using Terminal.Application.Helpers;
using Terminal.Application.Reports.Repositories;
using Terminal.Application.Users.Repositories;
using Terminal.Application.Users;
using Terminal.Application.Votes.Repositories;
using Terminal.Domain.Models;
using Terminal.Application.References.Repositories;
using Terminal.Domain;
using Mapster;
using Terminal.Application.Definitions.Requests;

namespace Terminal.Application.Managers
{
    public class ManagerService : IManagerService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly IDefinitionRepository _definitionRepository;
        private readonly IVoteRepository _voteRepository;
        private readonly IReportRepository _reportRepository;
        private readonly IReferenceRepository _referenceRepository;
        private readonly string _userId;
        private readonly ILogger<ManagerService> _logger;

        public static List<Reference> References = null;
        public ManagerService(IHttpContextAccessor httpContextAccessor,
                            IUserRepository userRepository,
                            IDefinitionRepository definitionRepository,
                            IVoteRepository voteRepository,
                            IReportRepository reportRepository,
                            IReferenceRepository referenceRepository,
                            ILogger<ManagerService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _definitionRepository = definitionRepository;
            _voteRepository = voteRepository;
            _reportRepository = reportRepository;
            _referenceRepository = referenceRepository;
            _userId = _httpContextAccessor.HttpContext.User.Claims?.FirstOrDefault(x => x.Type == "UserId")?.Value;
            _logger = logger;
        }
        public async Task ApprovePendingDefinition(int definitionId, CancellationToken cancellationToken)
        {
            var definition = await _definitionRepository.GetByIdAsync(cancellationToken, definitionId);
            var user = await _userRepository.GetByIdAsync(cancellationToken, int.Parse(_userId));
            if (definition.DefinitionState != Domain.DefinitionState.Pending)
            {
                throw new("This definition is not in pending state.");
            }
            if(user.UserRank < Domain.Rank.Manager)
            {
                throw new("Only manager or higher rank can approve new definitions.");
            }
            await FindAndSetReferences(definition);
            definition.DefinitionState = Domain.DefinitionState.Approved;
            await _definitionRepository.UpdateAsync(definition, cancellationToken);
            _logger.LogInformation($"[ {DateTime.UtcNow.AddHours(4)} ] Manager with ID {_userId} approved definition with ID {definitionId}.");
            Reference newReference = new();
            newReference.EnglishTitle = definition.EnglishTitle;
            newReference.GeorgianTitle = definition.GeorgianTitle;
            newReference.ReferenceId = definition.Id;
            await _referenceRepository.AddAsync(newReference, CancellationToken.None);
            References.Add(newReference);
        }

        // Something horrible, please don't do at home!
        private Task FindAndSetReferences(Definition target)
        {
            var englishWords = target.EnglishContent.Replace(".", "").Replace("!", "").Replace(",", "").Split(' ');
            var georgianWords = target.GeorgianContent.Replace(".", "").Replace("!", "").Replace(",", "").Split(' ');
            foreach( var englishWord in englishWords )
            {
                var references = References.Where(e => e.EnglishTitle.Contains(englishWord)).ToList();
                foreach(var reference in references)
                {
                    if(target.EnglishContent.Contains(reference.EnglishTitle, StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (!target.References.Contains(reference))
                        {
                            target.References.Add(reference);
                        }
                    }
                }
            }
            foreach (var georgianWord in georgianWords)
            {
                var references = References.Where(e => e.EnglishTitle.Contains(georgianWord)).ToList();
                foreach (var reference in references)
                {
                    if (target.GeorgianContent.Contains(reference.GeorgianTitle, StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (!target.References.Contains(reference))
                        {
                            target.References.Add(reference);
                        }
                    }
                }
            }
            return Task.CompletedTask;
        }

        public async Task ApprovePendingReport(int reportId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(cancellationToken, int.Parse(_userId));
            var report = await _reportRepository.GetByIdAsync(cancellationToken, reportId);
            if(user.UserRank < Domain.Rank.Manager)
            {
                throw new("Only manager or higher rank can approve report.");
            }
            report.ReportState = Domain.ReportState.Approved;
            report.RevieverId = int.Parse(_userId);
            await _reportRepository.UpdateAsync(report, cancellationToken);
        }

        public async Task DeclinePendingDefinition(int definitionId, CancellationToken cancellationToken)
        {
            var definition = await _definitionRepository.GetByIdAsync(cancellationToken, definitionId);
            var user = await _userRepository.GetByIdAsync(cancellationToken, int.Parse(_userId));
            if (definition.DefinitionState != Domain.DefinitionState.Pending)
            {
                throw new("This definition is not in pending state.");
            }
            if (user.UserRank < Domain.Rank.Manager)
            {
                throw new("Only manager or higher rank can reject new definitions.");
            }
            definition.DefinitionState = Domain.DefinitionState.Rejected;
            _logger.LogInformation($"[ {DateTime.UtcNow.AddHours(4)} ] Manager with ID {_userId} rejected definition with ID {definitionId}.");
            await _definitionRepository.UpdateAsync(definition, cancellationToken);
        }

        public async Task DeclinePendingReport(int reportId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(cancellationToken, int.Parse(_userId));
            var report = await _reportRepository.GetByIdAsync(cancellationToken, reportId);
            if (user.UserRank < Domain.Rank.Manager)
            {
                throw new("Only manager or higher rank can reject report.");
            }
            report.ReportState = Domain.ReportState.Rejected;
            report.RevieverId = int.Parse(_userId);
            await _reportRepository.UpdateAsync(report, cancellationToken);
        }

        public async Task DeleteUser(int userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(cancellationToken, int.Parse(_userId));
            if(user.UserRank < Domain.Rank.Manager)
            {
                throw new("Only manager or higher rank can delete user account.");
            }

            var target = await _userRepository.GetByIdAsync(cancellationToken, user.Id);
            if(user.UserRank <= target.UserRank)
            {
                throw new("You can only ban those with rank lower than you.");
            }
            target.State = Domain.State.Deleted;
            target.DeletionDate = DateTime.UtcNow.AddHours(4);
            await _userRepository.UpdateAsync(target, cancellationToken);
            _logger.LogInformation($"[ {DateTime.UtcNow.AddHours(4)} ] Manager with ID {_userId} deleted user account with ID {userId}.");
        }
        public async Task Promote(int userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(cancellationToken, int.Parse(_userId));
            if (user.UserRank < Domain.Rank.Manager)
            {
                throw new("Only manager or higher rank can promote user.");
            }
            var target = await _userRepository.GetByIdAsync(cancellationToken, userId);
            if(user.UserRank - 1 <= target.UserRank)
            {
                throw new("You can't promote someone to rank equal or higher than yours.");
            }
            if(target.UserRank == Rank.User)
            {
                target.UserStatus = Status.Management;
            }
            target.UserRank++;
            await _userRepository.UpdateAsync(target, cancellationToken);
            _logger.LogInformation($"[ {DateTime.UtcNow.AddHours(4)} ] Manager with ID {_userId} promoted user with ID {userId} to {target.UserRank}.");
        }

        public async Task Demote(int userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(cancellationToken, int.Parse(_userId));
            if (user.UserRank < Domain.Rank.Manager)
            {
                throw new("Only manager or higher rank can demote user.");
            }
            var target = await _userRepository.GetByIdAsync(cancellationToken, userId);
            if (user.UserRank <= target.UserRank || target.UserRank == Rank.User)
            {
                throw new("You can't demote user with same or higher rank than you or with User rank.");
            }
            target.UserRank--;
            if(target.UserRank == Rank.User)
            {
                target.UserStatus = Status.User;
            }
            await _userRepository.UpdateAsync(target, cancellationToken);
            _logger.LogInformation($"[ {DateTime.UtcNow.AddHours(4)} ] Manager with ID {_userId} demoted user with ID {userId} to {target.UserRank}.");
        }

        public async Task<DefinitionResponseModel> GetDefinition(int definitionId, CancellationToken cancellationToken)
        {
            return (await _definitionRepository.GetByIdAsync(cancellationToken, definitionId)).Adapt<DefinitionResponseModel>();
        }

        public async Task<List<DefinitionResponseModel>> GetPendingDefinitions(CancellationToken cancellationToken)
        {
            return (await _definitionRepository.GetAll(cancellationToken))
                        .Where(e => e.DefinitionState == DefinitionState.Pending).ToList()
                        .Adapt<List<DefinitionResponseModel>>();
        }

        public async Task<List<Report>> GetPendingReports(CancellationToken cancellationToken)
        {
            return (await _reportRepository.GetAll(cancellationToken))
                        .Where(e => e.ReportState == ReportState.Pending).ToList()
                        .Adapt<List<Report>>();
        }

        public async Task<Report> GetReport(int reportId, CancellationToken cancellationToken)
        {
            return await _reportRepository.GetByIdAsync(cancellationToken, reportId);
        }


        public async Task AddSimilarToDefinition(int definitionId, int similarDefinitionId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(cancellationToken, int.Parse(_userId));
            if (user.UserRank < Domain.Rank.Manager)
            {
                throw new("Only manager or higher rank can add similar to definition.");
            }
            var definition = await _definitionRepository.GetByIdAsync(cancellationToken, definitionId);
            if(definition.Similars.FirstOrDefault(e => e.Id == similarDefinitionId) != null)
            {
                throw new("This definition is already in the similars list.");
            }
            var similar = await _definitionRepository.GetByIdAsync(cancellationToken, similarDefinitionId);
            definition.Similars.Add(similar);
            await _definitionRepository.UpdateAsync(definition, cancellationToken);
            _logger.LogInformation($"[ {DateTime.UtcNow.AddHours(4)} ] Manager with ID {_userId} added similar with ID {similarDefinitionId} to definition with ID {definitionId}.");
        }

        public async Task RemoveSimilarToDefinition(int definitionId, int similarDefinitionId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(cancellationToken, int.Parse(_userId));
            if (user.UserRank < Domain.Rank.Manager)
            {
                throw new("Only manager or higher rank can remove similar from definition.");
            }
            var definition = await _definitionRepository.GetByIdAsync(cancellationToken, definitionId);
            var target = definition.Similars.FirstOrDefault(e => e.Id == similarDefinitionId);
            if (target == null)
            {
                throw new("This definition is not in the similars list.");
            }
            definition.Similars.Remove(target);
            await _definitionRepository.UpdateAsync(definition, cancellationToken);
            _logger.LogInformation($"[ {DateTime.UtcNow.AddHours(4)} ] Manager with ID {_userId} removed similar with ID {similarDefinitionId} from definition with ID {definitionId}.");
        }

        public async Task AddReferenceToDefinition(int definitionId, int referenceId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(cancellationToken, int.Parse(_userId));
            if (user.UserRank < Domain.Rank.Manager)
            {
                throw new("Only manager or higher rank can add similar to definition.");
            }
            var definition = await _definitionRepository.GetByIdAsync(cancellationToken, definitionId);
            if (definition.References.FirstOrDefault(e => e.Id == referenceId) != null)
            {
                throw new("This reference is already in the reference list.");
            }
            var reference = await _referenceRepository.GetByIdAsync(cancellationToken, referenceId);
            definition.References.Add(reference);
            await _definitionRepository.UpdateAsync(definition, cancellationToken);
            _logger.LogInformation($"[ {DateTime.UtcNow.AddHours(4)} ] Manager with ID {_userId} added reference with ID {referenceId} to definition with ID {definitionId}.");
        }

        public async Task RemoveReferenceToDefinition(int definitionId, int referenceId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(cancellationToken, int.Parse(_userId));
            if (user.UserRank < Domain.Rank.Manager)
            {
                throw new("Only manager or higher rank can add similar to definition.");
            }
            var definition = await _definitionRepository.GetByIdAsync(cancellationToken, definitionId);
            var target = definition.References.FirstOrDefault(e => e.Id == referenceId);
            if (target == null)
            {
                throw new("This reference is already in the reference list.");
            }
            definition.References.Remove(target);
            await _definitionRepository.UpdateAsync(definition, cancellationToken);
            _logger.LogInformation($"[ {DateTime.UtcNow.AddHours(4)} ] Manager with ID {_userId} removed reference with ID {referenceId} from definition with ID {definitionId}.");
        }

        public async Task UpdateDefinition(DefinitionUpdateRequest entity, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(cancellationToken, int.Parse(_userId));
            if (user.UserRank < Domain.Rank.Manager)
            {
                throw new("Only manager or higher rank can add similar to definition.");
            }
            var definition = await _definitionRepository.GetByIdAsync(cancellationToken, entity.Id);
            _logger.LogInformation($"[ {DateTime.UtcNow.AddHours(4)} ] Manager with ID {_userId} updated definition with ID {entity.Id}:" +
                                    $"\n{definition.GeorgianTitle} -> {entity.GeorgianTitle}" +
                                    $"\n{definition.EnglishTitle} -> {entity.EnglishTitle}" +
                                    $"\n{definition.GeorgianContent} -> {entity.GeorgianContent}" +
                                    $"\n{definition.EnglishContent} -> {entity.EnglishContent}");
            definition.GeorgianTitle = entity.GeorgianTitle;
            definition.EnglishTitle = entity.EnglishTitle;
            definition.GeorgianContent = entity.GeorgianContent;
            definition.EnglishContent = entity.EnglishContent;
            await _definitionRepository.UpdateAsync(definition, cancellationToken);
        }
    }
}
