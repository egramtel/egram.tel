using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TD
{
    public class Dialer : IDisposable
    {
        private static int _id = 0;
        private readonly ConcurrentDictionary<int, Action<Structure>> _tasks;

        private readonly ILogger<Dialer> _logger;
        private readonly Client _client;
        private readonly Hub _hub;

        public Dialer(
            ILogger<Dialer> logger,
            Client client,
            Hub hub
            )
        {
            _tasks = new ConcurrentDictionary<int, Action<Structure>>();
            _logger = logger;
            _client = client;
            _hub = hub;

            _hub.Received += OnReceived;
        }

        private void OnReceived(object _, Structure structure)
        {
            if (int.TryParse(structure.Extra, out int id) && _tasks.TryRemove(id, out var action))
            {
                action(structure);
            }
        }
        
        public void Send<TResut>(Method<TResut> method)
        {
            var data = JsonConvert.SerializeObject(method);
            _logger.LogDebug(data);
            _client.Send(data);
        }

        public Structure Execute<TResult>(Method<TResult> method)
        {
            var data = JsonConvert.SerializeObject(method);
            _logger.LogDebug(data);
            data = _client.Execute(data);
            _logger.LogDebug(data);
            var structure = JsonConvert.DeserializeObject<Structure>(data, new Converter());
            return structure;
        }
        
        public Task<TResult> ExecuteAsync<TResult>(Method<TResult> method)
            where TResult : Structure
        {
            var id = Interlocked.Increment(ref _id);
            var tcs = new TaskCompletionSource<TResult>();

            method.Extra = id.ToString();
            _tasks.TryAdd(id, structure =>
            {
                if (structure is Error err)
                {
                    tcs.SetException(new ErrorException(err));
                }
                else if (structure is TResult result)
                {
                    tcs.SetResult(result);
                }
            });
            
            Send(method);
            
            return tcs.Task;
        }

        public void Dispose()
        {
            _hub.Received -= OnReceived;
        }
    }
}