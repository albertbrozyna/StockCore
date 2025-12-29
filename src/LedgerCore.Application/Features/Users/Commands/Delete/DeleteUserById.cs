using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StockCore.Application.Common.Interfaces;


namespace StockCore.Application.Features.Users.Commands.Delete
{

    public static class DeleteUser
    {
        public record Command(Guid UserId) : IRequest<Result<Response>>;
        public record Response();

        public class Validator : AbstractValidator<DeleteUser.Command>
        {
            public Validator()
            {
                RuleFor(x => x.UserId).NotEmpty().WithMessage("User id cannot be empty.");
            }
        }

        public class DeleteUserHandler : IRequestHandler<Command, Result<Response>>
        {
            private readonly IAppDbContext _context;
            public DeleteUserHandler(IAppDbContext context)
            {
                _context = context;
            }

            public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
            {
                var userOrNull = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

                var result = Maybe.From(userOrNull)
                    .ToResult($"User with id {request.UserId} was not found.")
                    .Tap(user => user.DeleteUser());

                if (result.IsFailure)
                {
                    return result.ConvertFailure<Response>();
                }

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success(new Response());
            }
        }
    }
}
