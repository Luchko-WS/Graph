using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph.Common
{
    static class EventHelper
    {
        public static void Invoke(object sender, EventHandler handler)
        {
            Invoke(sender, handler, null);
        }

        public static void Invoke(object sender, EventHandler handler, EventArgs args)
        {
            var h = handler;
            if(h != null)
            {
                h.Invoke(sender, args);
            }
        }
    }
}
