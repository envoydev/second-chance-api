using MapsterMapper;
using MediatR;
using SecondChance.Application.CQRS.Users.Dtos;
using SecondChance.Application.Persistant;
using SecondChance.Domain.Entities;

namespace SecondChance.Application.CQRS.Users.Commands.CreateUser;

public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserResult>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(IApplicationDbContext applicationDbContext,
        IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<UserResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var newUser = _mapper.Map<User>(request);

        _applicationDbContext.Users.Add(newUser);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        var projectResult = _mapper.Map<UserResult>(newUser);

        return projectResult;
    }
}