using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sklepik.ViewModel      //  http://www.codearsenal.net/2013/07/pub-sub-observer-pattern-in-mvvm.html
{
    public delegate void PubSubEventHandler<T>(object sender, PubSubEventArgs<T> args);

    public class PubSubEventArgs<T> : EventArgs
    {
        public T Item { get; set; }

        public PubSubEventArgs(T item)
        {
            Item = item;
        }
    }

    public static class PubSub<T>
    {
        private static Dictionary<string, PubSubEventHandler<T>> events 
            = new Dictionary<string, PubSubEventHandler<T>>();

        public static void RegisterEvent(string name, PubSubEventHandler<T> handler)
        {
            PubSubEventHandler<T> h;
            if (events.TryGetValue(name, out h))
                h += handler;
            else
                events.Add(name, handler);
        }

        public static void RaiseEvent(string name, object sender, PubSubEventArgs<T> args)
        {
            if (events.ContainsKey(name) && events[name] != null)
                events[name](sender, args);
        }
    }
}
