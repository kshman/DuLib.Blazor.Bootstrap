using Markdig;

namespace Du.Blazor.Services;

public class MarkdownProvider : IMarkdownProvider
{
	private readonly MarkdownPipeline _pipeline;

	public MarkdownProvider() =>
		_pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

	public MarkdownPipeline Pipeline => _pipeline;

	public string GetHtml(string markdown) =>
		Markdown.ToHtml(markdown, _pipeline);

	public MarkupString GetMarkup(string markdown) =>
		(MarkupString)Markdown.ToHtml(markdown, _pipeline);
}

public interface IMarkdownProvider
{
	public MarkdownPipeline Pipeline { get; }

	public string GetHtml(string markdown);
	public MarkupString GetMarkup(string markdown);
}

public static class MarkdownProviderExtension
{
	public static IServiceCollection AddDuMarkdownProvider(this IServiceCollection services)
	{
		if (services == null)
			throw new ArgumentNullException(nameof(services));

		services.AddScoped<IMarkdownProvider, MarkdownProvider>();

		return services;
	}
}
