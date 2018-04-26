using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Egram.Components.TDLib
{
    public class Agent : IAgent
    {
        private readonly TD.Hub _hub;
        private readonly TD.Dialer _dialer;

        public Agent(
            TD.Hub hub,
            TD.Dialer dialer
            )
        {
            _hub = hub;
            _dialer = dialer;

            _hub.Received += (sender, structure) =>
            {
                Received?.Invoke(sender, structure);
            };
        }

        public event EventHandler<TD.Structure> Received;

        public Task<TResult> ExecuteAsync<TResult>(TD.Method<TResult> method)
            where TResult : TD.Structure
        {
            return _dialer.ExecuteAsync(method);
        }
    }
}
