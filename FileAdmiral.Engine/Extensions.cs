using System;

namespace FileAdmiral.Engine
{
    public delegate void EventDelegate<T>(object sender, T eventArgs) where T : EventArgs;

    public static class Extensions
    {
        public static bool Raise(this EventHandler handler, object sender, EventArgs e)
        {
            if (handler != null)
            {
                handler(sender, e);
                return true;
            }
            return false;
        }

        public static bool Raise<TEventArgs>(this EventHandler<TEventArgs> handler, object sender, TEventArgs e)
            where TEventArgs : EventArgs
        {
            if (handler != null)
            {
                handler(sender, e);
                return true;
            }
            return false;
        }
    }
}
