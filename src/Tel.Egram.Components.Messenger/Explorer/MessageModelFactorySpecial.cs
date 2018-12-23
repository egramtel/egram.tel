using TdLib;
using Tel.Egram.Components.Messenger.Explorer.Messages;
using Tel.Egram.Messaging.Messages;

namespace Tel.Egram.Components.Messenger.Explorer
{
    public partial class MessageModelFactory : IMessageModelFactory
    {
        private MessageModel CreateGameScoreMessage(Message message, TdApi.MessageContent.MessageGameScore gameScore)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }
        
        private MessageModel CreatePaymentSuccessfulMessage(Message message, TdApi.MessageContent.MessagePaymentSuccessful paymentSuccessful)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        private MessageModel CreatePaymentSuccessfulBotMessage(Message message, TdApi.MessageContent.MessagePaymentSuccessfulBot paymentSuccessfulBot)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        private MessageModel CreateContactRegisteredMessage(Message message, TdApi.MessageContent.MessageContactRegistered contactRegistered)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        private MessageModel CreateWebsiteConnectedMessage(Message message, TdApi.MessageContent.MessageWebsiteConnected websiteConnected)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        private MessageModel CreateLocationMessage(Message message, TdApi.MessageContent.MessageLocation location)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        private MessageModel CreateVenueMessage(Message message, TdApi.MessageContent.MessageVenue venue)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        private MessageModel CreateContactMessage(Message message, TdApi.MessageContent.MessageContact contact)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        private MessageModel CreateGameMessage(Message message, TdApi.MessageContent.MessageGame game)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        private MessageModel CreateInvoiceMessage(Message message, TdApi.MessageContent.MessageInvoice invoice)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        private MessageModel CreatePassportDataSentMessage(Message message, TdApi.MessageContent.MessagePassportDataSent passportDataSent)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        private MessageModel CreatePassportDataReceivedMessage(Message message, TdApi.MessageContent.MessagePassportDataReceived passportDataReceived)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        private MessageModel CreateDocumentMessage(Message message, TdApi.MessageContent.MessageDocument messageDocument)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        private MessageModel CreateAudioMessage(Message message, TdApi.MessageContent.MessageAudio messageAudio)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        private MessageModel CreateVoiceNoteMessage(Message message, TdApi.MessageContent.MessageVoiceNote voiceNote)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }
    }
}