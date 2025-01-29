using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Application.Definitions.Requests;
using Terminal.Application.Definitions.Responses;
using Terminal.Application.Reports.Requests;
using Terminal.Application.Users.Requests;
using Terminal.Domain.Models;
using Terminal.Domain.Utilities;

namespace Terminal.Application.Users
{
    public interface IUserService
    {
        public Task UpdateAsync(UserUpdateRequest entity, CancellationToken cancellationToken);
        public Task<User> GetByIdAsync(CancellationToken cancellationToken, params object[] key);
        public Task DeleteAsync(CancellationToken cancellationToken, params object[] id);
        public Task<User> AuthenticateAsync(UserLoginRequest entity, CancellationToken cancellationToken);
        public Task RequestRegistration(UserCreationRequest entity, CancellationToken cancellationToken);
        public Task ConfirmRegistration(UserCreationConfirmationRequest entity, CancellationToken cancellationToken);
        public Task SuggestDefinition(DefinitionCreationRequest entity, CancellationToken cancellationToken);
        public Task<List<(DefinitionResponseModel, bool?)>> GetRandomDefinitionsForMainPage(CancellationToken cancellationToken);
        public Task<(DefinitionResponseModel, bool?)> GetRandomDefinition(CancellationToken cancellationToken);
        public Task<(DefinitionResponseModel, bool?)> GetDefinition(int definitionId, CancellationToken cancellationToken);
        public Task SetVote(Vote vote, CancellationToken cancellationToken);
        public Task SendReport(ReportCreationRequest report, CancellationToken cancellationToken);
    }
}
