using FancyTodoList.Models;
using System.Threading.Tasks;

namespace FancyTodoList.Interfaces
{
	public interface IBingSpeechService
	{
		Task<SpeechResult> RecognizeSpeechAsync(string filename, string apiKey);
	}
}
