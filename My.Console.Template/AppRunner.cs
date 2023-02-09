namespace My.Console.Template;

public class AppRunner
{
	public async Task<bool> RunAsync(string[] args)
	{
		Log.Information("Processing...");

		await Task.Delay(TimeSpan.FromSeconds(3)).ConfigureAwait(false);

		Log.Information("Completed.");

		return true;
	}
}