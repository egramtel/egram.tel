using System;
using System.Reactive.Linq;
using Avalonia.Threading;
using Egram.Components.Chatting;
using Egram.Components.Navigation;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace Egram.Components.Workarea
{
    public class WorkareaContext : ReactiveObject, IDisposable
    {
        private readonly Topic _topic;
        private readonly ToolbarContextFactory _toolbarContextFactory;
        private readonly ChatContextFactory _chatContextFactory;

        public WorkareaContext(
            Topic topic,
            ToolbarContextFactory toolbarContextFactory,
            ChatContextFactory chatContextFactory
            )
        {
            _topic = topic;
            _toolbarContextFactory = toolbarContextFactory;
            _chatContextFactory = chatContextFactory;

            ToolbarContext = _toolbarContextFactory.FromTopic(topic);
            ChatContext = _chatContextFactory.FromTopic(topic);

            IsToolbarVisible = true;
            IsChatVisible = true;
            IsSidepadVisible = false;
            ContentColumnSpan = 3;
        }

        public WorkareaContext()
        {
            IsToolbarVisible = false;
            IsChatVisible = false;
            IsSidepadVisible = false;
            ContentColumnSpan = 3;
        }

        private bool _isToolbarVisible;
        public bool IsToolbarVisible
        {
            get => _isToolbarVisible;
            set => this.RaiseAndSetIfChanged(ref _isToolbarVisible, value);
        }

        private ToolbarContext _toolbarContext;
        public ToolbarContext ToolbarContext
        {
            get => _toolbarContext;
            set => this.RaiseAndSetIfChanged(ref _toolbarContext, value);
        }

        private bool _isChatVisible;
        public bool IsChatVisible
        {
            get => _isChatVisible;
            set => this.RaiseAndSetIfChanged(ref _isChatVisible, value);
        }
        
        private ChatContext _chatContext;
        public ChatContext ChatContext
        {
            get => _chatContext;
            set => this.RaiseAndSetIfChanged(ref _chatContext, value);
        }

        private bool _isSidepadVisible;
        public bool IsSidepadVisible
        {
            get => _isSidepadVisible;
            set => this.RaiseAndSetIfChanged(ref _isSidepadVisible, value);
        }

        private SidepadContext _sidepadContext;
        public SidepadContext SidepadContext
        {
            get => _sidepadContext;
            set => this.RaiseAndSetIfChanged(ref _sidepadContext, value);
        }

        private int _contentColumnSpan = 3;
        public int ContentColumnSpan
        {
            get => _contentColumnSpan;
            set => this.RaiseAndSetIfChanged(ref _contentColumnSpan, value);
        }

        public void Dispose()
        {
            _toolbarContext?.Dispose();
            _chatContext?.Dispose();
            _sidepadContext?.Dispose();
        }
    }

    public class WorkareaContextFactory
    {
        private readonly IServiceProvider _provider;

        public WorkareaContextFactory(IServiceProvider provider)
        {
            _provider = provider;
        }
        
        public WorkareaContext FromTopic(Topic topic)
        {
            return new WorkareaContext(
                topic,
                _provider.GetService<ToolbarContextFactory>(),
                _provider.GetService<ChatContextFactory>());
        }

        public WorkareaContext CreateEmpty()
        {
            return new WorkareaContext();
        }
    }
}