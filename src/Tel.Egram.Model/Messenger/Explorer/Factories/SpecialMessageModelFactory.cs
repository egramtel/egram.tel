using TdLib;
using Tel.Egram.Model.Messenger.Explorer.Messages;
using Tel.Egram.Model.Messenger.Explorer.Messages.Special;
using Tel.Egram.Services.Messaging.Messages;
using Tel.Egram.Services.Utils.Formatting;

namespace Tel.Egram.Model.Messenger.Explorer.Factories
{
    public class SpecialMessageModelFactory : ISpecialMessageModelFactory
    {
        private readonly IStringFormatter _stringFormatter;

        public SpecialMessageModelFactory(
            IStringFormatter stringFormatter)
        {
            _stringFormatter = stringFormatter;
        }
        
        public DocumentMessageModel CreateDocumentMessage(
            Message message,
            TdApi.MessageContent.MessageDocument messageDocument)
        {
            var document = messageDocument.Document;
            var name = messageDocument.Document.FileName;
            var text = messageDocument.Caption.Text;
            var size = $"({_stringFormatter.FormatMemorySize(messageDocument.Document.Document_.Size)})";
            
            return new DocumentMessageModel
            {
                Document = document,
                Name = name,
                Text = text,
                Size = size
            };
        }
        
        public MessageModel CreateGameScoreMessage(
            Message message,
            TdApi.MessageContent.MessageGameScore gameScore)
        {
            return new UnsupportedMessageModel();
        }
        
        public MessageModel CreatePaymentSuccessfulMessage(
            Message message,
            TdApi.MessageContent.MessagePaymentSuccessful paymentSuccessful)
        {
            return new UnsupportedMessageModel();
        }

        public MessageModel CreatePaymentSuccessfulBotMessage(
            Message message,
            TdApi.MessageContent.MessagePaymentSuccessfulBot paymentSuccessfulBot)
        {
            return new UnsupportedMessageModel();
        }

        public MessageModel CreateContactRegisteredMessage(
            Message message,
            TdApi.MessageContent.MessageContactRegistered contactRegistered)
        {
            return new UnsupportedMessageModel();
        }

        public MessageModel CreateWebsiteConnectedMessage(
            Message message,
            TdApi.MessageContent.MessageWebsiteConnected websiteConnected)
        {
            return new UnsupportedMessageModel();
        }

        public MessageModel CreateLocationMessage(
            Message message,
            TdApi.MessageContent.MessageLocation location)
        {
            return new UnsupportedMessageModel();
        }

        public MessageModel CreateVenueMessage(
            Message message,
            TdApi.MessageContent.MessageVenue venue)
        {
            return new UnsupportedMessageModel();
        }

        public MessageModel CreateContactMessage(
            Message message,
            TdApi.MessageContent.MessageContact contact)
        {
            return new UnsupportedMessageModel();
        }

        public MessageModel CreateGameMessage(
            Message message,
            TdApi.MessageContent.MessageGame game)
        {
            return new UnsupportedMessageModel();
        }

        public MessageModel CreateInvoiceMessage(
            Message message,
            TdApi.MessageContent.MessageInvoice invoice)
        {
            return new UnsupportedMessageModel();
        }

        public MessageModel CreatePassportDataSentMessage(
            Message message,
            TdApi.MessageContent.MessagePassportDataSent passportDataSent)
        {
            return new UnsupportedMessageModel();
        }

        public MessageModel CreatePassportDataReceivedMessage(
            Message message,
            TdApi.MessageContent.MessagePassportDataReceived passportDataReceived)
        {
            return new UnsupportedMessageModel();
        }

        public MessageModel CreateAudioMessage(
            Message message,
            TdApi.MessageContent.MessageAudio messageAudio)
        {
            return new UnsupportedMessageModel();
        }

        public MessageModel CreateVoiceNoteMessage(
            Message message,
            TdApi.MessageContent.MessageVoiceNote voiceNote)
        {
            return new UnsupportedMessageModel();
        }
    }
}