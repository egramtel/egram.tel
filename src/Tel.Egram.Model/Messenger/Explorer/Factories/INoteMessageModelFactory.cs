using TdLib;
using Tel.Egram.Model.Messenger.Explorer.Messages;
using Tel.Egram.Services.Messaging.Messages;

namespace Tel.Egram.Model.Messenger.Explorer.Factories
{
    public interface INoteMessageModelFactory
    {
        MessageModel CreateCallMessage(Message message, TdApi.MessageContent.MessageCall messageCall);
        MessageModel CreateBasicGroupChatCreateMessage(Message message, TdApi.MessageContent.MessageBasicGroupChatCreate basicGroupChatCreate);
        MessageModel CreateChatChangeTitleMessage(Message message, TdApi.MessageContent.MessageChatChangeTitle chatChangeTitle);
        MessageModel CreateChatChangePhotoMessage(Message message, TdApi.MessageContent.MessageChatChangePhoto chatChangePhoto);
        MessageModel CreateChatDeletePhotoMessage(Message message, TdApi.MessageContent.MessageChatDeletePhoto chatDeletePhoto);
        MessageModel CreateChatAddMembersMessage(Message message, TdApi.MessageContent.MessageChatAddMembers chatAddMembers);
        MessageModel CreateChatJoinByLinkMessage(Message message, TdApi.MessageContent.MessageChatJoinByLink chatJoinByLink);
        MessageModel CreateChatDeleteMemberMessage(Message message, TdApi.MessageContent.MessageChatDeleteMember chatDeleteMember);
        MessageModel CreateChatUpgradeToMessage(Message message, TdApi.MessageContent.MessageChatUpgradeTo chatUpgradeTo);
        MessageModel CreateChatUpgradeFromMessage(Message message, TdApi.MessageContent.MessageChatUpgradeFrom chatUpgradeFrom);
        MessageModel CreatePinMessageMessage(Message message, TdApi.MessageContent.MessagePinMessage pinMessage);
        MessageModel CreateScreenshotTakenMessage(Message message, TdApi.MessageContent.MessageScreenshotTaken screenshotTaken);
        MessageModel CreateChatSetTtlMessage(Message message, TdApi.MessageContent.MessageChatSetTtl chatSetTtl);
        MessageModel CreateCustomServiceActionMessage(Message message, TdApi.MessageContent.MessageCustomServiceAction customServiceAction);
    }
}