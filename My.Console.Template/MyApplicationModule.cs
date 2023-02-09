namespace My.Console.Template;

public class MyApplicationModule : Module
{
	protected override void Load(ContainerBuilder builder)
	{
		//builder.RegisterAssemblyTypes(Assembly.Load(nameof(Microsoft)))
		//	.Where(t => t.Namespace?.Contains("Practicing.Services") == true)
		//	.As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));
	}
}