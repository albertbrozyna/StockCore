using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StockCore.Application.Common.Interfaces;
using StockCore.Domain.Entities;
using StockCore.Domain.Enums;

namespace StockCore.Application.Features.Users.Queries.GetAllUsers
{
    public static class GetAllUsers
    {
        public record Query() : IRequest<Result<Response>>;

        public record UserDto
        {
            public Guid Id { get; init; }
            public string FirstName { get; init; }
            public string LastName { get; init; }
            public string Email { get; init; }
            public UserRole Role { get; init; }
            public bool IsActive { get; init; }
            public bool IsDeleted { get; init; }
            public DateTime CreatedAt { get; init; }
            public DateTime LastLogin { get; init; }
            public string? PhoneNumber { get; init; }
            public string AvatarUrl { get; init; }
        }

        public record Response(List<UserDto> Users);

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
            }
        }

        public class ResponseProfile : Profile
        {
            public ResponseProfile()
            {
                CreateMap<User, UserDto>();
            }
        }

        public class Handler(IAppDbContext appDbContext, IMapper mapper) : IRequestHandler<Query, Result<Response>>
        {
            private readonly IAppDbContext _appDbContext = appDbContext;
            private readonly IMapper _mapper = mapper;

            public async Task<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
            {
                var users = await _appDbContext.Users.AsNoTracking().ProjectTo<UserDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

                return Result.Success(new Response(users));
            }
        }
    }
}
