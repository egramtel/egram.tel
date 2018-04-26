using System;
using Egram.Components.Navigation;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace Egram.Components.Workarea
{
    public class ToolbarContext : ReactiveObject, IDisposable
    {
        private readonly Topic _topic;

        public ToolbarContext(Topic topic)
        {
            _topic = topic;

            ChatName = topic.Chat.Title;
            ChatInfo = "42 members";
        }

        private string _chatName;
        public string ChatName
        {
            get => _chatName;
            set => this.RaiseAndSetIfChanged(ref _chatName, value);
        }

        private string _chatInfo;
        public string ChatInfo
        {
            get => _chatInfo;
            set => this.RaiseAndSetIfChanged(ref _chatInfo, value);
        }

        public void Dispose()
        {
            //
        }
    }

    public class ToolbarContextFactory
    {
        private readonly IServiceProvider _provider;

        public ToolbarContextFactory(IServiceProvider provider)
        {
            _provider = provider;
        }
        
        public ToolbarContext FromTopic(Topic topic)
        {
            return new ToolbarContext(topic);
        }
    }
}