using TdLib;

namespace Tel.Egram.Components.Catalog
{
    public static class CatalogFilter
    {
        public static bool All(EntryModelProxy modelProxy)
        {
            return true;
        }
        
        public static bool BotFilter(EntryModelProxy modelProxy)
        {
            if (modelProxy.EntryModel is ChatEntryModel chatEntryModel)
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

        public static bool DirectFilter(EntryModelProxy modelProxy)
        {
            if (modelProxy.EntryModel is ChatEntryModel chatEntryModel)
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

        public static bool GroupFilter(EntryModelProxy modelProxy)
        {
            if (modelProxy.EntryModel is ChatEntryModel chatEntryModel)
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

        public static bool ChannelFilter(EntryModelProxy modelProxy)
        {
            if (modelProxy.EntryModel is ChatEntryModel chatEntryModel)
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