using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("username", secret: true);
var password = builder.AddParameter("password", secret: true);

var postgres = builder
    .AddPostgres("postgres", username, password)
    .WithDataVolume(isReadOnly: false);

var commandApi = builder
    .AddProject<CourseManagement_Infrastructure_CommandApi>("command-api")
    .WithReference(postgres)
    .WaitFor(postgres);

commandApi.WithUrl($"{commandApi.GetEndpoint("https")}/swagger", "Command API Swagger UI");

var queryApi = builder
    .AddProject<CourseManagement_Infrastructure_QueryApi>("query-api")
    .WithReference(postgres)
    .WaitFor(postgres);

queryApi.WithUrl($"{queryApi.GetEndpoint("https")}/swagger", "Query API Swagger UI");

builder
    .AddProject<CourseManagement_Infrastructure_Frontend>("frontend")
    .WithExternalHttpEndpoints()
    .WithReference(commandApi)
    .WaitFor(commandApi)
    .WithReference(queryApi)
    .WaitFor(queryApi);

builder.Build().Run();