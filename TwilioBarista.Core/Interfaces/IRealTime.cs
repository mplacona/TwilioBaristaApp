using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwilioBarista.Core.Interfaces
{
    public interface IRealTime<out T>
    {
        T GetClient(string id, string key, string secret);
        object Trigger(string channelName, string eventName, object data);
    }
}
