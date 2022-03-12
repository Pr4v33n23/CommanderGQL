using CommanderGQL.Data;
using CommanderGQL.Models;

namespace CommanderGQL.GraphQL.Commands;

public class CommandType : ObjectType<Command>
{

    protected override void Configure(IObjectTypeDescriptor<Command> descriptor)
    {
        descriptor.Description("Represents any executable command");
        descriptor
           .Field(p => p.Platform)
           .ResolveWith<Resolvers>(p => p.GetPlatforms(default!, default!))
           .UseDbContext<AppDbContext>()
           .Description("Platforms to which commands belong");

    }

    private class Resolvers
    {
        public Platform GetPlatforms([Parent] Command command, [ScopedService] AppDbContext context)
        {
            return context.Platforms.FirstOrDefault(p => p.Id == command.PlatformId);
        }
    }
}
