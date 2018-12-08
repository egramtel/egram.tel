using TdLib;
using Tel.Egram.Components.Messenger.Catalog.Entries;
using Tel.Egram.Messaging.Chats;

namespace Tel.Egram.Components.Messenger.Catalog
{
    public static class CatalogFilter
    {
        public static bool All(EntryModel model)
        {
            return true;
        }
        
        public static bool BotFilter(EntryModel model)
        {
            if (model is ChatEntryModel chatEntryModel)
            {
                var chat = chatEntryModel.Chat;
                if (chat.ChatData.Type is TdApi.ChatType.ChatTypePrivate)
                {
                    return chat.User != null &&
                           chat.User.Type is TdApi.UserType.UserTypeBot;
                }
            }
            return false;
        }

        public static bool DirectFilter(EntryModel model)
        {
            if (model is ChatEntryModel chatEntryModel)
            {
                var chat = chatEntryModel.Chat;
                if (chat.ChatData.Type is TdApi.ChatType.ChatTypePrivate)
                {
                    return chat.User != null &&
                           chat.User.Type is TdApi.UserType.UserTypeRegular;
                }
            }
            return false;
        }

        public static bool GroupFilter(EntryModel model)
        {
            if (model is ChatEntryModel chatEntryModel)
            {
                var chat = chatEntryModel.Chat;
                if (chat.ChatData.Type is TdApi.ChatType.ChatTypeSupergroup supergroupType)
                {
                    return !supergroupType.IsChannel;
                }

                return chat.ChatData.Type is TdApi.ChatType.ChatTypeBasicGroup;
            }
            return false;
        }

        public static bool ChannelFilter(EntryModel model)
        {
            if (model is ChatEntryModel chatEntryModel)
            {
                var chat = chatEntryModel.Chat;
                if (chat.ChatData.Type is TdApi.ChatType.ChatTypeSupergroup supergroupType)
                {
                    return supergroupType.IsChannel;
                }
            }
            return false;
        }
    }
}