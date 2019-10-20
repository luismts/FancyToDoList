using FancyTodoList.Models;
using System.Threading.Tasks;

namespace FancyTodoList.Interfaces
{
	public interface IBingSpellCheckService
	{
		Task<SpellCheckResult> SpellCheckTextAsync(string text);
	}
}
