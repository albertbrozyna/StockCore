using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StockCore.Application.Common.Interfaces;
using StockCore.Domain.Enums;

namespace StockCore.Application.Features.Users.Commands
{
    public static class AddUserToRole
    {
        public record Command(Guid UserId, UserRole NewRole) : IRequest<Result<Response>>;
        public record Response();
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.UserId).NotEmpty().WithName("User id cannot be empty");
            }
        }

        public class DeleteUserFromRoleHandler(IIdentityService identityService) : IRequestHandler<Command, Result<Response>>
        {
            public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = await identityService.AddRoleToUser(request.UserId, request.NewRole,cancellationToken);

                if (result.IsFailure)
                {
                    return Result.Failure<Response>(result.Error);
                }
                return Result.Success(new Response());
            }
        }
    }

}
