using System;
using System.Linq;
using TdLib;
using Tel.Egram.Model.Messenger.Explorer.Messages;
using Tel.Egram.Services.Messaging.Messages;
using Tel.Egram.Services.Utils.Formatting;

namespace Tel.Egram.Model.Messenger.Explorer.Factories
{
    public partial class MessageModelFactory : IMessageModelFactory
    {
        private readonly IStringFormatter _stringFormatter;

        public MessageModelFactory(
            IStringFormatter stringFormatter)
        {
            _stringFormatter = stringFormatter;
        }
        
        public MessageModel CreateMessage(Message message)
        {
            var model = GetMessage(message);
            ApplyMessageAttributes(model, message);
            return model;
        }

        private MessageModel GetMessage(Message message)
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

        private void ApplyMessageAttributes(MessageModel model, Message message)
        {
            var user = message.UserData;
            var chat = message.ChatData;

            var authorName = (user == null)
                ? chat.Title
                : $"{user.FirstName} {user.LastName}";
                
            model.Message = message;
            model.AuthorName = authorName;
            model.Time = _stringFormatter.AsShortTime(message.MessageData.Date);

            if (message.ReplyMessage != null)
            {
                model.HasReply = true;
                model.Reply = new ReplyModel();
                
                model.Reply.Message = message.ReplyMessage;
                model.Reply.AuthorName = GetReplyAuthorName(message.ReplyMessage);
                model.Reply.Text = GetReplyText(message.ReplyMessage);
                model.Reply.PhotoData = GetReplyPhoto(message.ReplyMessage);
                model.Reply.VideoData = GetReplyVideo(message.ReplyMessage);
                model.Reply.StickerData = GetReplySticker(message.ReplyMessage);
            }
        }

        private string GetReplyAuthorName(Message message)
        {
            var replyUser = message.UserData;
            var replyChat = message.ChatData;
            
            var replyAuthorName = (replyUser == null)
                ? replyChat.Title
                : $"{replyUser.FirstName} {replyUser.LastName}";

            return replyAuthorName;
        }

        private string GetReplyText(Message message)
        {
            var messageData = message.MessageData;
            var content = messageData.Content;

            string text = null;
            switch (content)
            {
                case TdApi.MessageContent.MessageText messageText:
                    text = messageText.Text?.Text;
                    break;
                
                case TdApi.MessageContent.MessagePhoto messagePhoto:
                    text = messagePhoto.Caption?.Text;
                    break;
            }

            if (text == null)
                return text;
            
            return new string(
                text.Take(64)
                    .TakeWhile(c => c != '\n' && c != '\r')
                    .ToArray());
        }

        private TdApi.Photo GetReplyPhoto(Message message)
        {
            if (message.MessageData.Content is TdApi.MessageContent.MessagePhoto messagePhoto)
            {
                return messagePhoto.Photo;
            }

            return null;
        }

        private TdApi.Video GetReplyVideo(Message message)
        {
            if (message.MessageData.Content is TdApi.MessageContent.MessageVideo messageVideo)
            {
                return messageVideo.Video;
            }

            return null;
        }

        private TdApi.Sticker GetReplySticker(Message message)
        {
            if (message.MessageData.Content is TdApi.MessageContent.MessageSticker messageSticker)
            {
                return messageSticker.Sticker;
            }

            return null;
        }
    }
}