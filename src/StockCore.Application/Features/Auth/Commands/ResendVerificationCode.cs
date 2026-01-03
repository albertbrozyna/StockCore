using CSharpFunctionalExtensions;
using FluentValidation;
using StockCore.Domain.Entities;
using StockCore.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StockCore.Application.Common.Interfaces;
using StockCore.Application.Common.Interfaces.Authentication;

namespace StockCore.Application.Features.Auth.Commands
{
    public static class ResendVerificationCode
    {
        public record Command(Guid UserId) : IRequest<Result<Response>>; 
        public record Response();

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.UserId).NotEmpty().WithMessage("User Id cannot be empty");
            }
        }

        public class Handler(IVerificationCodeService verificationCodeService, IEmailSender emailSender,IIdentityService identityService) : IRequestHandler<Command, Result<Response>>
        {
            public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await identityService.GetUserById(request.UserId.ToString());

                if(user is null)
                {
                    return Result.Failure<Response>("User with this id doesn't exist");
                }

                var code = await verificationCodeService.GenerateCode(user.Id, cancellationToken);

                var topic = "Verify your account";

                var content = $"Hello, your verification code to StockCoreApp is: {code}";

                await emailSender.SendEmail(topic, content, user.Email);

                return Result.Success(new Response());
            }
        }
    }
}