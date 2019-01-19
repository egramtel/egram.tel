using TdLib;
using Tel.Egram.Model.Messenger.Explorer.Messages;
using Tel.Egram.Services.Messaging.Messages;

namespace Tel.Egram.Model.Messenger.Explorer.Factories
{
    public class NoteMessageModelFactory : INoteMessageModelFactory
    {
        public MessageModel CreateCallMessage(Message message, TdApi.MessageContent.MessageCall messageCall)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        public MessageModel CreateBasicGroupChatCreateMessage(Message message, TdApi.MessageContent.MessageBasicGroupChatCreate basicGroupChatCreate)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        public MessageModel CreateChatChangeTitleMessage(Message message, TdApi.MessageContent.MessageChatChangeTitle chatChangeTitle)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        public MessageModel CreateChatChangePhotoMessage(Message message, TdApi.MessageContent.MessageChatChangePhoto chatChangePhoto)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        public MessageModel CreateChatDeletePhotoMessage(Message message, TdApi.MessageContent.MessageChatDeletePhoto chatDeletePhoto)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        public MessageModel CreateChatAddMembersMessage(Message message, TdApi.MessageContent.MessageChatAddMembers chatAddMembers)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        public MessageModel CreateChatJoinByLinkMessage(Message message, TdApi.MessageContent.MessageChatJoinByLink chatJoinByLink)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        public MessageModel CreateChatDeleteMemberMessage(Message message, TdApi.MessageContent.MessageChatDeleteMember chatDeleteMember)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        public MessageModel CreateChatUpgradeToMessage(Message message, TdApi.MessageContent.MessageChatUpgradeTo chatUpgradeTo)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        public MessageModel CreateChatUpgradeFromMessage(Message message, TdApi.MessageContent.MessageChatUpgradeFrom chatUpgradeFrom)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        public MessageModel CreatePinMessageMessage(Message message, TdApi.MessageContent.MessagePinMessage pinMessage)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        public MessageModel CreateScreenshotTakenMessage(Message message, TdApi.MessageContent.MessageScreenshotTaken screenshotTaken)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        public MessageModel CreateChatSetTtlMessage(Message message, TdApi.MessageContent.MessageChatSetTtl chatSetTtl)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        public MessageModel CreateCustomServiceActionMessage(Message message, TdApi.MessageContent.MessageCustomServiceAction customServiceAction)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }
    }
}