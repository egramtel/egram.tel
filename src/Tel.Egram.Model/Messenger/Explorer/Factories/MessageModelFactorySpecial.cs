using TdLib;
using Tel.Egram.Model.Messenger.Explorer.Messages;
using Tel.Egram.Model.Messenger.Explorer.Messages.Special;
using Tel.Egram.Services.Messaging.Messages;

namespace Tel.Egram.Model.Messenger.Explorer.Factories
{
    public partial class MessageModelFactory
    {
        private DocumentMessageModel CreateDocumentMessage(
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
        
        private MessageModel CreateGameScoreMessage(
            Message message,
            TdApi.MessageContent.MessageGameScore gameScore)
        {
            return new UnsupportedMessageModel();
        }
        
        private MessageModel CreatePaymentSuccessfulMessage(
            Message message,
            TdApi.MessageContent.MessagePaymentSuccessful paymentSuccessful)
        {
            return new UnsupportedMessageModel();
        }

        private MessageModel CreatePaymentSuccessfulBotMessage(
            Message message,
            TdApi.MessageContent.MessagePaymentSuccessfulBot paymentSuccessfulBot)
        {
            return new UnsupportedMessageModel();
        }

        private MessageModel CreateContactRegisteredMessage(
            Message message,
            TdApi.MessageContent.MessageContactRegistered contactRegistered)
        {
            return new UnsupportedMessageModel();
        }

        private MessageModel CreateWebsiteConnectedMessage(
            Message message,
            TdApi.MessageContent.MessageWebsiteConnected websiteConnected)
        {
            return new UnsupportedMessageModel();
        }

        private MessageModel CreateLocationMessage(
            Message message,
            TdApi.MessageContent.MessageLocation location)
        {
            return new UnsupportedMessageModel();
        }

        private MessageModel CreateVenueMessage(
            Message message,
            TdApi.MessageContent.MessageVenue venue)
        {
            return new UnsupportedMessageModel();
        }

        private MessageModel CreateContactMessage(
            Message message,
            TdApi.MessageContent.MessageContact contact)
        {
            return new UnsupportedMessageModel();
        }

        private MessageModel CreateGameMessage(
            Message message,
            TdApi.MessageContent.MessageGame game)
        {
            return new UnsupportedMessageModel();
        }

        private MessageModel CreateInvoiceMessage(
            Message message,
            TdApi.MessageContent.MessageInvoice invoice)
        {
            return new UnsupportedMessageModel();
        }

        private MessageModel CreatePassportDataSentMessage(
            Message message,
            TdApi.MessageContent.MessagePassportDataSent passportDataSent)
        {
            return new UnsupportedMessageModel();
        }

        private MessageModel CreatePassportDataReceivedMessage(
            Message message,
            TdApi.MessageContent.MessagePassportDataReceived passportDataReceived)
        {
            return new UnsupportedMessageModel();
        }

        private MessageModel CreateAudioMessage(
            Message message,
            TdApi.MessageContent.MessageAudio messageAudio)
        {
            return new UnsupportedMessageModel();
        }

        private MessageModel CreateVoiceNoteMessage(
            Message message,
            TdApi.MessageContent.MessageVoiceNote voiceNote)
        {
            return new UnsupportedMessageModel();
        }
    }
}