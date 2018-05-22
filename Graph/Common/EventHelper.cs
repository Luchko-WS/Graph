using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph.Common
{
    static class EventHelper
    {
        public static void Invoke<T>(EventHandler<T> handler, object sender)
        {
            Invoke(handler, sender, default(T));
        }

        public static void Invoke<T>(EventHandler<T> handler, object sender, T args)
        {
            var h = handler;
            if(h != null)
            {
                h.Invoke(sender, args);
            }
        }
    }
}
