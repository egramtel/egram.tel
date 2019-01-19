using System;
using System.Linq;
using TdLib;
using Tel.Egram.Model.Messenger.Explorer.Messages;
using Tel.Egram.Services.Messaging.Messages;
using Tel.Egram.Services.Utils.Formatting;

namespace Tel.Egram.Model.Messenger.Explorer.Factories
{
    public class MessageModelFactory : IMessageModelFactory
    {
        private readonly IBasicMessageModelFactory _basicMessageModelFactory;
        private readonly INoteMessageModelFactory _noteMessageModelFactory;
        private readonly ISpecialMessageModelFactory _specialMessageModelFactory;
        private readonly IVisualMessageModelFactory _visualMessageModelFactory;
        
        private readonly IStringFormatter _stringFormatter;

        public MessageModelFactory(
            IBasicMessageModelFactory basicMessageModelFactory,
            INoteMessageModelFactory noteMessageModelFactory,
            ISpecialMessageModelFactory specialMessageModelFactory,
            IVisualMessageModelFactory visualMessageModelFactory,
            IStringFormatter stringFormatter)
        {
            _basicMessageModelFactory = basicMessageModelFactory;
            _noteMessageModelFactory = noteMessageModelFactory;
            _specialMessageModelFactory = specialMessageModelFactory;
            _visualMessageModelFactory = visualMessageModelFactory;
            
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
                    return _basicMessageModelFactory.CreateTextMessage(message, messageText);
                
                // visual
                case TdApi.MessageContent.MessagePhoto messagePhoto:
                    return _visualMessageModelFactory.CreatePhotoMessage(message, messagePhoto);
                
                case TdApi.MessageContent.MessageExpiredPhoto expiredPhoto:
                    return _visualMessageModelFactory.CreateExpiredPhotoMessage(message, expiredPhoto);
                
                case TdApi.MessageContent.MessageSticker messageSticker:
                    return _visualMessageModelFactory.CreateStickerMessage(message, messageSticker);
                
                case TdApi.MessageContent.MessageAnimation messageAnimation:
                    return _visualMessageModelFactory.CreateAnimationMessage(message, messageAnimation);
                
                case TdApi.MessageContent.MessageVideo messageVideo:
                    return _visualMessageModelFactory.CreateVideoMessage(message, messageVideo);
                
                case TdApi.MessageContent.MessageExpiredVideo expiredVideo:
                    return _visualMessageModelFactory.CreateExpiredVideoMessage(message, expiredVideo);
                
                case TdApi.MessageContent.MessageVideoNote videoNote:
                    return _visualMessageModelFactory.CreateVideoNoteMessage(message, videoNote);
                
                // special
                case TdApi.MessageContent.MessageDocument messageDocument:
                    return _specialMessageModelFactory.CreateDocumentMessage(message, messageDocument);
                
                case TdApi.MessageContent.MessageAudio messageAudio:
                    return _specialMessageModelFactory.CreateAudioMessage(message, messageAudio);
                
                case TdApi.MessageContent.MessageVoiceNote voiceNote:
                    return _specialMessageModelFactory.CreateVoiceNoteMessage(message, voiceNote);
                
                case TdApi.MessageContent.MessagePaymentSuccessful paymentSuccessful:
                    return _specialMessageModelFactory.CreatePaymentSuccessfulMessage(message, paymentSuccessful);
                
                case TdApi.MessageContent.MessagePaymentSuccessfulBot paymentSuccessfulBot:
                    return _specialMessageModelFactory.CreatePaymentSuccessfulBotMessage(message, paymentSuccessfulBot);
                
                case TdApi.MessageContent.MessageLocation location:
                    return _specialMessageModelFactory.CreateLocationMessage(message, location);
                
                case TdApi.MessageContent.MessageVenue venue:
                    return _specialMessageModelFactory.CreateVenueMessage(message, venue);
                
                case TdApi.MessageContent.MessageContact contact:
                    return _specialMessageModelFactory.CreateContactMessage(message, contact);
                
                case TdApi.MessageContent.MessageGame game:
                    return _specialMessageModelFactory.CreateGameMessage(message, game);
                
                case TdApi.MessageContent.MessageGameScore gameScore:
                    return _specialMessageModelFactory.CreateGameScoreMessage(message, gameScore);
                
                case TdApi.MessageContent.MessageInvoice invoice:
                    return _specialMessageModelFactory.CreateInvoiceMessage(message, invoice);
                
                case TdApi.MessageContent.MessagePassportDataSent passportDataSent:
                    return _specialMessageModelFactory.CreatePassportDataSentMessage(message, passportDataSent);
                
                case TdApi.MessageContent.MessagePassportDataReceived passportDataReceived:
                    return _specialMessageModelFactory.CreatePassportDataReceivedMessage(message, passportDataReceived);
                
                case TdApi.MessageContent.MessageContactRegistered contactRegistered:
                    return _specialMessageModelFactory.CreateContactRegisteredMessage(message, contactRegistered);
                
                case TdApi.MessageContent.MessageWebsiteConnected websiteConnected:
                    return _specialMessageModelFactory.CreateWebsiteConnectedMessage(message, websiteConnected);
                
                // notes
                case TdApi.MessageContent.MessageCall messageCall:
                    return _noteMessageModelFactory.CreateCallMessage(message, messageCall);
                
                case TdApi.MessageContent.MessageBasicGroupChatCreate basicGroupChatCreate:
                    return _noteMessageModelFactory.CreateBasicGroupChatCreateMessage(message, basicGroupChatCreate);
                
                case TdApi.MessageContent.MessageChatChangeTitle chatChangeTitle:
                    return _noteMessageModelFactory.CreateChatChangeTitleMessage(message, chatChangeTitle);
                
                case TdApi.MessageContent.MessageChatChangePhoto chatChangePhoto:
                    return _noteMessageModelFactory.CreateChatChangePhotoMessage(message, chatChangePhoto);
                
                case TdApi.MessageContent.MessageChatDeletePhoto chatDeletePhoto:
                    return _noteMessageModelFactory.CreateChatDeletePhotoMessage(message, chatDeletePhoto);
                
                case TdApi.MessageContent.MessageChatAddMembers chatAddMembers:
                    return _noteMessageModelFactory.CreateChatAddMembersMessage(message, chatAddMembers);
                
                case TdApi.MessageContent.MessageChatJoinByLink chatJoinByLink:
                    return _noteMessageModelFactory.CreateChatJoinByLinkMessage(message, chatJoinByLink);
                
                case TdApi.MessageContent.MessageChatDeleteMember chatDeleteMember:
                    return _noteMessageModelFactory.CreateChatDeleteMemberMessage(message, chatDeleteMember);
                
                case TdApi.MessageContent.MessageChatUpgradeTo chatUpgradeTo:
                    return _noteMessageModelFactory.CreateChatUpgradeToMessage(message, chatUpgradeTo);
                
                case TdApi.MessageContent.MessageChatUpgradeFrom chatUpgradeFrom:
                    return _noteMessageModelFactory.CreateChatUpgradeFromMessage(message, chatUpgradeFrom);
                
                case TdApi.MessageContent.MessagePinMessage pinMessage:
                    return _noteMessageModelFactory.CreatePinMessageMessage(message, pinMessage);
                
                case TdApi.MessageContent.MessageScreenshotTaken screenshotTaken:
                    return _noteMessageModelFactory.CreateScreenshotTakenMessage(message, screenshotTaken);
                
                case TdApi.MessageContent.MessageChatSetTtl chatSetTtl:
                    return _noteMessageModelFactory.CreateChatSetTtlMessage(message, chatSetTtl);
                
                case TdApi.MessageContent.MessageCustomServiceAction customServiceAction:
                    return _noteMessageModelFactory.CreateCustomServiceActionMessage(message, customServiceAction);
                
                default:
                    return _basicMessageModelFactory.CreateUnsupportedMessage(message);
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
            model.Time = _stringFormatter.FormatShortTime(message.MessageData.Date);

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