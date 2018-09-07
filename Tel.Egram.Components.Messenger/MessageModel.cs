using System.Linq;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using ReactiveUI;
using TdLib;
using Tel.Egram.Feeds;
using Tel.Egram.Graphics;
using Tel.Egram.Messages;

namespace Tel.Egram.Components.Messenger
{
    public class MessageModel : ReactiveObject
    {
        private static readonly ColorMapper _colorMapper = new ColorMapper();
        
        public Message Message { get; set; }
        
        private string _text;
        public string Text
        {
            get => _text;
            set => this.RaiseAndSetIfChanged(ref _text, value);
        }

        private string _authorName;
        public string AuthorName
        {
            get => _authorName;
            set => this.RaiseAndSetIfChanged(ref _authorName, value);
        }

        private IBrush _authorColor;
        public IBrush AuthorColor
        {
            get => _authorColor;
            set => this.RaiseAndSetIfChanged(ref _authorColor, value);
        }

        private string _authorInit;
        public string AuthorInit
        {
            get => _authorInit;
            set => this.RaiseAndSetIfChanged(ref _authorInit, value);
        }

        private IBitmap _authorAvatar;
        public IBitmap AuthorAvatar
        {
            get => _authorAvatar;
            set => this.RaiseAndSetIfChanged(ref _authorAvatar, value);
        }

        private bool _isFallbackAuthorAvatar;
        public bool IsFallbackAuthorAvatar
        {
            get => _isFallbackAuthorAvatar;
            set => this.RaiseAndSetIfChanged(ref _isFallbackAuthorAvatar, value);
        }
        
        public static MessageModel FromMessage(Message message)
        {
            var content = message.Msg.Content;
            string text = "";
            string authorName = message.Chat.Title ?? " ";
            
            switch (content)
            {
                case TdApi.MessageContent.MessageText messageText:
                    text = messageText.Text.Text;
                    break;
                
                case TdApi.MessageContent.MessagePhoto messagePhoto:
                    text = messagePhoto.Caption.Text;
                    break;
            }

            var color = new SolidColorBrush(Color.Parse("#" + _colorMapper[message.Chat.Id]));
            var init = authorName.Substring(0, 1).ToUpper();
            
            return new MessageModel
            {
                Message = message,
                _text = text,
                _authorName = authorName,
                _authorColor = color,
                _authorInit = init
            };
        }
    }
}