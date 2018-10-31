namespace Tel.Egram.Models.Messenger.Explorer.Messages
{
    public class TextMessageModel : MessageModel
    {
        public string Text { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}