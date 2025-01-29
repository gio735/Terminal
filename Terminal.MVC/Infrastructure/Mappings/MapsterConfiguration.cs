using Mapster;
using System.Diagnostics.Metrics;
using System.Net;
using Terminal.Application.Definitions.Requests;
using Terminal.Application.Definitions.Responses;
using Terminal.Application.References.Responses;
using Terminal.Application.Users.Requests;
using Terminal.Application.Users.Responses;
using Terminal.Domain.Models;
using Terminal.MVC.Models;

namespace Terminal.MVC.Infrastructure.Mappings
{
    public static class MapsterConfiguration
    {
        public static IServiceCollection RegisterMaps(this IServiceCollection services)
        {
            TypeAdapterConfig<UserCreationRequest, User>
            .NewConfig();
            TypeAdapterConfig<User, UserResponseModel>
            .NewConfig();
            TypeAdapterConfig<User, DetailedUserResponseModel>
            .NewConfig();

            TypeAdapterConfig<DefinitionCreationRequest, Definition>
            .NewConfig();
            TypeAdapterConfig<Definition, DefinitionResponseModel>
            .NewConfig();

            TypeAdapterConfig<Reference, ReferenceResponseModel>
            .NewConfig();


            TypeAdapterConfig<UserSignInModel, UserLoginRequest>
            .NewConfig();
            return services;
        }
    }
}
