using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FancyTodoList.Helpers;
using Xamarin.Forms;

namespace FancyTodoList.Controls
{
    public class CustomRepeaterView : RepeaterView<Models.Item>
    {

    }

    /// <summary>
    /// Low cost control to display a set of clickable items
    /// </summary>
    /// <typeparam name="T">The Type of viewmodel</typeparam>
    public class RepeaterView<T> : StackLayout where T : class
    {
        /// <summary>
        /// Definition for <see cref="ItemTemplate"/>
        /// </summary>
        /// Element created at 15/11/2014,3:11 PM by Charles
        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create("ItemTemplate", typeof(DataTemplate), typeof(RepeaterView<T>), default(DataTemplate));
        //public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create<RepeaterView<T>, DataTemplate>(p => p.ItemTemplate, default(DataTemplate));

        /// <summary>
        /// Definition for <see cref="ItemsSource"/>
        /// </summary>
        /// Element created at 15/11/2014,3:11 PM by Charles
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create<RepeaterView<T>, IEnumerable<T>>(p => p.ItemsSource, Enumerable.Empty<T>(), BindingMode.OneWay, null, ItemsChanged);


        /// <summary>
        /// Definition for <see cref="ItemTappedCommand"/>
        /// </summary>
        public static readonly BindableProperty ItemTappedCommandProperty =
           BindableProperty.Create("ItemTappedCommand", typeof(ICommand), typeof(RepeaterView<T>));
        //public static BindableProperty ItemClickCommandProperty =
        //    BindableProperty.Create<RepeaterView<T>, ICommand>(x => x.ItemClickCommand, null);

        /// <summary>
        /// Definition for <see cref="TemplateSelector"/>
        /// </summary>
        public static readonly BindableProperty TemplateSelectorProperty =
          BindableProperty.Create("TemplateSelector", typeof(TemplateSelector), typeof(RepeaterView<T>), default(TemplateSelector));
        //public static readonly BindableProperty TemplateSelectorProperty =
        //    BindableProperty.Create<RepeaterView<T>, TemplateSelector>(x => x.TemplateSelector, default(TemplateSelector));

        /// <summary>
        /// The item template selector property
        /// </summary>
        public static readonly BindableProperty ItemTemplateSelectorProperty =
            BindableProperty.Create<RepeaterView<T>, DataTemplateSelector>(x => x.ItemTemplateSelector, default(DataTemplateSelector), propertyChanged: OnDataTemplateSelectorChanged);

        private DataTemplateSelector currentItemSelector;
        /// <summary>
        /// Gets or sets the item template selector.
        /// </summary>
        /// <value>The item template selector.</value>
        public DataTemplateSelector ItemTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(ItemTemplateSelectorProperty);
            set => SetValue(ItemTemplateSelectorProperty, value);
        }

        private static void OnDataTemplateSelectorChanged(BindableObject bindable, DataTemplateSelector oldvalue, DataTemplateSelector newvalue)
        {
            ((RepeaterView<T>)bindable).OnDataTemplateSelectorChanged(oldvalue, newvalue);
        }

        /// <summary>
        /// Called when [data template selector changed].
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <exception cref="System.ArgumentException">Cannot set both ItemTemplate and ItemTemplateSelector;ItemTemplateSelector</exception>
        protected virtual void OnDataTemplateSelectorChanged(DataTemplateSelector oldValue, DataTemplateSelector newValue)
        {
            // check to see we don't have an ItemTemplate set
            if (ItemTemplate != null && newValue != null)
                throw new ArgumentException("Cannot set both ItemTemplate and ItemTemplateSelector", "ItemTemplateSelector");

            currentItemSelector = newValue;
        }

        /// <summary>
        /// Event delegate definition fo the <see cref="ItemCreated"/> event
        /// </summary>
        /// <param name="sender">The sender(this).</param>
        /// <param name="args">The <see cref="RepeaterViewItemAddedEventArgs"/> instance containing the event data.</param>
        /// Element created at 15/11/2014,3:12 PM by Charles
        public delegate void RepeaterViewItemAddedEventHandler(object sender, RepeaterViewItemAddedEventArgs args);

        /// <summary>Occurs when a view has been created.</summary>
        /// Element created at 15/11/2014,3:13 PM by Charles
        public event RepeaterViewItemAddedEventHandler ItemCreated;

        /// <summary>
        /// The Collection changed handler
        /// </summary>
        /// Element created at 15/11/2014,3:13 PM by Charles
        private IDisposable _collectionChangedHandle;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeaterView{T}"/> class.
        /// </summary>
        /// Element created at 15/11/2014,3:13 PM by Charles
        public RepeaterView()
        {
            Spacing = 0;
        }

        /// <summary>Gets or sets the items source.</summary>
        /// <value>The items source.</value>
        /// Element created at 15/11/2014,3:13 PM by Charles
        public IEnumerable<T> ItemsSource
        {
            get => (IEnumerable<T>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        /// <summary>Gets or sets the template selector.</summary>
        /// <value>The template selector.</value>
        /// Element created at 15/11/2014,3:13 PM by Charles
        public TemplateSelector TemplateSelector
        {
            get => (TemplateSelector)GetValue(TemplateSelectorProperty);
            set => SetValue(TemplateSelectorProperty, value);
        }

        /// <summary>Gets or sets the item click command.</summary>
        /// <value>The item click command.</value>
        /// Element created at 15/11/2014,3:13 PM by Charles
        public ICommand ItemTappedCommand
        {
            get => (ICommand)GetValue(ItemTappedCommandProperty);
            set => SetValue(ItemTappedCommandProperty, value);
        }

        /// <summary>
        /// The item template property
        /// This can be used on it's own or in combination with 
        /// the <see cref="TemplateSelector"/>
        /// </summary>
        /// Element created at 15/11/2014,3:10 PM by Charles
        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        /// <summary>
        /// Gives codebehind a chance to play with the
        /// newly created view object :D
        /// </summary>
        /// <param name="view">The visual view object</param>
        /// <param name="model">The item being added</param>
        protected virtual void NotifyItemAdded(View view, T model)
        {
            if (ItemCreated != null)
            {
                ItemCreated(this, new RepeaterViewItemAddedEventArgs(view, model));
            }
        }

        /// <summary>
        /// Select a datatemplate dynamically
        /// Prefer the TemplateSelector then the DataTemplate
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected virtual DataTemplate GetTemplateFor(Type type)
        {
            DataTemplate retTemplate = null;

            if (TemplateSelector != null)
                retTemplate = TemplateSelector.TemplateFor(type);

            return retTemplate ?? ItemTemplate;
        }

        /// <summary>
        /// Creates a view based on the items type
        /// While we do have T, T could very well be
        /// a common superclass or an interface by
        /// using the items actual type we support
        /// both inheritance based polymorphism
        /// and shape based polymorphism
        ///
        /// </summary>
        /// <param name="item"></param>
        /// <returns>A <see cref="View"/> item as it's BindingContext</returns>
        /// <exception cref="InvalidVisualObjectException"></exception>Thrown when the matched datatemplate inflates to an object not derived from either
        /// <see cref="Xamarin.Forms.View"/> or <see cref="Xamarin.Forms.ViewCell"/>
        protected virtual View ViewFor(T item)
        {
            // Check the item template selector first
            View view = null;
            //if (currentItemSelector != null)
            //{
            //    view = this.ViewFor(item, currentItemSelector);
            //}

            //if (view == null)
            //{
            var template = GetTemplateFor(item.GetType());
            var content = template.CreateContent();

            if (!(content is View) && !(content is ViewCell))
                throw new InvalidVisualObjectException(content.GetType());

            view = (content is View) ? content as View : ((ViewCell)content).View;
            //}

            view.BindingContext = item;

            if (ItemTappedCommand != null)
                view.GestureRecognizers.Add(new TapGestureRecognizer { Command = ItemTappedCommand, CommandParameter = item });

            return view;
        }

        /// <summary>
        /// Reset the collection of bound objects
        /// Remove the old collection changed eventhandler (if any)
        /// Create new cells for each new item
        /// </summary>
        /// <param name="bindable">The control</param>
        /// <param name="oldValue">Previous bound collection</param>
        /// <param name="newValue">New bound collection</param>
        private static void ItemsChanged(BindableObject bindable, IEnumerable<T> oldValue, IEnumerable<T> newValue)
        {
            var control = bindable as RepeaterView<T>;

            if (control == null)
                throw new Exception(
                    "Invalid bindable object passed to ReapterView::ItemsChanged expected a ReapterView<T> received a "
                    + bindable.GetType().Name);

            if (control._collectionChangedHandle != null)
            {
                control._collectionChangedHandle.Dispose();
            }

            control._collectionChangedHandle = new CollectionChangedHandle<View, T>(
                control.Children,
                newValue,
                control.ViewFor,
                (v, m, i) => control.NotifyItemAdded(v, m));
        }
    }

    /// <summary>
    /// Argument for the <see cref="RepeaterView{T}.ItemCreated"/> event
    /// </summary>
    /// Element created at 15/11/2014,3:13 PM by Charles
    public class RepeaterViewItemAddedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepeaterViewItemAddedEventArgs"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="model">The model.</param>
        /// Element created at 15/11/2014,3:14 PM by Charles
        public RepeaterViewItemAddedEventArgs(View view, object model)
        {
            View = view;
            Model = model;
        }

        /// <summary>Gets or sets the view.</summary>
        /// <value>The visual element.</value>
        /// Element created at 15/11/2014,3:14 PM by Charles
        public View View { get; set; }

        /// <summary>Gets or sets the model.</summary>
        /// <value>The original viewmodel.</value>
        /// Element created at 15/11/2014,3:14 PM by Charles
        public object Model { get; set; }
    }

    /// <summary>
    /// Small utility class that takes
    /// gyuwon's idea to it's logical 
    /// conclusion.
    /// The code in the ItemsCollectionChanged methods
    /// rarely changes.  The only real change is projecting 
    /// from source type T to targeted type TSyncType which
    /// is then inserted into the target collection
    /// </summary>
    public class CollectionChangedHandle<TSyncType, T> : IDisposable where T : class where TSyncType : class
    {
        private readonly Func<T, TSyncType> _projector;
        private readonly Action<TSyncType, T, int> _postadd;
        private readonly Action<TSyncType> _cleanup;
        private readonly INotifyCollectionChanged _itemsSourceCollectionChangedImplementation;
        private readonly IEnumerable<T> _sourceCollection;
        private readonly IList<TSyncType> _target;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionChangedHandle{TSyncType,T}"/> class.
        /// </summary>
        /// <param name="target">The collection to be kept in sync with source</param>
        /// <param name="source">The original collection</param>
        /// <param name="projector">A function that returns {TSyncType} for a {T}</param>
        /// <param name="postadd">A functino called right after insertion into the synced collection</param>
        /// <param name="cleanup">A function that performs any needed cleanup when {TSyncType} is removed from the target</param>
        public CollectionChangedHandle(IList<TSyncType> target, IEnumerable<T> source, Func<T, TSyncType> projector, Action<TSyncType, T, int> postadd = null, Action<TSyncType> cleanup = null)
        {
            if (source == null)
                return;

            _itemsSourceCollectionChangedImplementation = source as INotifyCollectionChanged;

            _sourceCollection = source;
            _target = target;
            _projector = projector;
            _postadd = postadd;
            _cleanup = cleanup;

            InitialPopulation();

            if (_itemsSourceCollectionChangedImplementation == null)
                return;

            _itemsSourceCollectionChangedImplementation.CollectionChanged += CollectionChanged;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_itemsSourceCollectionChangedImplementation == null) return;
            _itemsSourceCollectionChangedImplementation.CollectionChanged -= CollectionChanged;
        }

        /// <summary>Keeps <see cref="_target"/> in sync with <see cref="_sourceCollection"/>.</summary>
        /// <param name="sender">The sender, completely ignored.</param>
        /// <param name="args">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        /// Element created at 15/11/2014,2:57 PM by Charles
        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.Action == NotifyCollectionChangedAction.Reset)
            {
                SafeClearTarget();
            }
            else
            {
                //Create a temp list to prevent multiple enumeration issues
                var tlist = new List<T>(_sourceCollection);

                if (args.OldItems != null)
                {
                    var syncitem = _target[args.OldStartingIndex];
                    if (syncitem != null && _cleanup != null) _cleanup(syncitem);
                    _target.RemoveAt(args.OldStartingIndex);
                }

                if (args.NewItems == null)
                    return;

                foreach (var obj in args.NewItems)
                {
                    var item = obj as T;
                    if (item == null) continue;
                    var index = tlist.IndexOf(item);
                    var newsyncitem = _projector(item);
                    _target.Insert(index, newsyncitem);

                    if (_postadd != null)
                        _postadd(newsyncitem, item, index);
                }
            }

        }

        /// <summary>Initials the population.</summary>
        /// Element created at 15/11/2014,2:53 PM by Charles
        private void InitialPopulation()
        {
            SafeClearTarget();
            foreach (var t in _sourceCollection.Where(x => x != null))
            {
                _target.Add(_projector(t));
            }
        }

        private void SafeClearTarget()
        {
            while (_target.Count > 0)
            {
                var syncitem = _target[0];
                _target.RemoveAt(0);

                if (_cleanup != null)
                    _cleanup(syncitem);
            }
        }
    }
}
