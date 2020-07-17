using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Oniqys.Wpf
{
    public interface IBehavior<T>
        where T : FrameworkElement
    {
        public void Attach(T owner);

        public void Detach(T owner);
    }
}
