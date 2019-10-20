using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyTodoList.Helpers
{
    /// <summary>
    /// Clase generica para eventos
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EventArgs<T> : EventArgs
    {
        public T Value { get; private set; }

        public EventArgs(T val)
        {
            Value = val;
        }
    }
}
