using System.Windows;

namespace Oniqys.Wpf
{
    public abstract class BerhaviorBase<T> : IBehavior<T>
        where T : FrameworkElement
    {
        protected T Owner { get; private set; }

        public void Attach(T owner) => Owner = owner;

        public void Detach() => Owner = null;
    }
}
