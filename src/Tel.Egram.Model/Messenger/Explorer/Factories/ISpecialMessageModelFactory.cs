using TdLib;
using Tel.Egram.Model.Messenger.Explorer.Messages;
using Tel.Egram.Model.Messenger.Explorer.Messages.Special;
using Tel.Egram.Services.Messaging.Messages;

namespace Tel.Egram.Model.Messenger.Explorer.Factories
{
    public interface ISpecialMessageModelFactory
    {
        DocumentMessageModel CreateDocumentMessage(
            Message message,
            TdApi.MessageContent.MessageDocument messageDocument);

        MessageModel CreateGameScoreMessage(
            Message message,
            TdApi.MessageContent.MessageGameScore gameScore);

        MessageModel CreatePaymentSuccessfulMessage(
            Message message,
            TdApi.MessageContent.MessagePaymentSuccessful paymentSuccessful);

        MessageModel CreatePaymentSuccessfulBotMessage(
            Message message,
            TdApi.MessageContent.MessagePaymentSuccessfulBot paymentSuccessfulBot);

        MessageModel CreateContactRegisteredMessage(
            Message message,
            TdApi.MessageContent.MessageContactRegistered contactRegistered);

        MessageModel CreateWebsiteConnectedMessage(
            Message message,
            TdApi.MessageContent.MessageWebsiteConnected websiteConnected);

        MessageModel CreateLocationMessage(
            Message message,
            TdApi.MessageContent.MessageLocation location);

        MessageModel CreateVenueMessage(
            Message message,
            TdApi.MessageContent.MessageVenue venue);

        MessageModel CreateContactMessage(
            Message message,
            TdApi.MessageContent.MessageContact contact);

        MessageModel CreateGameMessage(
            Message message,
            TdApi.MessageContent.MessageGame game);

        MessageModel CreateInvoiceMessage(
            Message message,
            TdApi.MessageContent.MessageInvoice invoice);

        MessageModel CreatePassportDataSentMessage(
            Message message,
            TdApi.MessageContent.MessagePassportDataSent passportDataSent);

        MessageModel CreatePassportDataReceivedMessage(
            Message message,
            TdApi.MessageContent.MessagePassportDataReceived passportDataReceived);

        MessageModel CreateAudioMessage(
            Message message,
            TdApi.MessageContent.MessageAudio messageAudio);

        MessageModel CreateVoiceNoteMessage(
            Message message,
            TdApi.MessageContent.MessageVoiceNote voiceNote);
    }
}