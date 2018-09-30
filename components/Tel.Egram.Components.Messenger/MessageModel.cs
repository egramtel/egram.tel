using Avalonia.Media;
using Avalonia.Media.Imaging;
using PropertyChanged;
using TdLib;
using Tel.Egram.Graphics;
using Tel.Egram.Messages;

namespace Tel.Egram.Components.Messenger
{
    [AddINotifyPropertyChangedInterface]
    public class MessageModel
    {
        private static readonly ColorMapper _colorMapper = new ColorMapper();
        
        public Message Message { get; set; }
        public string Text { get; set; }
        public string AuthorName { get; set; }
        public IBrush AuthorColor { get; set; }
        public string AuthorInit { get; set; }
        public IBitmap AuthorAvatar { get; set; }
        public bool IsFallbackAuthorAvatar { get; set; }
       
        public static MessageModel FromMessage(Message message)
        {
            var content = message.Msg.Content;
            var text = "";
            var authorName = message.Chat.Title ?? " ";
            
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
                Text = text,
                AuthorName = authorName,
                AuthorColor = color,
                AuthorInit = init
            };
        }
    }
}