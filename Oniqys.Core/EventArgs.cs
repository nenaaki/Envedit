using System;

namespace Oniqys.Core
{
    public class EventArgs<T> : EventArgs
    {
        T Args { get; }

        public EventArgs(T args)
        {
            Args = args;
        }

    }
}
