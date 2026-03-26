using Tharga.MongoDB;
using Tharga.Team;
using Tharga.Team.MongoDB;

namespace Tharga.Blazor1.Features.Team;

public class AppTeamService(
    IUserService userService,
    ITeamRepository<TeamEntity, TeamMember> teamRepository,
    IMongoDbServiceFactory mongoDbServiceFactory)
    : TeamServiceRepositoryBase<TeamEntity, TeamMember>(userService, teamRepository, mongoDbServiceFactory)
{
    protected override Task<TeamEntity> CreateTeam(string teamKey, string name, IUser user)
    {
        return Task.FromResult(new TeamEntity
        {
            Key = teamKey,
            Name = name,
            Members = [new TeamMember
            {
                Key = user.Key,
                AccessLevel = AccessLevel.Owner,
                State = MembershipState.Member
            }]
        });
    }

    protected override Task<TeamMember> CreateTeamMember(InviteUserModel model)
    {
        return Task.FromResult(new TeamMember
        {
            Key = model.Email,
            AccessLevel = model.AccessLevel,
            State = MembershipState.Invited
        });
    }
}
