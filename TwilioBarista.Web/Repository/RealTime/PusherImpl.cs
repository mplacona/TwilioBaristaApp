using System;
using PusherServer;
using TwilioBarista.Core.Interfaces;

namespace TwilioBarista.Web.Repository.RealTime
{
    public class PusherImpl : IRealTime<Pusher>
    {
        public Pusher GetClient(string id, string key, string secret)
        {
            return new Pusher(id, key, secret);
        }

        public object Trigger(string channelName, string eventName, object data)
        {
            throw new NotImplementedException();
        }
    }
}
