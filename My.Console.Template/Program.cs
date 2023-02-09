var hostBuilder = CreateHostBuilder(args);

// Register services directly with Autofac here. Don't
// call builder.Populate(), that happens in AutofacServiceProviderFactory.
hostBuilder.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new MyApplicationModule()));

using var host = hostBuilder.Build();

using var scope = host.Services.CreateScope();

var services = scope.ServiceProvider;

var app = services.GetRequiredService<AppRunner>();

try
{
	var running = await app.RunAsync(args).ConfigureAwait(false);

	return 0;
}
catch (Exception ex)
{
	Log.Fatal(ex, "Application terminated unexpectedly");

	return 1;
}
finally
{
	Log.CloseAndFlush();
}

static IHostBuilder CreateHostBuilder(string[] args)
{
	return Host.CreateDefaultBuilder(args)
		.ConfigureHostConfiguration(builder =>
		{
			if (args.Any())
				// environment from command line
				// e.g.: dotnet run --environment "Staging"
				builder.AddCommandLine(args);
		})
		.ConfigureAppConfiguration((context, builder) =>
		{
			var env = context.HostingEnvironment;

			builder.SetBasePath(AppContext.BaseDirectory)
				.AddJsonFile("appsettings.json", false)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
				// Override config by env, using like Logging:Level or Logging__Level;
				.AddEnvironmentVariables();
		}).ConfigureLogging((context, builder) =>
		{
			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(context.Configuration)
				.Enrich.FromLogContext()
				.WriteTo.Console()
				.CreateBootstrapLogger();

			builder.AddSerilog();
		})
		.ConfigureServices((context, services) =>
		{
			services.TryAddSingleton<AppRunner>();
		})
		.UseServiceProviderFactory(context => new AutofacServiceProviderFactory())
		.UseSerilog();
}