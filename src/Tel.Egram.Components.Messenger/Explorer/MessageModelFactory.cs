using TdLib;
using Tel.Egram.Components.Messenger.Explorer.Messages;
using Tel.Egram.Components.Messenger.Explorer.Messages.Basic;
using Tel.Egram.Components.Messenger.Explorer.Messages.Visual;
using Tel.Egram.Graphics;
using Tel.Egram.Messaging.Messages;

namespace Tel.Egram.Components.Messenger.Explorer
{
    public partial class MessageModelFactory : IMessageModelFactory
    {
        public MessageModel CreateMessage(Message message)
        {
            var messageData = message.MessageData;
            var content = messageData.Content;

            switch (content)
            {
                // basic
                case TdApi.MessageContent.MessageText messageText:
                    return CreateTextMessage(message, messageText);
                
                // visual
                case TdApi.MessageContent.MessagePhoto messagePhoto:
                    return CreatePhotoMessage(message, messagePhoto);
                
                case TdApi.MessageContent.MessageExpiredPhoto expiredPhoto:
                    return CreateExpiredPhotoMessage(message, expiredPhoto);
                
                case TdApi.MessageContent.MessageSticker messageSticker:
                    return CreateStickerMessage(message, messageSticker);
                
                case TdApi.MessageContent.MessageAnimation messageAnimation:
                    return CreateAnimationMessage(message, messageAnimation);
                
                case TdApi.MessageContent.MessageVideo messageVideo:
                    return CreateVideoMessage(message, messageVideo);
                
                case TdApi.MessageContent.MessageExpiredVideo expiredVideo:
                    return CreateExpiredVideoMessage(message, expiredVideo);
                
                case TdApi.MessageContent.MessageVideoNote videoNote:
                    return CreateVideoNoteMessage(message, videoNote);
                
                // special
                case TdApi.MessageContent.MessageDocument messageDocument:
                    return CreateDocumentMessage(message, messageDocument);
                
                case TdApi.MessageContent.MessageAudio messageAudio:
                    return CreateAudioMessage(message, messageAudio);
                
                case TdApi.MessageContent.MessageVoiceNote voiceNote:
                    return CreateVoiceNoteMessage(message, voiceNote);
                
                case TdApi.MessageContent.MessagePaymentSuccessful paymentSuccessful:
                    return CreatePaymentSuccessfulMessage(message, paymentSuccessful);
                
                case TdApi.MessageContent.MessagePaymentSuccessfulBot paymentSuccessfulBot:
                    return CreatePaymentSuccessfulBotMessage(message, paymentSuccessfulBot);
                
                case TdApi.MessageContent.MessageLocation location:
                    return CreateLocationMessage(message, location);
                
                case TdApi.MessageContent.MessageVenue venue:
                    return CreateVenueMessage(message, venue);
                
                case TdApi.MessageContent.MessageContact contact:
                    return CreateContactMessage(message, contact);
                
                case TdApi.MessageContent.MessageGame game:
                    return CreateGameMessage(message, game);
                
                case TdApi.MessageContent.MessageGameScore gameScore:
                    return CreateGameScoreMessage(message, gameScore);
                
                case TdApi.MessageContent.MessageInvoice invoice:
                    return CreateInvoiceMessage(message, invoice);
                
                case TdApi.MessageContent.MessagePassportDataSent passportDataSent:
                    return CreatePassportDataSentMessage(message, passportDataSent);
                
                case TdApi.MessageContent.MessagePassportDataReceived passportDataReceived:
                    return CreatePassportDataReceivedMessage(message, passportDataReceived);
                
                case TdApi.MessageContent.MessageContactRegistered contactRegistered:
                    return CreateContactRegisteredMessage(message, contactRegistered);
                
                case TdApi.MessageContent.MessageWebsiteConnected websiteConnected:
                    return CreateWebsiteConnectedMessage(message, websiteConnected);
                
                // notes
                case TdApi.MessageContent.MessageCall messageCall:
                    return CreateCallMessage(message, messageCall);
                
                case TdApi.MessageContent.MessageBasicGroupChatCreate basicGroupChatCreate:
                    return CreateBasicGroupChatCreateMessage(message, basicGroupChatCreate);
                
                case TdApi.MessageContent.MessageChatChangeTitle chatChangeTitle:
                    return CreateChatChangeTitleMessage(message, chatChangeTitle);
                
                case TdApi.MessageContent.MessageChatChangePhoto chatChangePhoto:
                    return CreateChatChangePhotoMessage(message, chatChangePhoto);
                
                case TdApi.MessageContent.MessageChatDeletePhoto chatDeletePhoto:
                    return CreateChatDeletePhotoMessage(message, chatDeletePhoto);
                
                case TdApi.MessageContent.MessageChatAddMembers chatAddMembers:
                    return CreateChatAddMembersMessage(message, chatAddMembers);
                
                case TdApi.MessageContent.MessageChatJoinByLink chatJoinByLink:
                    return CreateChatJoinByLinkMessage(message, chatJoinByLink);
                
                case TdApi.MessageContent.MessageChatDeleteMember chatDeleteMember:
                    return CreateChatDeleteMemberMessage(message, chatDeleteMember);
                
                case TdApi.MessageContent.MessageChatUpgradeTo chatUpgradeTo:
                    return CreateChatUpgradeToMessage(message, chatUpgradeTo);
                
                case TdApi.MessageContent.MessageChatUpgradeFrom chatUpgradeFrom:
                    return CreateChatUpgradeFromMessage(message, chatUpgradeFrom);
                
                case TdApi.MessageContent.MessagePinMessage pinMessage:
                    return CreatePinMessageMessage(message, pinMessage);
                
                case TdApi.MessageContent.MessageScreenshotTaken screenshotTaken:
                    return CreateScreenshotTakenMessage(message, screenshotTaken);
                
                case TdApi.MessageContent.MessageChatSetTtl chatSetTtl:
                    return CreateChatSetTtlMessage(message, chatSetTtl);
                
                case TdApi.MessageContent.MessageCustomServiceAction customServiceAction:
                    return CreateCustomServiceActionMessage(message, customServiceAction);
                
                default:
                    return CreateUnsupportedMessage(message);
            }
        }
    }
}