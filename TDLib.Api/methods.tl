//@description Returns the current authorization state; this is an offline request. For informational purposes only. Use updateAuthorizationState instead to maintain the current authorization state
getAuthorizationState = AuthorizationState;


//@description Sets the parameters for TDLib initialization. Works only when the current authorization state is authorizationStateWaitTdlibParameters @parameters Parameters
setTdlibParameters parameters:tdlibParameters = Ok;

//@description Checks the database encryption key for correctness. Works only when the current authorization state is authorizationStateWaitEncryptionKey @encryption_key Encryption key to check or set up
checkDatabaseEncryptionKey encryption_key:bytes = Ok;

//@description Sets the phone number of the user and sends an authentication code to the user. Works only when the current authorization state is authorizationStateWaitPhoneNumber
//@phone_number The phone number of the user, in international format @allow_flash_call Pass true if the authentication code may be sent via flash call to the specified phone number @is_current_phone_number Pass true if the phone number is used on the current device. Ignored if allow_flash_call is false
setAuthenticationPhoneNumber phone_number:string allow_flash_call:Bool is_current_phone_number:Bool = Ok;

//@description Re-sends an authentication code to the user. Works only when the current authorization state is authorizationStateWaitCode and the next_code_type of the result is not null
resendAuthenticationCode = Ok;

//@description Checks the authentication code. Works only when the current authorization state is authorizationStateWaitCode @code The verification code received via SMS, Telegram message, phone call, or flash call
//@first_name If the user is not yet registered, the first name of the user; 1-255 characters @last_name If the user is not yet registered; the last name of the user; optional; 0-255 characters
checkAuthenticationCode code:string first_name:string last_name:string = Ok;

//@description Checks the authentication password for correctness. Works only when the current authorization state is authorizationStateWaitPassword @password The password to check
checkAuthenticationPassword password:string = Ok;

//@description Requests to send a password recovery code to an email address that was previously set up. Works only when the current authorization state is authorizationStateWaitPassword
requestAuthenticationPasswordRecovery = Ok;

//@description Recovers the password with a password recovery code sent to an email address that was previously set up. Works only when the current authorization state is authorizationStateWaitPassword @recovery_code Recovery code to check
recoverAuthenticationPassword recovery_code:string = Ok;

//@description Checks the authentication token of a bot; to log in as a bot. Works only when the current authorization state is authorizationStateWaitPhoneNumber. Can be used instead of setAuthenticationPhoneNumber and checkAuthenticationCode to log in @token The bot token
checkAuthenticationBotToken token:string = Ok;

//@description Closes the TDLib instance after a proper logout. Requires an available network connection. All local data will be destroyed. After the logout completes, updateAuthorizationState with authorizationStateClosed will be sent
logOut = Ok;

//@description Closes the TDLib instance. All databases will be flushed to disk and properly closed. After the close completes, updateAuthorizationState with authorizationStateClosed will be sent
close = Ok;

//@description Closes the TDLib instance, destroying all local data without a proper logout. The current user session will remain in the list of all active sessions. All local data will be destroyed. After the destruction completes updateAuthorizationState with authorizationStateClosed will be sent
destroy = Ok;


//@description Changes the database encryption key. Usually the encryption key is never changed and is stored in some OS keychain @new_encryption_key New encryption key
setDatabaseEncryptionKey new_encryption_key:bytes = Ok;


//@description Returns the current state of 2-step verification
getPasswordState = PasswordState;

//@description Changes the password for the user. If a new recovery email address is specified, then the error EMAIL_UNCONFIRMED is returned and the password change will not be applied until the new recovery email address has been confirmed. The application should periodically call getPasswordState to check whether the new email address has been confirmed
//@old_password Previous password of the user @new_password New password of the user; may be empty to remove the password @new_hint New password hint; may be empty @set_recovery_email_address Pass true if the recovery email address should be changed @new_recovery_email_address New recovery email address; may be empty
setPassword old_password:string new_password:string new_hint:string set_recovery_email_address:Bool new_recovery_email_address:string = PasswordState;

//@description Returns a recovery email address that was previously set up. This method can be used to verify a password provided by the user @password The password for the current user
getRecoveryEmailAddress password:string = RecoveryEmailAddress;

//@description Changes the recovery email address of the user. If a new recovery email address is specified, then the error EMAIL_UNCONFIRMED is returned and the email address will not be changed until the new email has been confirmed. The application should periodically call getPasswordState to check whether the email address has been confirmed.
//-If new_recovery_email_address is the same as the email address that is currently set up, this call succeeds immediately and aborts all other requests waiting for an email confirmation @password Password of the current user @new_recovery_email_address New recovery email address
setRecoveryEmailAddress password:string new_recovery_email_address:string = PasswordState;

//@description Requests to send a password recovery code to an email address that was previously set up
requestPasswordRecovery = PasswordRecoveryInfo;

//@description Recovers the password using a recovery code sent to an email address that was previously set up @recovery_code Recovery code to check
recoverPassword recovery_code:string = PasswordState;

//@description Creates a new temporary password for processing payments @password Persistent user password @valid_for Time during which the temporary password will be valid, in seconds; should be between 60 and 86400
createTemporaryPassword password:string valid_for:int32 = TemporaryPasswordState;

//@description Returns information about the current temporary password
getTemporaryPasswordState = TemporaryPasswordState;


//@description Handles a DC_UPDATE push service notification. Can be called before authorization @dc Value of the "dc" parameter of the notification @addr Value of the "addr" parameter of the notification
processDcUpdate dc:string addr:string = Ok;


//@description Returns the current user
getMe = User;

//@description Returns information about a user by their identifier. This is an offline request if the current user is not a bot @user_id User identifier
getUser user_id:int32 = User;

//@description Returns full information about a user by their identifier @user_id User identifier
getUserFullInfo user_id:int32 = UserFullInfo;

//@description Returns information about a basic group by its identifier. This is an offline request if the current user is not a bot @basic_group_id Basic group identifier
getBasicGroup basic_group_id:int32 = BasicGroup;

//@description Returns full information about a basic group by its identifier @basic_group_id Basic group identifier
getBasicGroupFullInfo basic_group_id:int32 = BasicGroupFullInfo;

//@description Returns information about a supergroup or channel by its identifier. This is an offline request if the current user is not a bot @supergroup_id Supergroup or channel identifier
getSupergroup supergroup_id:int32 = Supergroup;

//@description Returns full information about a supergroup or channel by its identifier, cached for up to 1 minute @supergroup_id Supergroup or channel identifier
getSupergroupFullInfo supergroup_id:int32 = SupergroupFullInfo;

//@description Returns information about a secret chat by its identifier. This is an offline request @secret_chat_id Secret chat identifier
getSecretChat secret_chat_id:int32 = SecretChat;

//@description Returns information about a chat by its identifier, this is an offline request if the current user is not a bot @chat_id Chat identifier
getChat chat_id:int53 = Chat;

//@description Returns information about a message @chat_id Identifier of the chat the message belongs to @message_id Identifier of the message to get
getMessage chat_id:int53 message_id:int53 = Message;

//@description Returns information about a message that is replied by given message @chat_id Identifier of the chat the message belongs to @message_id Identifier of the message reply to which get
getRepliedMessage chat_id:int53 message_id:int53 = Message;

//@description Returns information about a pinned chat message @chat_id Identifier of the chat the message belongs to
getChatPinnedMessage chat_id:int53 = Message;

//@description Returns information about messages. If a message is not found, returns null on the corresponding position of the result @chat_id Identifier of the chat the messages belong to @message_ids Identifiers of the messages to get
getMessages chat_id:int53 message_ids:vector<int53> = Messages;

//@description Returns information about a file; this is an offline request @file_id Identifier of the file to get
getFile file_id:int32 = File;

//@description Returns information about a file by its remote ID; this is an offline request. Can be used to register a URL as a file for further uploading, or sending as a message @remote_file_id Remote identifier of the file to get @file_type File type, if known
getRemoteFile remote_file_id:string file_type:FileType = File;

//@description Returns an ordered list of chats. Chats are sorted by the pair (order, chat_id) in decreasing order. (For example, to get a list of chats from the beginning, the offset_order should be equal to 2^63 - 1).
//-For optimal performance the number of returned chats is chosen by the library. @offset_order Chat order to return chats from @offset_chat_id Chat identifier to return chats from
//@limit The maximum number of chats to be returned. It is possible that fewer chats than the limit are returned even if the end of the list is not reached
getChats offset_order:int64 offset_chat_id:int53 limit:int32 = Chats;

//@description Searches a public chat by its username. Currently only private chats, supergroups and channels can be public. Returns the chat if found; otherwise an error is returned @username Username to be resolved
searchPublicChat username:string = Chat;

//@description Searches public chats by looking for specified query in their username and title. Currently only private chats, supergroups and channels can be public. Returns a meaningful number of results. Returns nothing if the length of the searched username prefix is less than 5. Excludes private chats with contacts and chats from the chat list from the results @query Query to search for
searchPublicChats query:string = Chats;

//@description Searches for the specified query in the title and username of already known chats, this is an offline request. Returns chats in the order seen in the chat list @query Query to search for. If the query is empty, returns up to 20 recently found chats @limit Maximum number of chats to be returned
searchChats query:string limit:int32 = Chats;

//@description Searches for the specified query in the title and username of already known chats via request to the server. Returns chats in the order seen in the chat list @query Query to search for @limit Maximum number of chats to be returned
searchChatsOnServer query:string limit:int32 = Chats;

//@description Returns a list of frequently used chats. Supported only if the chat info database is enabled @category Category of chats to be returned @limit Maximum number of chats to be returned; up to 30
getTopChats category:TopChatCategory limit:int32 = Chats;

//@description Removes a chat from the list of frequently used chats. Supported only if the chat info database is enabled @category Category of frequently used chats @chat_id Chat identifier
removeTopChat category:TopChatCategory chat_id:int53 = Ok;

//@description Adds a chat to the list of recently found chats. The chat is added to the beginning of the list. If the chat is already in the list, it will be removed from the list first @chat_id Identifier of the chat to add
addRecentlyFoundChat chat_id:int53 = Ok;

//@description Removes a chat from the list of recently found chats @chat_id Identifier of the chat to be removed
removeRecentlyFoundChat chat_id:int53 = Ok;

//@description Clears the list of recently found chats
clearRecentlyFoundChats = Ok;

//@description Checks whether a username can be set for a chat @chat_id Chat identifier; should be identifier of a supergroup chat, or a channel chat, or a private chat with self, or zero if chat is being created @username Username to be checked
checkChatUsername chat_id:int64 username:string = CheckChatUsernameResult;

//@description Returns a list of public chats created by the user
getCreatedPublicChats = Chats;


//@description Returns a list of common chats with a given user. Chats are sorted by their type and creation date @user_id User identifier @offset_chat_id Chat identifier starting from which to return chats; use 0 for the first request @limit Maximum number of chats to be returned; up to 100
getGroupsInCommon user_id:int32 offset_chat_id:int53 limit:int32 = Chats;


//@description Returns messages in a chat. The messages are returned in a reverse chronological order (i.e., in order of decreasing message_id).
//-For optimal performance the number of returned messages is chosen by the library. This is an offline request if only_local is true
//@chat_id Chat identifier
//@from_message_id Identifier of the message starting from which history must be fetched; use 0 to get results from the beginning (i.e., from oldest to newest)
//@offset Specify 0 to get results from exactly the from_message_id or a negative offset to get the specified message and some newer messages
//@limit The maximum number of messages to be returned; must be positive and can't be greater than 100. If the offset is negative, the limit must be greater than -offset. Fewer messages may be returned than specified by the limit, even if the end of the message history has not been reached
//@only_local If true, returns only messages that are available locally without sending network requests
getChatHistory chat_id:int53 from_message_id:int53 offset:int32 limit:int32 only_local:Bool = Messages;

//@description Deletes all messages in the chat only for the user. Cannot be used in channels and public supergroups @chat_id Chat identifier @remove_from_chat_list Pass true if the chat should be removed from the chats list
deleteChatHistory chat_id:int53 remove_from_chat_list:Bool = Ok;

//@description Searches for messages with given words in the chat. Returns the results in reverse chronological order, i.e. in order of decreasing message_id. Cannot be used in secret chats with a non-empty query
//-(searchSecretMessages should be used instead), or without an enabled message database. For optimal performance the number of returned messages is chosen by the library
//@chat_id Identifier of the chat in which to search messages
//@query Query to search for
//@sender_user_id If not 0, only messages sent by the specified user will be returned. Not supported in secret chats
//@from_message_id Identifier of the message starting from which history must be fetched; use 0 to get results from the beginning
//@offset Specify 0 to get results from exactly the from_message_id or a negative offset to get the specified message and some newer messages
//@limit The maximum number of messages to be returned; must be positive and can't be greater than 100. If the offset is negative, the limit must be greater than -offset. Fewer messages may be returned than specified by the limit, even if the end of the message history has not been reached
//@filter Filter for message content in the search results
searchChatMessages chat_id:int53 query:string sender_user_id:int32 from_message_id:int53 offset:int32 limit:int32 filter:SearchMessagesFilter = Messages;

//@description Searches for messages in all chats except secret chats. Returns the results in reverse chronological order (i.e., in order of decreasing (date, chat_id, message_id)).
//-For optimal performance the number of returned messages is chosen by the library
//@query Query to search for
//@offset_date The date of the message starting from which the results should be fetched. Use 0 or any date in the future to get results from the beginning
//@offset_chat_id The chat identifier of the last found message, or 0 for the first request
//@offset_message_id The message identifier of the last found message, or 0 for the first request
//@limit The maximum number of messages to be returned, up to 100. Fewer messages may be returned than specified by the limit, even if the end of the message history has not been reached
searchMessages query:string offset_date:int32 offset_chat_id:int53 offset_message_id:int53 limit:int32 = Messages;

//@description Searches for messages in secret chats. Returns the results in reverse chronological order. For optimal performance the number of returned messages is chosen by the library
//@chat_id Identifier of the chat in which to search. Specify 0 to search in all secret chats @query Query to search for. If empty, searchChatMessages should be used instead
//@from_search_id The identifier from the result of a previous request, use 0 to get results from the beginning
//@limit Maximum number of messages to be returned; up to 100. Fewer messages may be returned than specified by the limit, even if the end of the message history has not been reached
//@filter A filter for the content of messages in the search results
searchSecretMessages chat_id:int53 query:string from_search_id:int64 limit:int32 filter:SearchMessagesFilter = FoundMessages;

//@description Searches for call messages. Returns the results in reverse chronological order (i. e., in order of decreasing message_id). For optimal performance the number of returned messages is chosen by the library
//@from_message_id Identifier of the message from which to search; use 0 to get results from the beginning
//@limit The maximum number of messages to be returned; up to 100. Fewer messages may be returned than specified by the limit, even if the end of the message history has not been reached @only_missed If true, returns only messages with missed calls
searchCallMessages from_message_id:int53 limit:int32 only_missed:Bool = Messages;

//@description Returns information about the recent locations of chat members that were sent to the chat. Returns up to 1 location message per user @chat_id Chat identifier @limit Maximum number of messages to be returned
searchChatRecentLocationMessages chat_id:int53 limit:int32 = Messages;

//@description Returns all active live locations that should be updated by the client. The list is persistent across application restarts only if the message database is used
getActiveLiveLocationMessages = Messages;

//@description Returns the last message sent in a chat no later than the specified date @chat_id Chat identifier @date Point in time (Unix timestamp) relative to which to search for messages
getChatMessageByDate chat_id:int53 date:int32 = Message;


//@description Returns a public HTTPS link to a message. Available only for messages in public supergroups and channels
//@chat_id Identifier of the chat to which the message belongs
//@message_id Identifier of the message
//@for_album Pass true if a link for a whole media album should be returned
getPublicMessageLink chat_id:int53 message_id:int53 for_album:Bool = PublicMessageLink;


//@description Sends a message. Returns the sent message @chat_id Target chat @reply_to_message_id Identifier of the message to reply to or 0
//@disable_notification Pass true to disable notification for the message. Not supported in secret chats @from_background Pass true if the message is sent from the background
//@reply_markup Markup for replying to the message; for bots only @input_message_content The content of the message to be sent
sendMessage chat_id:int53 reply_to_message_id:int53 disable_notification:Bool from_background:Bool reply_markup:ReplyMarkup input_message_content:InputMessageContent = Message;

//@description Sends messages grouped together into an album. Currently only photo and video messages can be grouped into an album. Returns sent messages @chat_id Target chat @reply_to_message_id Identifier of a message to reply to or 0
//@disable_notification Pass true to disable notification for the messages. Not supported in secret chats @from_background Pass true if the messages are sent from the background
//@input_message_contents Contents of messages to be sent
sendMessageAlbum chat_id:int53 reply_to_message_id:int53 disable_notification:Bool from_background:Bool input_message_contents:vector<InputMessageContent> = Messages;

//@description Invites a bot to a chat (if it is not yet a member) and sends it the /start command. Bots can't be invited to a private chat other than the chat with the bot. Bots can't be invited to channels (although they can be added as admins) and secret chats. Returns the sent message
//@bot_user_id Identifier of the bot @chat_id Identifier of the target chat @parameter A hidden parameter sent to the bot for deep linking purposes (https://api.telegram.org/bots#deep-linking)
sendBotStartMessage bot_user_id:int32 chat_id:int53 parameter:string = Message;

//@description Sends the result of an inline query as a message. Returns the sent message. Always clears a chat draft message @chat_id Target chat @reply_to_message_id Identifier of a message to reply to or 0
//@disable_notification Pass true to disable notification for the message. Not supported in secret chats @from_background Pass true if the message is sent from background
//@query_id Identifier of the inline query @result_id Identifier of the inline result
sendInlineQueryResultMessage chat_id:int53 reply_to_message_id:int53 disable_notification:Bool from_background:Bool query_id:int64 result_id:string = Message;

//@description Forwards previously sent messages. Returns the forwarded messages in the same order as the message identifiers passed in message_ids. If a message can't be forwarded, null will be returned instead of the message
//@chat_id Identifier of the chat to which to forward messages @from_chat_id Identifier of the chat from which to forward messages @message_ids Identifiers of the messages to forward
//@disable_notification Pass true to disable notification for the message, doesn't work if messages are forwarded to a secret chat @from_background Pass true if the message is sent from the background
//@as_album True, if the messages should be grouped into an album after forwarding. For this to work, no more than 10 messages may be forwarded, and all of them must be photo or video messages
forwardMessages chat_id:int53 from_chat_id:int53 message_ids:vector<int53> disable_notification:Bool from_background:Bool as_album:Bool = Messages;

//@description Changes the current TTL setting (sets a new self-destruct timer) in a secret chat and sends the corresponding message @chat_id Chat identifier @ttl New TTL value, in seconds
sendChatSetTtlMessage chat_id:int53 ttl:int32 = Message;

//@description Sends a notification about a screenshot taken in a chat. Supported only in private and secret chats @chat_id Chat identifier
sendChatScreenshotTakenNotification chat_id:int53 = Ok;

//@description Deletes messages @chat_id Chat identifier @message_ids Identifiers of the messages to be deleted @revoke Pass true to try to delete outgoing messages for all chat members (may fail if messages are too old). Always true for supergroups, channels and secret chats
deleteMessages chat_id:int53 message_ids:vector<int53> revoke:Bool = Ok;

//@description Deletes all messages sent by the specified user to a chat. Supported only in supergroups; requires can_delete_messages administrator privileges @chat_id Chat identifier @user_id User identifier
deleteChatMessagesFromUser chat_id:int53 user_id:int32 = Ok;


//@description Edits the text of a message (or a text of a game message). Non-bot users can edit messages for a limited period of time. Returns the edited message after the edit is completed on the server side
//@chat_id The chat the message belongs to @message_id Identifier of the message @reply_markup The new message reply markup; for bots only @input_message_content New text content of the message. Should be of type InputMessageText
editMessageText chat_id:int53 message_id:int53 reply_markup:ReplyMarkup input_message_content:InputMessageContent = Message;

//@description Edits the message content of a live location. Messages can be edited for a limited period of time specified in the live location. Returns the edited message after the edit is completed server-side
//@chat_id The chat the message belongs to @message_id Identifier of the message @reply_markup Tew message reply markup; for bots only @location New location content of the message; may be null. Pass null to stop sharing the live location
editMessageLiveLocation chat_id:int53 message_id:int53 reply_markup:ReplyMarkup location:location = Message;

//@description Edits the message content caption. Non-bots can edit messages for a limited period of time. Returns the edited message after the edit is completed server-side
//@chat_id The chat the message belongs to @message_id Identifier of the message @reply_markup The new message reply markup; for bots only @caption New message content caption; 0-200 characters
editMessageCaption chat_id:int53 message_id:int53 reply_markup:ReplyMarkup caption:formattedText = Message;

//@description Edits the message reply markup; for bots only. Returns the edited message after the edit is completed server-side
//@chat_id The chat the message belongs to @message_id Identifier of the message @reply_markup New message reply markup
editMessageReplyMarkup chat_id:int53 message_id:int53 reply_markup:ReplyMarkup = Message;

//@description Edits the text of an inline text or game message sent via a bot; for bots only @inline_message_id Inline message identifier @reply_markup New message reply markup @input_message_content New text content of the message. Should be of type InputMessageText
editInlineMessageText inline_message_id:string reply_markup:ReplyMarkup input_message_content:InputMessageContent = Ok;

//@description Edits the content of a live location in an inline message sent via a bot; for bots only @inline_message_id Inline message identifier @reply_markup New message reply markup @location New location content of the message; may be null. Pass null to stop sharing the live location
editInlineMessageLiveLocation inline_message_id:string reply_markup:ReplyMarkup location:location = Ok;

//@description Edits the caption of an inline message sent via a bot; for bots only @inline_message_id Inline message identifier @reply_markup New message reply markup @caption New message content caption; 0-200 characters
editInlineMessageCaption inline_message_id:string reply_markup:ReplyMarkup caption:formattedText = Ok;

//@description Edits the reply markup of an inline message sent via a bot; for bots only @inline_message_id Inline message identifier @reply_markup New message reply markup
editInlineMessageReplyMarkup inline_message_id:string reply_markup:ReplyMarkup = Ok;


//@description Returns all entities (mentions, hashtags, cashtags, bot commands, URLs, and email addresses) contained in the text. This is an offline method. Can be called before authorization. Can be called synchronously @text The text in which to look for entites
getTextEntities text:string = TextEntities;

//@description Parses Bold, Italic, Code, Pre, PreCode and TextUrl entities contained in the text. This is an offline method. Can be called before authorization. Can be called synchronously @text The text which should be parsed @parse_mode Text parse mode
parseTextEntities text:string parse_mode:TextParseMode = FormattedText;

//@description Returns the MIME type of a file, guessed by its extension. Returns an empty string on failure. This is an offline method. Can be called before authorization. Can be called synchronously @file_name The name of the file or path to the file
getFileMimeType file_name:string = Text;

//@description Returns the extension of a file, guessed by its MIME type. Returns an empty string on failure. This is an offline method. Can be called before authorization. Can be called synchronously @mime_type The MIME type of the file
getFileExtension mime_type:string = Text;


//@description Sends an inline query to a bot and returns its results. Returns an error with code 502 if the bot fails to answer the query before the query timeout expires @bot_user_id The identifier of the target bot
//@chat_id Identifier of the chat, where the query was sent @user_location Location of the user, only if needed @query Text of the query @offset Offset of the first entry to return
getInlineQueryResults bot_user_id:int32 chat_id:int53 user_location:location query:string offset:string = InlineQueryResults;

//@description Sets the result of an inline query; for bots only @inline_query_id Identifier of the inline query @is_personal True, if the result of the query can be cached for the specified user
//@results The results of the query @cache_time Allowed time to cache the results of the query, in seconds @next_offset Offset for the next inline query; pass an empty string if there are no more results
//@switch_pm_text If non-empty, this text should be shown on the button that opens a private chat with the bot and sends a start message to the bot with the parameter switch_pm_parameter @switch_pm_parameter The parameter for the bot start message
answerInlineQuery inline_query_id:int64 is_personal:Bool results:vector<InputInlineQueryResult> cache_time:int32 next_offset:string switch_pm_text:string switch_pm_parameter:string = Ok;


//@description Sends a callback query to a bot and returns an answer. Returns an error with code 502 if the bot fails to answer the query before the query timeout expires @chat_id Identifier of the chat with the message @message_id Identifier of the message from which the query originated @payload Query payload
getCallbackQueryAnswer chat_id:int53 message_id:int53 payload:CallbackQueryPayload = CallbackQueryAnswer;

//@description Sets the result of a callback query; for bots only @callback_query_id Identifier of the callback query @text Text of the answer @show_alert If true, an alert should be shown to the user instead of a toast notification @url URL to be opened @cache_time Time during which the result of the query can be cached, in seconds
answerCallbackQuery callback_query_id:int64 text:string show_alert:Bool url:string cache_time:int32 = Ok;


//@description Sets the result of a shipping query; for bots only @shipping_query_id Identifier of the shipping query @shipping_options Available shipping options @error_message An error message, empty on success
answerShippingQuery shipping_query_id:int64 shipping_options:vector<shippingOption> error_message:string = Ok;

//@description Sets the result of a pre-checkout query; for bots only @pre_checkout_query_id Identifier of the pre-checkout query @error_message An error message, empty on success
answerPreCheckoutQuery pre_checkout_query_id:int64 error_message:string = Ok;


//@description Updates the game score of the specified user in the game; for bots only @chat_id The chat to which the message with the game @message_id Identifier of the message @edit_message True, if the message should be edited @user_id User identifier @score The new score
//@force Pass true to update the score even if it decreases. If the score is 0, the user will be deleted from the high score table
setGameScore chat_id:int53 message_id:int53 edit_message:Bool user_id:int32 score:int32 force:Bool = Message;

//@description Updates the game score of the specified user in a game; for bots only @inline_message_id Inline message identifier @edit_message True, if the message should be edited @user_id User identifier @score The new score
//@force Pass true to update the score even if it decreases. If the score is 0, the user will be deleted from the high score table
setInlineGameScore inline_message_id:string edit_message:Bool user_id:int32 score:int32 force:Bool = Ok;

//@description Returns the high scores for a game and some part of the high score table in the range of the specified user; for bots only @chat_id The chat that contains the message with the game @message_id Identifier of the message @user_id User identifier
getGameHighScores chat_id:int53 message_id:int53 user_id:int32 = GameHighScores;

//@description Returns game high scores and some part of the high score table in the range of the specified user; for bots only @inline_message_id Inline message identifier @user_id User identifier
getInlineGameHighScores inline_message_id:string user_id:int32 = GameHighScores;


//@description Deletes the default reply markup from a chat. Must be called after a one-time keyboard or a ForceReply reply markup has been used. UpdateChatReplyMarkup will be sent if the reply markup will be changed @chat_id Chat identifier
//@message_id The message identifier of the used keyboard
deleteChatReplyMarkup chat_id:int53 message_id:int53 = Ok;


//@description Sends a notification about user activity in a chat @chat_id Chat identifier @action The action description
sendChatAction chat_id:int53 action:ChatAction = Ok;


//@description This method should be called if the chat is opened by the user. Many useful activities depend on the chat being opened or closed (e.g., in supergroups and channels all updates are received only for opened chats) @chat_id Chat identifier
openChat chat_id:int53 = Ok;

//@description This method should be called if the chat is closed by the user. Many useful activities depend on the chat being opened or closed @chat_id Chat identifier
closeChat chat_id:int53 = Ok;

//@description This method should be called if messages are being viewed by the user. Many useful activities depend on whether the messages are currently being viewed or not (e.g., marking messages as read, incrementing a view counter, updating a view counter, removing deleted messages in supergroups and channels) @chat_id Chat identifier @message_ids The identifiers of the messages being viewed
//@force_read True, if messages in closed chats should be marked as read
viewMessages chat_id:int53 message_ids:vector<int53> force_read:Bool = Ok;

//@description This method should be called if the message content has been opened (e.g., the user has opened a photo, video, document, location or venue, or has listened to an audio file or voice note message). An updateMessageContentOpened update will be generated if something has changed @chat_id Chat identifier of the message @message_id Identifier of the message with the opened content
openMessageContent chat_id:int53 message_id:int53 = Ok;


//@description Marks all mentions in a chat as read @chat_id Chat identifier
readAllChatMentions chat_id:int53 = Ok;


//@description Returns an existing chat corresponding to a given user @user_id User identifier @force If true, the chat will be created without network request. In this case all information about the chat except its type, title and photo can be incorrect
createPrivateChat user_id:int32 force:Bool = Chat;

//@description Returns an existing chat corresponding to a known basic group @basic_group_id Basic group identifier @force If true, the chat will be created without network request. In this case all information about the chat except its type, title and photo can be incorrect
createBasicGroupChat basic_group_id:int32 force:Bool = Chat;

//@description Returns an existing chat corresponding to a known supergroup or channel @supergroup_id Supergroup or channel identifier @force If true, the chat will be created without network request. In this case all information about the chat except its type, title and photo can be incorrect
createSupergroupChat supergroup_id:int32 force:Bool = Chat;

//@description Returns an existing chat corresponding to a known secret chat @secret_chat_id Secret chat identifier
createSecretChat secret_chat_id:int32 = Chat;

//@description Creates a new basic group and sends a corresponding messageBasicGroupChatCreate. Returns the newly created chat @user_ids Identifiers of users to be added to the basic group @title Title of the new basic group; 1-255 characters
createNewBasicGroupChat user_ids:vector<int32> title:string = Chat;

//@description Creates a new supergroup or channel and sends a corresponding messageSupergroupChatCreate. Returns the newly created chat @title Title of the new chat; 1-255 characters @is_channel True, if a channel chat should be created @param_description Chat description; 0-255 characters
createNewSupergroupChat title:string is_channel:Bool description:string = Chat;

//@description Creates a new secret chat. Returns the newly created chat @user_id Identifier of the target user
createNewSecretChat user_id:int32 = Chat;

//@description Creates a new supergroup from an existing basic group and sends a corresponding messageChatUpgradeTo and messageChatUpgradeFrom. Deactivates the original basic group @chat_id Identifier of the chat to upgrade
upgradeBasicGroupChatToSupergroupChat chat_id:int53 = Chat;


//@description Changes the chat title. Supported only for basic groups, supergroups and channels. Requires administrator rights in basic groups and the appropriate administrator rights in supergroups and channels. The title will not be changed until the request to the server has been completed
//@chat_id Chat identifier @title New title of the chat; 1-255 characters
setChatTitle chat_id:int53 title:string = Ok;

//@description Changes the photo of a chat. Supported only for basic groups, supergroups and channels. Requires administrator rights in basic groups and the appropriate administrator rights in supergroups and channels. The photo will not be changed before request to the server has been completed
//@chat_id Chat identifier @photo New chat photo. You can use a zero InputFileId to delete the chat photo. Files that are accessible only by HTTP URL are not acceptable
setChatPhoto chat_id:int53 photo:InputFile = Ok;

//@description Changes the draft message in a chat @chat_id Chat identifier @draft_message New draft message; may be null
setChatDraftMessage chat_id:int53 draft_message:draftMessage = Ok;

//@description Changes the pinned state of a chat. You can pin up to GetOption("pinned_chat_count_max") non-secret chats and the same number of secret chats @chat_id Chat identifier @is_pinned New value of is_pinned
toggleChatIsPinned chat_id:int53 is_pinned:Bool = Ok;

//@description Changes client data associated with a chat @chat_id Chat identifier @client_data New value of client_data
setChatClientData chat_id:int53 client_data:string = Ok;

//@description Adds a new member to a chat. Members can't be added to private or secret chats. Members will not be added until the chat state has been synchronized with the server
//@chat_id Chat identifier @user_id Identifier of the user @forward_limit The number of earlier messages from the chat to be forwarded to the new member; up to 300. Ignored for supergroups and channels
addChatMember chat_id:int53 user_id:int32 forward_limit:int32 = Ok;

//@description Adds multiple new members to a chat. Currently this option is only available for supergroups and channels. This option can't be used to join a chat. Members can't be added to a channel if it has more than 200 members. Members will not be added until the chat state has been synchronized with the server
//@chat_id Chat identifier @user_ids Identifiers of the users to be added to the chat
addChatMembers chat_id:int53 user_ids:vector<int32> = Ok;

//@description Changes the status of a chat member, needs appropriate privileges. This function is currently not suitable for adding new members to the chat; instead, use addChatMember. The chat member status will not be changed until it has been synchronized with the server
//@chat_id Chat identifier @user_id User identifier @status The new status of the member in the chat
setChatMemberStatus chat_id:int53 user_id:int32 status:ChatMemberStatus = Ok;

//@description Returns information about a single member of a chat @chat_id Chat identifier @user_id User identifier
getChatMember chat_id:int53 user_id:int32 = ChatMember;

//@description Searches for a specified query in the first name, last name and username of the members of a specified chat. Requires administrator rights in channels @chat_id Chat identifier @query Query to search for @limit The maximum number of users to be returned
searchChatMembers chat_id:int53 query:string limit:int32 = ChatMembers;

//@description Returns a list of users who are administrators of the chat @chat_id Chat identifier
getChatAdministrators chat_id:int53 = Users;


//@description Changes the order of pinned chats @chat_ids The new list of pinned chats
setPinnedChats chat_ids:vector<int53> = Ok;


//@description Asynchronously downloads a file from the cloud. updateFile will be used to notify about the download progress and successful completion of the download. Returns file state just after the download has been started
//@file_id Identifier of the file to download
//@priority Priority of the download (1-32). The higher the priority, the earlier the file will be downloaded. If the priorities of two files are equal, then the last one for which downloadFile was called will be downloaded first
downloadFile file_id:int32 priority:int32 = File;

//@description Stops the downloading of a file. If a file has already been downloaded, does nothing @file_id Identifier of a file to stop downloading @only_if_pending Pass true to stop downloading only if it hasn't been started, i.e. request hasn't been sent to server
cancelDownloadFile file_id:int32 only_if_pending:Bool = Ok;

//@description Asynchronously uploads a file to the cloud without sending it in a message. updateFile will be used to notify about upload progress and successful completion of the upload. The file will not have a persistent remote identifier until it will be sent in a message @file File to upload @file_type File type
//@priority Priority of the upload (1-32). The higher the priority, the earlier the file will be uploaded. If the priorities of two files are equal, then the first one for which uploadFile was called will be uploaded first
uploadFile file:InputFile file_type:FileType priority:int32 = File;

//@description Stops the uploading of a file. Supported only for files uploaded by using uploadFile. For other files the behavior is undefined @file_id Identifier of the file to stop uploading
cancelUploadFile file_id:int32 = Ok;

//@description The next part of a file was generated
//@generation_id The identifier of the generation process
//@expected_size Expected size of the generated file, in bytes; 0 if unknown
//@local_prefix_size The number of bytes already generated
setFileGenerationProgress generation_id:int64 expected_size:int32 local_prefix_size:int32 = Ok;

//@description Finishes the file generation
//@generation_id The identifier of the generation process
//@error If set, means that file generation has failed and should be terminated
finishFileGeneration generation_id:int64 error:error = Ok;

//@description Deletes a file from the TDLib file cache @file_id Identifier of the file to delete
deleteFile file_id:int32 = Ok;


//@description Generates a new invite link for a chat; the previously generated link is revoked. Available for basic groups, supergroups, and channels. In basic groups this can be called only by the group's creator; in supergroups and channels this requires appropriate administrator rights @chat_id Chat identifier
generateChatInviteLink chat_id:int53 = ChatInviteLink;

//@description Checks the validity of an invite link for a chat and returns information about the corresponding chat @invite_link Invite link to be checked; should begin with "https://t.me/joinchat/", "https://telegram.me/joinchat/", or "https://telegram.dog/joinchat/"
checkChatInviteLink invite_link:string = ChatInviteLinkInfo;

//@description Uses an invite link to add the current user to the chat if possible. The new member will not be added until the chat state has been synchronized with the server
//@invite_link Invite link to import; should begin with "https://t.me/joinchat/", "https://telegram.me/joinchat/", or "https://telegram.dog/joinchat/"
joinChatByInviteLink invite_link:string = Chat;


//@description Creates a new call @user_id Identifier of the user to be called @protocol Description of the call protocols supported by the client
createCall user_id:int32 protocol:callProtocol = CallId;

//@description Accepts an incoming call @call_id Call identifier @protocol Description of the call protocols supported by the client
acceptCall call_id:int32 protocol:callProtocol = Ok;

//@description Discards a call @call_id Call identifier @is_disconnected True, if the user was disconnected @duration The call duration, in seconds @connection_id Identifier of the connection used during the call
discardCall call_id:int32 is_disconnected:Bool duration:int32 connection_id:int64 = Ok;

//@description Sends a call rating @call_id Call identifier @rating Call rating; 1-5 @comment An optional user comment if the rating is less than 5
sendCallRating call_id:int32 rating:int32 comment:string = Ok;

//@description Sends debug information for a call @call_id Call identifier @debug_information Debug information in application-specific format
sendCallDebugInformation call_id:int32 debug_information:string = Ok;



//@description Adds a user to the blacklist @user_id User identifier
blockUser user_id:int32 = Ok;

//@description Removes a user from the blacklist @user_id User identifier
unblockUser user_id:int32 = Ok;

//@description Returns users that were blocked by the current user @offset Number of users to skip in the result; must be non-negative @limit Maximum number of users to return; up to 100
getBlockedUsers offset:int32 limit:int32 = Users;


//@description Adds new contacts or edits existing contacts; contacts' user identifiers are ignored @contacts The list of contacts to import or edit
importContacts contacts:vector<contact> = ImportedContacts;

//@description Searches for the specified query in the first names, last names and usernames of the known user contacts @query Query to search for; can be empty to return all contacts @limit Maximum number of users to be returned
searchContacts query:string limit:int32 = Users;

//@description Removes users from the contacts list @user_ids Identifiers of users to be deleted
removeContacts user_ids:vector<int32> = Ok;

//@description Returns the total number of imported contacts
getImportedContactCount = Count;

//@description Changes imported contacts using the list of current user contacts saved on the device. Imports newly added contacts and, if at least the file database is enabled, deletes recently deleted contacts.
//-Query result depends on the result of the previous query, so only one query is possible at the same time @contacts The new list of contacts
changeImportedContacts contacts:vector<contact> = ImportedContacts;

//@description Clears all imported contacts
clearImportedContacts = Ok;


//@description Returns the profile photos of a user. The result of this query may be outdated: some photos might have been deleted already @user_id User identifier @offset The number of photos to skip; must be non-negative @limit Maximum number of photos to be returned; up to 100
getUserProfilePhotos user_id:int32 offset:int32 limit:int32 = UserProfilePhotos;


//@description Returns stickers from the installed sticker sets that correspond to a given emoji. If the emoji is not empty, favorite and recently used stickers may also be returned @emoji String representation of emoji. If empty, returns all known installed stickers @limit Maximum number of stickers to be returned
getStickers emoji:string limit:int32 = Stickers;

//@description Searches for stickers from public sticker sets that correspond to a given emoji @emoji String representation of emoji; must be non-empty @limit Maximum number of stickers to be returned
searchStickers emoji:string limit:int32 = Stickers;

//@description Returns a list of installed sticker sets @is_masks Pass true to return mask sticker sets; pass false to return ordinary sticker sets
getInstalledStickerSets is_masks:Bool = StickerSets;

//@description Returns a list of archived sticker sets @is_masks Pass true to return mask stickers sets; pass false to return ordinary sticker sets @offset_sticker_set_id Identifier of the sticker set from which to return the result @limit Maximum number of sticker sets to return
getArchivedStickerSets is_masks:Bool offset_sticker_set_id:int64 limit:int32 = StickerSets;

//@description Returns a list of trending sticker sets
getTrendingStickerSets = StickerSets;

//@description Returns a list of sticker sets attached to a file. Currently only photos and videos can have attached sticker sets @file_id File identifier
getAttachedStickerSets file_id:int32 = StickerSets;

//@description Returns information about a sticker set by its identifier @set_id Identifier of the sticker set
getStickerSet set_id:int64 = StickerSet;

//@description Searches for a sticker set by its name @name Name of the sticker set
searchStickerSet name:string = StickerSet;

//@description Searches for installed sticker sets by looking for specified query in their title and name @is_masks Pass true to return mask sticker sets; pass false to return ordinary sticker sets @query Query to search for @limit Maximum number of sticker sets to return
searchInstalledStickerSets is_masks:Bool query:string limit:int32 = StickerSets;

//@description Searches for ordinary sticker sets by looking for specified query in their title and name. Excludes installed sticker sets from the results @query Query to search for
searchStickerSets query:string = StickerSets;

//@description Installs/uninstalls or activates/archives a sticker set @set_id Identifier of the sticker set @is_installed The new value of is_installed @is_archived The new value of is_archived. A sticker set can't be installed and archived simultaneously
changeStickerSet set_id:int64 is_installed:Bool is_archived:Bool = Ok;

//@description Informs the server that some trending sticker sets have been viewed by the user @sticker_set_ids Identifiers of viewed trending sticker sets
viewTrendingStickerSets sticker_set_ids:vector<int64> = Ok;

//@description Changes the order of installed sticker sets @is_masks Pass true to change the order of mask sticker sets; pass false to change the order of ordinary sticker sets @sticker_set_ids Identifiers of installed sticker sets in the new correct order
reorderInstalledStickerSets is_masks:Bool sticker_set_ids:vector<int64> = Ok;

//@description Returns a list of recently used stickers @is_attached Pass true to return stickers and masks that were recently attached to photos or video files; pass false to return recently sent stickers
getRecentStickers is_attached:Bool = Stickers;

//@description Manually adds a new sticker to the list of recently used stickers. The new sticker is added to the top of the list. If the sticker was already in the list, it is removed from the list first. Only stickers belonging to a sticker set can be added to this list
//@is_attached Pass true to add the sticker to the list of stickers recently attached to photo or video files; pass false to add the sticker to the list of recently sent stickers @sticker Sticker file to add
addRecentSticker is_attached:Bool sticker:InputFile = Stickers;

//@description Removes a sticker from the list of recently used stickers @is_attached Pass true to remove the sticker from the list of stickers recently attached to photo or video files; pass false to remove the sticker from the list of recently sent stickers @sticker Sticker file to delete
removeRecentSticker is_attached:Bool sticker:InputFile = Ok;

//@description Clears the list of recently used stickers @is_attached Pass true to clear the list of stickers recently attached to photo or video files; pass false to clear the list of recently sent stickers
clearRecentStickers is_attached:Bool = Ok;

//@description Returns favorite stickers
getFavoriteStickers = Stickers;

//@description Adds a new sticker to the list of favorite stickers. The new sticker is added to the top of the list. If the sticker was already in the list, it is removed from the list first. Only stickers belonging to a sticker set can be added to this list
//@sticker Sticker file to add
addFavoriteSticker sticker:InputFile = Ok;

//@description Removes a sticker from the list of favorite stickers @sticker Sticker file to delete from the list
removeFavoriteSticker sticker:InputFile = Ok;

//@description Returns emoji corresponding to a sticker @sticker Sticker file identifier
getStickerEmojis sticker:InputFile = StickerEmojis;


//@description Returns saved animations
getSavedAnimations = Animations;

//@description Manually adds a new animation to the list of saved animations. The new animation is added to the beginning of the list. If the animation was already in the list, it is removed first. Only non-secret video animations with MIME type "video/mp4" can be added to the list
//@animation The animation file to be added. Only animations known to the server (i.e. successfully sent via a message) can be added to the list
addSavedAnimation animation:InputFile = Ok;

//@description Removes an animation from the list of saved animations @animation Animation file to be removed
removeSavedAnimation animation:InputFile = Ok;


//@description Returns up to 20 recently used inline bots in the order of their last usage
getRecentInlineBots = Users;


//@description Searches for recently used hashtags by their prefix @prefix Hashtag prefix to search for @limit Maximum number of hashtags to be returned
searchHashtags prefix:string limit:int32 = Hashtags;

//@description Removes a hashtag from the list of recently used hashtags @hashtag Hashtag to delete
removeRecentHashtag hashtag:string = Ok;


//@description Returns a web page preview by the text of the message. Do not call this function too often. Returns a 404 error if the web page has no preview @text Message text with formatting
getWebPagePreview text:formattedText = WebPage;

//@description Returns an instant view version of a web page if available. Returns a 404 error if the web page has no instant view page @url The web page URL @force_full If true, the full instant view for the web page will be returned
getWebPageInstantView url:string force_full:Bool = WebPageInstantView;


//@description Returns the notification settings for a given scope @scope Scope for which to return the notification settings information
getNotificationSettings scope:NotificationSettingsScope = NotificationSettings;

//@description Changes notification settings for a given scope @scope Scope for which to change the notification settings
//@notification_settings The new notification settings for the given scope
setNotificationSettings scope:NotificationSettingsScope notification_settings:notificationSettings = Ok;

//@description Resets all notification settings to their default values. By default, the only muted chats are supergroups, the sound is set to "default" and message previews are shown
resetAllNotificationSettings = Ok;


//@description Uploads a new profile photo for the current user. If something changes, updateUser will be sent @photo Profile photo to set. inputFileId and inputFileRemote may still be unsupported
setProfilePhoto photo:InputFile = Ok;

//@description Deletes a profile photo. If something changes, updateUser will be sent @profile_photo_id Identifier of the profile photo to delete
deleteProfilePhoto profile_photo_id:int64 = Ok;

//@description Changes the first and last name of the current user. If something changes, updateUser will be sent @first_name The new value of the first name for the user; 1-255 characters @last_name The new value of the optional last name for the user; 0-255 characters
setName first_name:string last_name:string = Ok;

//@description Changes the bio of the current user @bio The new value of the user bio; 0-70 characters without line feeds
setBio bio:string = Ok;

//@description Changes the username of the current user. If something changes, updateUser will be sent @username The new value of the username. Use an empty string to remove the username
setUsername username:string = Ok;

//@description Changes the phone number of the user and sends an authentication code to the user's new phone number. On success, returns information about the sent code
//@phone_number The new phone number of the user in international format @allow_flash_call Pass true if the code can be sent via flash call to the specified phone number @is_current_phone_number Pass true if the phone number is used on the current device. Ignored if allow_flash_call is false
changePhoneNumber phone_number:string allow_flash_call:Bool is_current_phone_number:Bool = AuthenticationCodeInfo;

//@description Re-sends the authentication code sent to confirm a new phone number for the user. Works only if the previously received authenticationCodeInfo next_code_type was not null
resendChangePhoneNumberCode = AuthenticationCodeInfo;

//@description Checks the authentication code sent to confirm a new phone number of the user @code Verification code received by SMS, phone call or flash call
checkChangePhoneNumberCode code:string = Ok;


//@description Returns all active sessions of the current user
getActiveSessions = Sessions;

//@description Terminates a session of the current user @session_id Session identifier
terminateSession session_id:int64 = Ok;

//@description Terminates all other sessions of the current user
terminateAllOtherSessions = Ok;


//@description Returns all website where the current user used Telegram to log in
getConnectedWebsites = ConnectedWebsites;

//@description Disconnects website from the current user's Telegram account @website_id Website identifier
disconnectWebsite website_id:int64 = Ok;

//@description Disconnects all websites from the current user's Telegram account
disconnectAllWebsites = Ok;


//@description Toggles the "All members are admins" setting in basic groups; requires creator privileges in the group @basic_group_id Identifier of the basic group @everyone_is_administrator New value of everyone_is_administrator
toggleBasicGroupAdministrators basic_group_id:int32 everyone_is_administrator:Bool = Ok;


//@description Changes the username of a supergroup or channel, requires creator privileges in the supergroup or channel @supergroup_id Identifier of the supergroup or channel @username New value of the username. Use an empty string to remove the username
setSupergroupUsername supergroup_id:int32 username:string = Ok;

//@description Changes the sticker set of a supergroup; requires appropriate rights in the supergroup @supergroup_id Identifier of the supergroup @sticker_set_id New value of the supergroup sticker set identifier. Use 0 to remove the supergroup sticker set
setSupergroupStickerSet supergroup_id:int32 sticker_set_id:int64 = Ok;

//@description Toggles whether all members of a supergroup can add new members; requires appropriate administrator rights in the supergroup. @supergroup_id Identifier of the supergroup @anyone_can_invite New value of anyone_can_invite
toggleSupergroupInvites supergroup_id:int32 anyone_can_invite:Bool = Ok;

//@description Toggles sender signatures messages sent in a channel; requires appropriate administrator rights in the channel. @supergroup_id Identifier of the channel @sign_messages New value of sign_messages
toggleSupergroupSignMessages supergroup_id:int32 sign_messages:Bool = Ok;

//@description Toggles whether the message history of a supergroup is available to new members; requires appropriate administrator rights in the supergroup. @supergroup_id The identifier of the supergroup @is_all_history_available The new value of is_all_history_available
toggleSupergroupIsAllHistoryAvailable supergroup_id:int32 is_all_history_available:Bool = Ok;

//@description Changes information about a supergroup or channel; requires appropriate administrator rights @supergroup_id Identifier of the supergroup or channel @param_description New supergroup or channel description; 0-255 characters
setSupergroupDescription supergroup_id:int32 description:string = Ok;

//@description Pins a message in a supergroup or channel; requires appropriate administrator rights in the supergroup or channel @supergroup_id Identifier of the supergroup or channel @message_id Identifier of the new pinned message @disable_notification True, if there should be no notification about the pinned message
pinSupergroupMessage supergroup_id:int32 message_id:int53 disable_notification:Bool = Ok;

//@description Removes the pinned message from a supergroup or channel; requires appropriate administrator rights in the supergroup or channel @supergroup_id Identifier of the supergroup or channel
unpinSupergroupMessage supergroup_id:int32 = Ok;

//@description Reports some messages from a user in a supergroup as spam @supergroup_id Supergroup identifier @user_id User identifier @message_ids Identifiers of messages sent in the supergroup by the user. This list must be non-empty
reportSupergroupSpam supergroup_id:int32 user_id:int32 message_ids:vector<int53> = Ok;

//@description Returns information about members or banned users in a supergroup or channel. Can be used only if SupergroupFullInfo.can_get_members == true; additionally, administrator privileges may be required for some filters @supergroup_id Identifier of the supergroup or channel
//@filter The type of users to return. By default, supergroupMembersRecent @offset Number of users to skip @limit The maximum number of users be returned; up to 200
getSupergroupMembers supergroup_id:int32 filter:SupergroupMembersFilter offset:int32 limit:int32 = ChatMembers;

//@description Deletes a supergroup or channel along with all messages in the corresponding chat. This will release the supergroup or channel username and remove all members; requires creator privileges in the supergroup or channel. Chats with more than 1000 members can't be deleted using this method @supergroup_id Identifier of the supergroup or channel
deleteSupergroup supergroup_id:int32 = Ok;


//@description Closes a secret chat, effectively transfering its state to secretChatStateClosed @secret_chat_id Secret chat identifier
closeSecretChat secret_chat_id:int32 = Ok;


//@description Returns a list of service actions taken by chat members and administrators in the last 48 hours. Available only in supergroups and channels. Requires administrator rights. Returns results in reverse chronological order (i. e., in order of decreasing event_id)
//@chat_id Chat identifier @query Search query by which to filter events @from_event_id Identifier of an event from which to return results. Use 0 to get results from the latest events @limit Maximum number of events to return; up to 100
//@filters The types of events to return. By default, all types will be returned @user_ids User identifiers by which to filter events. By default, events relating to all users will be returned
getChatEventLog chat_id:int53 query:string from_event_id:int64 limit:int32 filters:chatEventLogFilters user_ids:vector<int32> = ChatEvents;


//@description Returns an invoice payment form. This method should be called when the user presses inlineKeyboardButtonBuy @chat_id Chat identifier of the Invoice message @message_id Message identifier
getPaymentForm chat_id:int53 message_id:int53 = PaymentForm;

//@description Validates the order information provided by a user and returns the available shipping options for a flexible invoice @chat_id Chat identifier of the Invoice message @message_id Message identifier @order_info The order information, provided by the user @allow_save True, if the order information can be saved
validateOrderInfo chat_id:int53 message_id:int53 order_info:orderInfo allow_save:Bool = ValidatedOrderInfo;

//@description Sends a filled-out payment form to the bot for final verification @chat_id Chat identifier of the Invoice message @message_id Message identifier @order_info_id Identifier returned by ValidateOrderInfo, or an empty string @shipping_option_id Identifier of a chosen shipping option, if applicable
//@credentials The credentials chosen by user for payment
sendPaymentForm chat_id:int53 message_id:int53 order_info_id:string shipping_option_id:string credentials:InputCredentials = PaymentResult;

//@description Returns information about a successful payment @chat_id Chat identifier of the PaymentSuccessful message @message_id Message identifier
getPaymentReceipt chat_id:int53 message_id:int53 = PaymentReceipt;

//@description Returns saved order info, if any
getSavedOrderInfo = OrderInfo;

//@description Deletes saved order info
deleteSavedOrderInfo = Ok;

//@description Deletes saved credentials for all payment provider bots
deleteSavedCredentials = Ok;


//@description Returns a user that can be contacted to get support
getSupportUser = User;

//@description Returns background wallpapers
getWallpapers = Wallpapers;


//@description Registers the currently used device for receiving push notifications @device_token Device token @other_user_ids List of at most 100 user identifiers of other users currently using the client
registerDevice device_token:DeviceToken other_user_ids:vector<int32> = Ok;


//@description Returns t.me URLs recently visited by a newly registered user @referrer Google Play referrer to identify the user
getRecentlyVisitedTMeUrls referrer:string = TMeUrls;


//@description Changes user privacy settings @setting The privacy setting @rules The new privacy rules
setUserPrivacySettingRules setting:UserPrivacySetting rules:userPrivacySettingRules = Ok;

//@description Returns the current privacy settings @setting The privacy setting
getUserPrivacySettingRules setting:UserPrivacySetting = UserPrivacySettingRules;


//@description Returns the value of an option by its name. (Check the list of available options on https://core.telegram.org/tdlib/options.) Can be called before authorization
//@name The name of the option
getOption name:string = OptionValue;

//@description Sets the value of an option. (Check the list of available options on https://core.telegram.org/tdlib/options.) Only writable options can be set. Can be called before authorization
//@name The name of the option @value The new value of the option
setOption name:string value:OptionValue = Ok;


//@description Changes the period of inactivity after which the account of the current user will automatically be deleted @ttl New account TTL
setAccountTtl ttl:accountTtl = Ok;

//@description Returns the period of inactivity after which the account of the current user will automatically be deleted
getAccountTtl = AccountTtl;

//@description Deletes the account of the current user, deleting all information associated with the user from the server. The phone number of the account can be used to create a new account @reason The reason why the account was deleted; optional
deleteAccount reason:string = Ok;


//@description Returns information on whether the current chat can be reported as spam @chat_id Chat identifier
getChatReportSpamState chat_id:int53 = ChatReportSpamState;

//@description Used to let the server know whether a chat is spam or not. Can be used only if ChatReportSpamState.can_report_spam is true. After this request, ChatReportSpamState.can_report_spam becomes false forever @chat_id Chat identifier @is_spam_chat If true, the chat will be reported as spam; otherwise it will be marked as not spam
changeChatReportSpamState chat_id:int53 is_spam_chat:Bool = Ok;

//@description Reports a chat to the Telegram moderators. Supported only for supergroups, channels, or private chats with bots, since other chats can't be checked by moderators @chat_id Chat identifier @reason The reason for reporting the chat @message_ids Identifiers of reported messages, if any
reportChat chat_id:int53 reason:ChatReportReason message_ids:vector<int53> = Ok;


//@description Returns storage usage statistics @chat_limit Maximum number of chats with the largest storage usage for which separate statistics should be returned. All other chats will be grouped in entries with chat_id == 0. If the chat info database is not used, the chat_limit is ignored and is always set to 0
getStorageStatistics chat_limit:int32 = StorageStatistics;

//@description Quickly returns approximate storage usage statistics
getStorageStatisticsFast = StorageStatisticsFast;

//@description Optimizes storage usage, i.e. deletes some files and returns new storage usage statistics. Secret thumbnails can't be deleted
//@size Limit on the total size of files after deletion. Pass -1 to use the default limit
//@ttl Limit on the time that has passed since the last time a file was accessed (or creation time for some filesystems). Pass -1 to use the default limit
//@count Limit on the total count of files after deletion. Pass -1 to use the default limit
//@immunity_delay The amount of time after the creation of a file during which it can't be deleted, in seconds. Pass -1 to use the default value
//@file_types If not empty, only files with the given type(s) are considered. By default, all types except thumbnails, profile photos, stickers and wallpapers are deleted
//@chat_ids If not empty, only files from the given chats are considered. Use 0 as chat identifier to delete files not belonging to any chat (e.g., profile photos)
//@exclude_chat_ids If not empty, files from the given chats are excluded. Use 0 as chat identifier to exclude all files not belonging to any chat (e.g., profile photos)
//@chat_limit Same as in getStorageStatistics. Affects only returned statistics
optimizeStorage size:int53 ttl:int32 count:int32 immunity_delay:int32 file_types:vector<FileType> chat_ids:vector<int53> exclude_chat_ids:vector<int53> chat_limit:int32 = StorageStatistics;


//@description Sets the current network type. Can be called before authorization. Calling this method forces all network connections to reopen, mitigating the delay in switching between different networks, so it should be called whenever the network is changed, even if the network type remains the same.
//-Network type is used to check whether the library can use the network at all and also for collecting detailed network data usage statistics @type The new network type. By default, networkTypeOther
setNetworkType type:NetworkType = Ok;

//@description Returns network data usage statistics. Can be called before authorization @only_current If true, returns only data for the current library launch
getNetworkStatistics only_current:Bool = NetworkStatistics;

//@description Adds the specified data to data usage statistics. Can be called before authorization @entry The network statistics entry with the data to be added to statistics
addNetworkStatistics entry:NetworkStatisticsEntry = Ok;

//@description Resets all network data usage statistics to zero. Can be called before authorization
resetNetworkStatistics = Ok;


//@description Informs the server about the number of pending bot updates if they haven't been processed for a long time; for bots only @pending_update_count The number of pending updates @error_message The last error message
setBotUpdatesStatus pending_update_count:int32 error_message:string = Ok;


//@description Uploads a PNG image with a sticker; for bots only; returns the uploaded file @user_id Sticker file owner @png_sticker PNG image with the sticker; must be up to 512 kB in size and fit in 512x512 square
uploadStickerFile user_id:int32 png_sticker:InputFile = File;

//@description Creates a new sticker set; for bots only. Returns the newly created sticker set @user_id Sticker set owner @title Sticker set title; 1-64 characters @name Sticker set name. Can contain only English letters, digits and underscores. Must end with *"_by_<bot username>"* (*<bot_username>* is case insensitive); 1-64 characters
//@is_masks True, if stickers are masks @stickers List of stickers to be added to the set
createNewStickerSet user_id:int32 title:string name:string is_masks:Bool stickers:vector<inputSticker> = StickerSet;

//@description Adds a new sticker to a set; for bots only. Returns the sticker set @user_id Sticker set owner @name Sticker set name @sticker Sticker to add to the set
addStickerToSet user_id:int32 name:string sticker:inputSticker = StickerSet;

//@description Changes the position of a sticker in the set to which it belongs; for bots only. The sticker set must have been created by the bot @sticker Sticker @position New position of the sticker in the set, zero-based
setStickerPositionInSet sticker:InputFile position:int32 = Ok;

//@description Removes a sticker from the set to which it belongs; for bots only. The sticker set must have been created by the bot @sticker Sticker
removeStickerFromSet sticker:InputFile = Ok;


//@description Sends a custom request; for bots only @method The method name @parameters JSON-serialized method parameters
sendCustomRequest method:string parameters:string = CustomRequestResult;

//@description Answers a custom query; for bots only @custom_query_id Identifier of a custom query @data JSON-serialized answer to the query
answerCustomQuery custom_query_id:int64 data:string = Ok;


//@description Succeeds after a specified amount of time has passed. Can be called before authorization @seconds Number of seconds before the function returns
setAlarm seconds:double = Ok;


//@description Uses current user IP to found his country. Returns two-letter ISO 3166-1 alpha-2 country code. Can be called before authorization
getCountryCode = Text;

//@description Returns the default text for invitation messages to be used as a placeholder when the current user invites friends to Telegram
getInviteText = Text;

//@description Returns the terms of service. Can be called before authorization
getTermsOfService = Text;


//@description Sets the proxy server for network requests. Can be called before authorization @proxy Proxy server to use. Specify null to remove the proxy server
setProxy proxy:Proxy = Ok;

//@description Returns the proxy that is currently set up. Can be called before authorization
getProxy = Proxy;


//@description Does nothing; for testing only
testCallEmpty = Ok;
//@description Returns the received string; for testing only @x String to return
testCallString x:string = TestString;
//@description Returns the received bytes; for testing only @x Bytes to return
testCallBytes x:bytes = TestBytes;
//@description Returns the received vector of numbers; for testing only @x Vector of numbers to return
testCallVectorInt x:vector<int32> = TestVectorInt;
//@description Returns the received vector of objects containing a number; for testing only @x Vector of objects to return
testCallVectorIntObject x:vector<testInt> = TestVectorIntObject;
//@description For testing only request. Returns the received vector of strings; for testing only @x Vector of strings to return
testCallVectorString x:vector<string> = TestVectorString;
//@description Returns the received vector of objects containing a string; for testing only @x Vector of objects to return
testCallVectorStringObject x:vector<testString> = TestVectorStringObject;
//@description Returns the squared received number; for testing only @x Number to square
testSquareInt x:int32 = TestInt;
//@description Sends a simple network request to the Telegram servers; for testing only
testNetwork = Ok;
//@description Forces an updates.getDifference call to the Telegram servers; for testing only
testGetDifference = Ok;
//@description Does nothing and ensures that the Update object is used; for testing only
testUseUpdate = Update;
//@description Does nothing and ensures that the Error object is used; for testing only
testUseError = Error;
