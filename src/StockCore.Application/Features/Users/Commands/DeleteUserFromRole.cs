using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StockCore.Application.Common.Interfaces;
using StockCore.Domain.Enums;

namespace StockCore.Application.Features.Users.Commands
{
    public static class DeleteUserFromRole
    {
        public record Command(Guid UserId, UserRole RoleToDelete) : IRequest<Result<Response>>;
        public record Response();
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.UserId).NotEmpty().WithName("User id cannot be empty");
            }
        }

        public class DeleteUserFromRoleHandler : IRequestHandler<Command, Result<Response>>
        {
            private readonly IIdentityService _identityService;

            public DeleteUserFromRoleHandler(IIdentityService identityService)
            {
                _identityService = identityService;
            }

            public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = await _identityService.DeleteRoleFromUser(request.UserId, request.RoleToDelete, cancellationToken);

                if (result.IsFailure)
                {
                    return Result.Failure<Response>(result.Error);
                }
                return Result.Success(new Response());
            }
        }
    }

}
