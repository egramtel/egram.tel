using TdLib;
using Tel.Egram.Model.Messenger.Explorer.Messages;
using Tel.Egram.Services.Messaging.Messages;

namespace Tel.Egram.Model.Messenger.Explorer
{
    public partial class MessageModelFactory : IMessageModelFactory
    {
        private MessageModel CreateCallMessage(Message message, TdApi.MessageContent.MessageCall messageCall)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        private MessageModel CreateBasicGroupChatCreateMessage(Message message, TdApi.MessageContent.MessageBasicGroupChatCreate basicGroupChatCreate)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        private MessageModel CreateChatChangeTitleMessage(Message message, TdApi.MessageContent.MessageChatChangeTitle chatChangeTitle)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        private MessageModel CreateChatChangePhotoMessage(Message message, TdApi.MessageContent.MessageChatChangePhoto chatChangePhoto)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        private MessageModel CreateChatDeletePhotoMessage(Message message, TdApi.MessageContent.MessageChatDeletePhoto chatDeletePhoto)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        private MessageModel CreateChatAddMembersMessage(Message message, TdApi.MessageContent.MessageChatAddMembers chatAddMembers)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        private MessageModel CreateChatJoinByLinkMessage(Message message, TdApi.MessageContent.MessageChatJoinByLink chatJoinByLink)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        private MessageModel CreateChatDeleteMemberMessage(Message message, TdApi.MessageContent.MessageChatDeleteMember chatDeleteMember)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        private MessageModel CreateChatUpgradeToMessage(Message message, TdApi.MessageContent.MessageChatUpgradeTo chatUpgradeTo)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        private MessageModel CreateChatUpgradeFromMessage(Message message, TdApi.MessageContent.MessageChatUpgradeFrom chatUpgradeFrom)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        private MessageModel CreatePinMessageMessage(Message message, TdApi.MessageContent.MessagePinMessage pinMessage)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        private MessageModel CreateScreenshotTakenMessage(Message message, TdApi.MessageContent.MessageScreenshotTaken screenshotTaken)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        private MessageModel CreateChatSetTtlMessage(Message message, TdApi.MessageContent.MessageChatSetTtl chatSetTtl)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        private MessageModel CreateCustomServiceActionMessage(Message message, TdApi.MessageContent.MessageCustomServiceAction customServiceAction)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }
    }
}