using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Application.Definitions.Responses;
using Terminal.Domain.Models;

namespace Terminal.Application.Managers
{
    public interface IManagerService
    {
        public Task Demote(int userId, CancellationToken cancellationToken);
        public Task Promote(int userId, CancellationToken cancellationToken);
        public Task DeleteUser(int userId, CancellationToken cancellationToken);
        public Task<List<DefinitionResponseModel>> GetPendingDefinitions(CancellationToken cancellationToken);
        public Task ApprovePendingDefinition(int definitionId, CancellationToken cancellationToken);
        public Task DeclinePendingDefinition(int definitionId, CancellationToken cancellationToken);
        public Task<List<Report>> GetPendingReports(CancellationToken cancellationToken);
        public Task ApprovePendingReport(int reportId, CancellationToken cancellationToken);
        public Task DeclinePendingReport(int reportId, CancellationToken cancellationToken);

        public Task<DefinitionResponseModel> GetDefinition(int definitionId, CancellationToken cancellationToken);
        public Task<Report> GetReport(int reportId, CancellationToken cancellationToken);
        public Task AddSimilarToDefinition(int definitionId, int similarDefinitionId, CancellationToken cancellationToken);
        public Task RemoveSimilarToDefinition(int definitionId, int similarDefinitionId, CancellationToken cancellationToken);
        public Task AddReferenceToDefinition(int definitionId, int referenceId, CancellationToken cancellationToken);
        public Task RemoveReferenceToDefinition(int definitionId, int referenceId, CancellationToken cancellationToken);
        public Task UpdateDefinition(Definition entity, CancellationToken cancellationToken);
    }
}
