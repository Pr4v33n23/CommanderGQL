using CommanderGQL.Data;
using CommanderGQL.Models;

namespace CommanderGQL.GraphQL.Platforms;

public class PlatformType : ObjectType<Platform>
{
    protected override void Configure(IObjectTypeDescriptor<Platform> descriptor)
    {
        descriptor.Description("Represents a software cmd line");
        descriptor
            .Field(p => p.LicenseKey).Ignore();
        descriptor
            .Field(p => p.Commands)
            .ResolveWith<Resolvers>(p => p.GetCommands(default!, default!))
            .UseDbContext<AppDbContext>()
            .Description("List of  available commands");
    }

    private class Resolvers
    {
        public IQueryable<Command> GetCommands([Parent]Platform platform, [ScopedService] AppDbContext context)
        {
            return context.Commands.Where(p => p.PlatformId == platform.Id);
        }
    }
}