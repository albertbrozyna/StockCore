using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StockCore.Application.Common.Interfaces;

namespace StockCore.Application.Features.Users.Commands
{
    public static class AcceptUserRequestToCompany
    {
        public record Command(Guid UserId) : IRequest<Result<Response>>;
        public record Response();
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.UserId).NotEmpty().WithName("User id cannot be empty");
            }
        }

        public class AcceptUserRequestToCompanyHandler : IRequestHandler<Command, Result<Response>>
        {
            private readonly IAppDbContext _appDbContext;

            public AcceptUserRequestToCompanyHandler(IAppDbContext appDbContext)
            {
                _appDbContext = appDbContext;
            }

            public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _appDbContext.Users.FirstOrDefaultAsync(user => user.Id == request.UserId);

                return await user.AsMaybe().ToResult($"User with Id: {request.UserId} was not found")
                    .Ensure(user => user.IsAcceptedByCompany, $"User with Id {request.UserId} is already accepted by company")
                    .Tap(user => user.AcceptUserToCompany())
                    .Tap(async _ => await _appDbContext.SaveChangesAsync(cancellationToken))
                    .Map(user => new Response());
            }
        }
    }
}
