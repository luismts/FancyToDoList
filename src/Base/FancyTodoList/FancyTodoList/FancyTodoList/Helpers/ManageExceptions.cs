using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FancyTodoList.Helpers
{
    /// <summary>
	/// exception thrown when a template cannot
	/// be found for a supplied type
	/// </summary>
	public class NoDataTemplateMatchException : Exception
    {
        /// <summary>
        /// Hide any possible default constructor
        /// Redundant I know, but it costs nothing
        /// and communicates the design intent to
        /// other developers.
        /// </summary>
        private NoDataTemplateMatchException() { }

        /// <summary>
        /// Constructs the exception and passses a meaningful
        /// message to the base Exception
        /// </summary>
        /// <param name="tomatch">The type that a match was attempted for</param>
        /// <param name="candidates">All types examined during the match process</param>
        public NoDataTemplateMatchException(Type tomatch, List<Type> candidates) :
            base(string.Format("Could not find a template for type [{0}]", tomatch.Name))
        {
            AttemptedMatch = tomatch;
            TypesExamined = candidates;
            TypeNamesExamined = TypesExamined.Select(x => x.Name).ToList();
        }

        /// <summary>
        /// The type that a match was attempted for
        /// </summary>
        public Type AttemptedMatch { get; set; }
        /// <summary>
        /// A list of all types that were examined
        /// </summary>
        public List<Type> TypesExamined { get; set; }
        /// <summary>
        /// A List of the names of all examined types (Simple name only)
        /// </summary>
        public List<string> TypeNamesExamined { get; set; }
    }

    /// <summary>
	/// Thrown when an invalid bindable object has been passed to a callback
	/// </summary>
	public class InvalidBindableException : Exception
    {

        /// <summary>
        /// Hide any possible default constructor
        /// Redundant I know, but it costs nothing
        /// and communicates the design intent to
        /// other developers.
        /// </summary>
        private InvalidBindableException() { }

        /// <summary>
        /// Constructs the exception and passes a meaningful
        /// message to the base Exception
        /// </summary>
        /// <param name="bindable">The bindable object that was passed</param>
        /// <param name="expected">The expected type</param>
        /// <param name="name">The calling methods name, uses [CallerMemberName]</param>
        public InvalidBindableException(BindableObject bindable, Type expected, [CallerMemberName]string name = null)
            : base(string.Format("Invalid bindable passed to {0} expected a {1} received a {2}", name, expected.Name, bindable.GetType().Name))
        {
        }

        /// <summary>
        /// The bindable object that was passed
        /// </summary>
        public BindableObject IncorrectBindableObject { get; set; }
        /// <summary>
        /// The expected type of the bindable object
        /// </summary>
        public Type ExpectedType { get; set; }
    }

    /// <summary>
    /// Thrown when datatemplate inflates to an object 
    /// that is neither a <see cref="Xamarin.Forms.View"/> object nor a
    /// <see cref="Xamarin.Forms.ViewCell"/> object
    /// </summary>
    public class InvalidVisualObjectException : Exception
    {
        /// <summary>
        /// Hide any possible default constructor
        /// Redundant I know, but it costs nothing
        /// and communicates the design intent to
        /// other developers.
        /// </summary>
        private InvalidVisualObjectException() { }

        /// <summary>
        /// Constructs the exception and passes a meaningful
        /// message to the base Exception
        /// </summary>
        /// <param name="inflatedtype">The actual type the datatemplate inflated to.</param>
        /// <param name="name">The calling methods name, uses [CallerMemberName]</param>
        public InvalidVisualObjectException(Type inflatedtype, [CallerMemberName] string name = null) :
        base(string.Format("Invalid template inflated in {0}. Datatemplates must inflate to Xamarin.Forms.View(and subclasses) "
            + "or a Xamarin.Forms.ViewCell(or subclasses).\nActual Type received: [{1}]", name, inflatedtype.Name))
        { }
        /// <summary>
        /// The actual type the datatemplate inflated to.
        /// </summary>
        public Type InflatedType { get; set; }
        /// <summary>
        /// The MemberName the exception occured in.
        /// </summary>
        public string MemberName { get; set; }
    }
}
