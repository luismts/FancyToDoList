using Akavache.Sqlite3.Internal;

namespace FancyTodoList.Models
{
	[Table("todoitem")]
	public class TodoItem
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public string Name { get; set; }
		public bool Done { get; set; }
	}
}
