﻿using System.Windows;

namespace Oniqys.Wpf
{
    public static class FrameworkElementExtensions
    {
        public static IBehavior<FrameworkElement>[] GetBehaviors(DependencyObject obj)
        {
            return (IBehavior<FrameworkElement>[])obj.GetValue(BehaviorsProperty);
        }

        public static void SetBehaviors(DependencyObject obj, IBehavior<FrameworkElement>[] value)
        {
            obj.SetValue(BehaviorsProperty, value);
        }

        // Using a DependencyProperty as the backing store for Behaviors.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BehaviorsProperty =
            DependencyProperty.RegisterAttached("Behaviors", typeof(IBehavior<FrameworkElement>[]), typeof(FrameworkElement), new PropertyMetadata(null,
                (s, e) =>
                {
                    if (!(s is FrameworkElement owner))
                        return;

                    if (e.NewValue is IBehavior<FrameworkElement>[] oldBehaviors)
                    {
                        foreach (var behavior in oldBehaviors)
                            behavior.Detach();
                    }

                    if (!(e.NewValue is IBehavior<FrameworkElement>[] behaviors))
                        return;

                    foreach (var behavior in behaviors)
                        behavior.Attach(owner);
                }));

    }
}
