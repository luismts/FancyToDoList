using System.Threading.Tasks;

namespace FancyTodoList.Interfaces
{
	public interface ITextTranslationService
	{
		Task<string> TranslateTextAsync(string text);
	}
}
