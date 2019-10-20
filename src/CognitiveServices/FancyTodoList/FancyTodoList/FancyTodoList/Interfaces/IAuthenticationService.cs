using System.Threading.Tasks;

namespace FancyTodoList.Interfaces
{
	public interface IAuthenticationService
	{
		Task InitializeAsync(string authenticationTokenEndpoint);
		string GetAccessToken();
	}
}
