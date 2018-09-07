using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using ReactiveUI;
using TdLib;
using Tel.Egram.Feeds;
using Tel.Egram.Graphics;

namespace Tel.Egram.Components.Catalog
{
    public class EntryModel : ReactiveObject
    {   
        public virtual Func<IDisposable> LoadAvatar { get; set; }
        
        private IBitmap _avatar;
        public IBitmap Avatar
        {
            get => _avatar;
            set => this.RaiseAndSetIfChanged(ref _avatar, value);
        }

        private IBrush _colorBrush;
        public IBrush ColorBrush
        {
            get => _colorBrush;
            set => this.RaiseAndSetIfChanged(ref _colorBrush, value);
        }

        private bool _isFallbackAvatar;
        public bool IsFallbackAvatar
        {
            get => _isFallbackAvatar;
            set => this.RaiseAndSetIfChanged(ref _isFallbackAvatar, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => this.RaiseAndSetIfChanged(ref _title, value);
        }

        private string _init;
        public string Init
        {
            get => _init;
            set => this.RaiseAndSetIfChanged(ref _init, value);
        }
    }
    
    public class ChatEntryModel : EntryModel
    {
        //private static readonly ColorMapper _colorMapper = new ColorMapper();
        
        public override Func<IDisposable> LoadAvatar { get; set; }
        
        private Chat _chat;
        public Chat Chat
        {
            get => _chat;
            set => this.RaiseAndSetIfChanged(ref _chat, value);
        }

        public static ChatEntryModel FromChat(Chat chat)
        {
            var title = chat.Ch.Title;
            var color = new SolidColorBrush(Colors.Transparent);
            var init = string.IsNullOrEmpty(title) ? null : title.Substring(0, 1).ToUpper();
            
            return new ChatEntryModel
            {
                Chat = chat,
                Title = title,
                ColorBrush = color,
                Init = init,
                LoadAvatar = () => Disposable.Empty
            };
        }
    }
}