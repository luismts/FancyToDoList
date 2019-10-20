using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace FancyTodoList.Models
{
    public class Item : BindableBase
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

        public string Category { get; set; }

        private bool _completed;
        public bool Completed
        {
            get { return _completed; }
            set { SetProperty(ref _completed, value); }
        }
        public bool Showed { get; set; }
    }
}
