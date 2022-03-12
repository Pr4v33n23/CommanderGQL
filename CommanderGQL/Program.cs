using CommanderGQL.Data;
using CommanderGQL.GraphQL;
using Microsoft.EntityFrameworkCore;
using GraphQL.Server.Ui.Voyager;
using CommanderGQL.GraphQL.Platforms;
using CommanderGQL.GraphQL.Commands;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddPooledDbContextFactory<AppDbContext>(options => options.UseSqlServer(
    configuration.GetConnectionString("CommandConnStr")
));
builder.Services.AddGraphQLServer()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                .AddSubscriptionType<Subscription>()
                .AddType<PlatformType>()
                .AddType<CommandType>()
                .AddSorting()
                .AddFiltering()
                .AddProjections()
                .AddInMemorySubscriptions();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseWebSockets();
app.UseHttpsRedirection();
app.MapControllers();
app.UseAuthorization();
app.MapGraphQL();
app.UseGraphQLVoyager(new VoyagerOptions()
{

    GraphQLEndPoint = "/graphql"
}, "/graphql-voyager");
app.Run();