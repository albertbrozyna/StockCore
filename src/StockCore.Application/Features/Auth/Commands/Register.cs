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
    public static class Register
    {
        public record Command(string FirstName, string LastName, string Email, string Password, UserRole Role, string PhoneNumber,string CompanyJoinCode) : IRequest<Result<Response>>;
        public record Response(Guid UserId);

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required").MaximumLength(50);
                RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required").MaximumLength(50);
                RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required").EmailAddress().WithMessage("Incorrect email format").MaximumLength(100);
                RuleFor(x => x.Role).IsInEnum().WithMessage("Role is incorrect");
                RuleFor(x => x.Password).NotEmpty().WithMessage("Password cannot be empty!").MinimumLength(8).WithMessage("Password have to be at least 8 letters long");
            }
        }

        public class Handler(IRegisterUserService registerUserService, IVerificationCodeService verificationCodeService, IEmailSender emailSender,IAppDbContext appDbContext) : IRequestHandler<Command, Result<Response>>
        {
            public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
            {
                var company = await appDbContext.Companies.FirstOrDefaultAsync(company => company.CompanyJoinCode == request.CompanyJoinCode,cancellationToken);

                if(company == null)
                {
                    return Result.Failure<Response>("Incorrect company join code");
                }

                User newUser = new(request.FirstName, request.LastName, request.Email, request.PhoneNumber,company.TenantId);

                var result = await registerUserService.RegisterUser(newUser, request.Password, request.Role).Map(user => new Response(user.Id));

                if (result.IsFailure)
                {
                    return result;
                }

                var code = await verificationCodeService.GenerateCode(newUser.Id, cancellationToken);

                var topic = "Verify your account";

                var content = $"Hello, your verification code to StockCoreApp is: {code}";

                await emailSender.SendEmail(topic, content, newUser.Email);

                return result;
            }
        }
    }
}