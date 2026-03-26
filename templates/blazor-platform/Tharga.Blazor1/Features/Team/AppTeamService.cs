using MongoDB.Bson;
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
            Id = ObjectId.GenerateNewId(),
            Key = teamKey,
            Name = name,
            Members = [new TeamMember
            {
                Key = user.Key,
                LastSeen = DateTime.UtcNow,
                AccessLevel = AccessLevel.Owner,
                State = MembershipState.Member
            }]
        });
    }

    protected override Task<TeamMember> CreateTeamMember(InviteUserModel model)
    {
        return Task.FromResult(new TeamMember
        {
            Key = null, //NOTE: This value will be assigned on registration.
            Name = model.Name,
            Invitation = new Invitation
            {
                EMail = model.Email,
                InviteKey = Guid.NewGuid().ToString(),
                InviteTime = DateTime.UtcNow
            },
            AccessLevel = model.AccessLevel,
            State = MembershipState.Invited
        });
    }
}
