//@description An object of this type can be returned on every function call, in case of an error
//@code Error code; subject to future changes. If the error code is 406, the error message must not be processed in any way and must not be displayed to the user
//@message Error message; subject to future changes
error code:int32 message:string = Error;


//@description An object of this type is returned on a successful function call for certain functions
ok = Ok;


//@description Contains parameters for TDLib initialization
//@use_test_dc If set to true, the Telegram test environment will be used instead of the production environment
//@database_directory The path to the directory for the persistent database; if empty, the current working directory will be used
//@files_directory The path to the directory for storing files; if empty, database_directory will be used
//@use_file_database If set to true, information about downloaded and uploaded files will be saved between application restarts
//@use_chat_info_database If set to true, the library will maintain a cache of users, basic groups, supergroups, channels and secret chats. Implies use_file_database
//@use_message_database If set to true, the library will maintain a cache of chats and messages. Implies use_chat_info_database
//@use_secret_chats If set to true, support for secret chats will be enabled
//@api_id Application identifier for Telegram API access, which can be obtained at https://my.telegram.org
//@api_hash Application identifier hash for Telegram API access, which can be obtained at https://my.telegram.org
//@system_language_code IETF language tag of the user's operating system language; must be non-empty
//@device_model Model of the device the application is being run on; must be non-empty
//@system_version Version of the operating system the application is being run on; must be non-empty
//@application_version Application version; must be non-empty
//@enable_storage_optimizer If set to true, old files will automatically be deleted
//@ignore_file_names If set to true, original file names will be ignored. Otherwise, downloaded files will be saved under names as close as possible to the original name
tdlibParameters use_test_dc:Bool database_directory:string files_directory:string use_file_database:Bool use_chat_info_database:Bool use_message_database:Bool use_secret_chats:Bool api_id:int32 api_hash:string system_language_code:string device_model:string system_version:string application_version:string enable_storage_optimizer:Bool ignore_file_names:Bool = TdlibParameters;


//@class AuthenticationCodeType @description Provides information about the method by which an authentication code is delivered to the user

//@description An authentication code is delivered via a private Telegram message, which can be viewed in another client @length Length of the code
authenticationCodeTypeTelegramMessage length:int32 = AuthenticationCodeType;

//@description An authentication code is delivered via an SMS message to the specified phone number @length Length of the code
authenticationCodeTypeSms length:int32 = AuthenticationCodeType;

//@description An authentication code is delivered via a phone call to the specified phone number @length Length of the code
authenticationCodeTypeCall length:int32 = AuthenticationCodeType;

//@description An authentication code is delivered by an immediately cancelled call to the specified phone number. The number from which the call was made is the code @pattern Pattern of the phone number from which the call will be made
authenticationCodeTypeFlashCall pattern:string = AuthenticationCodeType;


//@description Information about the authentication code that was sent @phone_number A phone number that is being authenticated @type Describes the way the code was sent to the user @next_type Describes the way the next code will be sent to the user; may be null @timeout Timeout before the code should be re-sent, in seconds
authenticationCodeInfo phone_number:string type:AuthenticationCodeType next_type:AuthenticationCodeType timeout:int32 = AuthenticationCodeInfo;


//@class AuthorizationState @description Represents the current authorization state of the client

//@description TDLib needs TdlibParameters for initialization
authorizationStateWaitTdlibParameters = AuthorizationState;

//@description TDLib needs an encryption key to decrypt the local database @is_encrypted True, if the database is currently encrypted
authorizationStateWaitEncryptionKey is_encrypted:Bool = AuthorizationState;

//@description TDLib needs the user's phone number to authorize
authorizationStateWaitPhoneNumber = AuthorizationState;

//@description TDLib needs the user's authentication code to finalize authorization @is_registered True, if the user is already registered @code_info Information about the authorization code that was sent
authorizationStateWaitCode is_registered:Bool code_info:authenticationCodeInfo = AuthorizationState;

//@description The user has been authorized, but needs to enter a password to start using the application @password_hint Hint for the password; can be empty @has_recovery_email_address True if a recovery email address has been set up
//@recovery_email_address_pattern Pattern of the email address to which the recovery email was sent; empty until a recovery email has been sent
authorizationStateWaitPassword password_hint:string has_recovery_email_address:Bool recovery_email_address_pattern:string = AuthorizationState;

//@description The user has been successfully authorized. TDLib is now ready to answer queries
authorizationStateReady = AuthorizationState;

//@description The user is currently logging out
authorizationStateLoggingOut = AuthorizationState;

//@description TDLib is closing, all subsequent queries will be answered with the error 500. Note that closing TDLib can take a while. All resources will be freed only after authorizationStateClosed has been received
authorizationStateClosing = AuthorizationState;

//@description TDLib client is in its final state. All databases are closed and all resources are released. No other updates will be received after this. All queries will be responded to
//-with error code 500. To continue working, one should create a new instance of the TDLib client
authorizationStateClosed = AuthorizationState;


//@description Represents the current state of 2-step verification @has_password True if a 2-step verification password has been set up @password_hint Hint for the password; can be empty @has_recovery_email_address True if a recovery email has been set up @unconfirmed_recovery_email_address_pattern Pattern of the email address to which a confirmation email was sent
passwordState has_password:Bool password_hint:string has_recovery_email_address:Bool unconfirmed_recovery_email_address_pattern:string = PasswordState;

//@description Contains information available to the user after requesting password recovery @recovery_email_address_pattern Pattern of the email address to which a recovery email was sent
passwordRecoveryInfo recovery_email_address_pattern:string = PasswordRecoveryInfo;

//@description Contains information about the current recovery email address @recovery_email_address Recovery email address
recoveryEmailAddress recovery_email_address:string = RecoveryEmailAddress;


//@description Returns information about the availability of a temporary password, which can be used for payments @has_password True, if a temporary password is available @valid_for Time left before the temporary password expires, in seconds
temporaryPasswordState has_password:Bool valid_for:int32 = TemporaryPasswordState;


//@description Represents a local file
//@path Local path to the locally available file part; may be empty
//@can_be_downloaded True, if it is possible to try to download or generate the file
//@can_be_deleted True, if the file can be deleted
//@is_downloading_active True, if the file is currently being downloaded (or a local copy is being generated by some other means)
//@is_downloading_completed True, if the local copy is fully available
//@downloaded_prefix_size If is_downloading_completed is false, then only some prefix of the file is ready to be read. downloaded_prefix_size is the size of that prefix
//@downloaded_size Total downloaded file bytes. Should be used only for calculating download progress. The actual file size may be bigger, and some parts of it may contain garbage
localFile path:string can_be_downloaded:Bool can_be_deleted:Bool is_downloading_active:Bool is_downloading_completed:Bool downloaded_prefix_size:int32 downloaded_size:int32 = LocalFile;

//@description Represents a remote file
//@id Remote file identifier, may be empty. Can be used across application restarts or even from other devices for the current user. If the ID starts with "http://" or "https://", it represents the HTTP URL of the file. TDLib is currently unable to download files if only their URL is known.
//-If downloadFile is called on such a file or if it is sent to a secret chat, TDLib starts a file generation process by sending updateFileGenerationStart to the client with the HTTP URL in the original_path and "#url#" as the conversion string. Clients should generate the file by downloading it to the specified location
//@is_uploading_active True, if the file is currently being uploaded (or a remote copy is being generated by some other means)
//@is_uploading_completed True, if a remote copy is fully available
//@uploaded_size Size of the remote available part of the file; 0 if unknown
remoteFile id:string is_uploading_active:Bool is_uploading_completed:Bool uploaded_size:int32 = RemoteFile;

//@description Represents a file
//@id Unique file identifier
//@size File size; 0 if unknown
//@expected_size Expected file size in case the exact file size is unknown, but an approximate size is known. Can be used to show download/upload progress
//@local Information about the local copy of the file
//@remote Information about the remote copy of the file
file id:int32 size:int32 expected_size:int32 local:localFile remote:remoteFile = File;


//@class InputFile @description Points to a file

//@description A file defined by its unique ID @id Unique file identifier
inputFileId id:int32 = InputFile;

//@description A file defined by its remote ID @id Remote file identifier
inputFileRemote id:string = InputFile;

//@description A file defined by a local path @path Local path to the file
inputFileLocal path:string = InputFile;

//@description A file generated by the client @original_path Local path to a file from which the file is generated, may be empty if there is no such file @conversion String specifying the conversion applied to the original file; should be persistent across application restarts @expected_size Expected size of the generated file; 0 if unknown
inputFileGenerated original_path:string conversion:string expected_size:int32 = InputFile;


//@description Photo description @type Thumbnail type (see https://core.telegram.org/constructor/photoSize) @photo Information about the photo file @width Photo width @height Photo height
photoSize type:string photo:file width:int32 height:int32 = PhotoSize;


//@class MaskPoint @description Part of the face, relative to which a mask should be placed

//@description A mask should be placed relatively to the forehead
maskPointForehead = MaskPoint;

//@description A mask should be placed relatively to the eyes
maskPointEyes = MaskPoint;

//@description A mask should be placed relatively to the mouth
maskPointMouth = MaskPoint;

//@description A mask should be placed relatively to the chin
maskPointChin = MaskPoint;

//@description Position on a photo where a mask should be placed @point Part of the face, relative to which the mask should be placed
//@x_shift Shift by X-axis measured in widths of the mask scaled to the face size, from left to right. (For example, -1.0 will place the mask just to the left of the default mask position)
//@y_shift Shift by Y-axis measured in heights of the mask scaled to the face size, from top to bottom. (For example, 1.0 will place the mask just below the default mask position)
//@scale Mask scaling coefficient. (For example, 2.0 means a doubled size)
maskPosition point:MaskPoint x_shift:double y_shift:double scale:double = MaskPosition;


//@description Represents a part of the text that needs to be formatted in some unusual way @offset Offset of the entity in UTF-16 code points @length Length of the entity, in UTF-16 code points @type Type of the entity
textEntity offset:int32 length:int32 type:TextEntityType = TextEntity;

//@description Contains a list of text entities @entities List of text entities
textEntities entities:vector<textEntity> = TextEntities;

//@description A text with some entities @text The text @entities Entities contained in the text
formattedText text:string entities:vector<textEntity> = FormattedText;


//@description Describes an animation file. The animation must be encoded in GIF or MPEG4 format @duration Duration of the animation, in seconds; as defined by the sender @width Width of the animation @height Height of the animation
//@file_name Original name of the file; as defined by the sender @mime_type MIME type of the file, usually "image/gif" or "video/mp4" @thumbnail Animation thumbnail; may be null @animation File containing the animation
animation duration:int32 width:int32 height:int32 file_name:string mime_type:string thumbnail:photoSize animation:file = Animation;

//@description Describes an audio file. Audio is usually in MP3 format @duration Duration of the audio, in seconds; as defined by the sender @title Title of the audio; as defined by the sender @performer Performer of the audio; as defined by the sender
//@file_name Original name of the file; as defined by the sender @mime_type The MIME type of the file; as defined by the sender @album_cover_thumbnail The thumbnail of the album cover; as defined by the sender. The full size thumbnail should be extracted from the downloaded file; may be null @audio File containing the audio
audio duration:int32 title:string performer:string file_name:string mime_type:string album_cover_thumbnail:photoSize audio:file = Audio;

//@description Describes a document of any type @file_name Original name of the file; as defined by the sender @mime_type MIME type of the file; as defined by the sender
//@thumbnail Document thumbnail; as defined by the sender; may be null @document File containing the document
document file_name:string mime_type:string thumbnail:photoSize document:file = Document;

//@description Describes a photo @id Photo identifier; 0 for deleted photos @has_stickers True, if stickers were added to the photo @sizes Available variants of the photo, in different sizes
photo id:int64 has_stickers:Bool sizes:vector<photoSize> = Photo;

//@description Describes a sticker @set_id The identifier of the sticker set to which the sticker belongs; 0 if none @width Sticker width; as defined by the sender @height Sticker height; as defined by the sender
//@emoji Emoji corresponding to the sticker @is_mask True, if the sticker is a mask @mask_position Position where the mask should be placed; may be null @thumbnail Sticker thumbnail in WEBP or JPEG format; may be null @sticker File containing the sticker
sticker set_id:int64 width:int32 height:int32 emoji:string is_mask:Bool mask_position:maskPosition thumbnail:photoSize sticker:file = Sticker;

//@description Describes a video file @duration Duration of the video, in seconds; as defined by the sender @width Video width; as defined by the sender @height Video height; as defined by the sender
//@file_name Original name of the file; as defined by the sender @mime_type MIME type of the file; as defined by the sender @has_stickers True, if stickers were added to the photo
//@supports_streaming True, if the video should be tried to be streamed @thumbnail Video thumbnail; as defined by the sender; may be null @video File containing the video
video duration:int32 width:int32 height:int32 file_name:string mime_type:string has_stickers:Bool supports_streaming:Bool thumbnail:photoSize video:file = Video;

//@description Describes a video note. The video must be equal in width and height, cropped to a circle, and stored in MPEG4 format @duration Duration of the video, in seconds; as defined by the sender @length Video width and height; as defined by the sender @thumbnail Video thumbnail; as defined by the sender; may be null @video File containing the video
videoNote duration:int32 length:int32 thumbnail:photoSize video:file = VideoNote;

//@description Describes a voice note. The voice note must be encoded with the Opus codec, and stored inside an OGG container. Voice notes can have only a single audio channel @duration Duration of the voice note, in seconds; as defined by the sender
//@waveform A waveform representation of the voice note in 5-bit format @mime_type MIME type of the file; as defined by the sender @voice File containing the voice note
voiceNote duration:int32 waveform:bytes mime_type:string voice:file = VoiceNote;

//@description Describes a user contact @phone_number Phone number of the user @first_name First name of the user; 1-255 characters in length @last_name Last name of the user @user_id Identifier of the user, if known; otherwise 0
contact phone_number:string first_name:string last_name:string user_id:int32 = Contact;

//@description Describes a location on planet Earth @latitude Latitude of the location in degrees; as defined by the sender @longitude Longitude of the location, in degrees; as defined by the sender
location latitude:double longitude:double = Location;

//@description Describes a venue @location Venue location; as defined by the sender @title Venue name; as defined by the sender @address Venue address; as defined by the sender @provider Provider of the venue database; as defined by the sender. Currently only "foursquare" needs to be supported
//@id Identifier of the venue in the provider database; as defined by the sender
venue location:location title:string address:string provider:string id:string = Venue;

//@description Describes a game @id Game ID @short_name Game short name. To share a game use the URL https://t.me/{bot_username}?game={game_short_name} @title Game title @text Game text, usually containing scoreboards for a game
//@param_description Game description @photo Game photo @animation Game animation; may be null
game id:int64 short_name:string title:string text:formattedText description:string photo:photo animation:animation = Game;


//@description Describes a user profile photo @id Photo identifier; 0 for an empty photo. Can be used to find a photo in a list of userProfilePhotos
//@small A small (160x160) user profile photo @big A big (640x640) user profile photo
profilePhoto id:int64 small:file big:file = ProfilePhoto;

//@description Describes the photo of a chat @small A small (160x160) chat photo @big A big (640x640) chat photo
chatPhoto small:file big:file = ChatPhoto;


//@class LinkState @description Represents the relationship between user A and user B. For incoming_link, user A is the current user; for outgoing_link, user B is the current user

//@description The phone number of user A is not known to user B
linkStateNone = LinkState;

//@description The phone number of user A is known but that number has not been saved to the contacts list of user B
linkStateKnowsPhoneNumber = LinkState;

//@description The phone number of user A has been saved to the contacts list of user B
linkStateIsContact = LinkState;


//@class UserType @description Represents the type of the user. The following types are possible: regular users, deleted users and bots

//@description A regular user
userTypeRegular = UserType;

//@description A deleted user or deleted bot. No information on the user besides the user_id is available. It is not possible to perform any active actions on this type of user
userTypeDeleted = UserType;

//@description A bot (see https://core.telegram.org/bots) @can_join_groups True, if the bot can be invited to basic group and supergroup chats
//@can_read_all_group_messages True, if the bot can read all messages in basic group or supergroup chats and not just those addressed to the bot. In private and channel chats a bot can always read all messages
//@is_inline True, if the bot supports inline queries @inline_query_placeholder Placeholder for inline queries (displayed on the client input field) @need_location True, if the location of the user should be sent with every inline query to this bot
userTypeBot can_join_groups:Bool can_read_all_group_messages:Bool is_inline:Bool inline_query_placeholder:string need_location:Bool = UserType;

//@description No information on the user besides the user_id is available, yet this user has not been deleted. This object is extremely rare and must be handled like a deleted user. It is not possible to perform any actions on users of this type
userTypeUnknown = UserType;


//@description Represents commands supported by a bot @command Text of the bot command @param_description Description of the bot command
botCommand command:string description:string = BotCommand;

//@description Provides information about a bot and its supported commands @param_description Long description shown on the user info page @commands A list of commands supported by the bot
botInfo description:string commands:vector<botCommand> = BotInfo;


//@description Represents a user @id User identifier @first_name First name of the user @last_name Last name of the user @username Username of the user
//@phone_number Phone number of the user @status Current online status of the user @profile_photo Profile photo of the user; may be null
//@outgoing_link Relationship from the current user to the other user @incoming_link Relationship from the other user to the current user @is_verified True, if the user is verified @restriction_reason If non-empty, it contains the reason why access to this user must be restricted. The format of the string is "{type}: {description}".
//-{type} contains the type of the restriction and at least one of the suffixes "-all", "-ios", "-android", or "-wp", which describe the platforms on which access should be restricted. (For example, "terms-ios-android". {description} contains a human-readable description of the restriction, which can be shown to the user)
//@have_access If false, the user is inaccessible, and the only information known about the user is inside this class. It can't be passed to any method except GetUser @type Type of the user @language_code IETF language tag of the user's language; only available to bots
user id:int32 first_name:string last_name:string username:string phone_number:string status:UserStatus profile_photo:profilePhoto outgoing_link:LinkState incoming_link:LinkState is_verified:Bool restriction_reason:string have_access:Bool type:UserType language_code:string = User;

//@description Contains full information about a user (except the full list of profile photos) @is_blocked True, if the user is blacklisted by the current user @can_be_called True, if the user can be called @has_private_calls True, if the user can't be called due to their privacy settings
//@bio A short user bio @share_text For bots, the text that is included with the link when users share the bot @group_in_common_count Number of group chats where both the other user and the current user are a member; 0 for the current user @bot_info If the user is a bot, information about the bot; may be null
userFullInfo is_blocked:Bool can_be_called:Bool has_private_calls:Bool bio:string share_text:string group_in_common_count:int32 bot_info:botInfo = UserFullInfo;

//@description Contains part of the list of user photos @total_count Total number of user profile photos @photos A list of photos
userProfilePhotos total_count:int32 photos:vector<photo> = UserProfilePhotos;

//@description Represents a list of users @total_count Approximate total count of users found @user_ids A list of user identifiers
users total_count:int32 user_ids:vector<int32> = Users;


//@class ChatMemberStatus @description Provides information about the status of a member in a chat

//@description The user is the creator of a chat and has all the administrator privileges @is_member True, if the user is a member of the chat
chatMemberStatusCreator is_member:Bool = ChatMemberStatus;

//@description The user is a member of a chat and has some additional privileges. In basic groups, administrators can edit and delete messages sent by others, add new members, and ban unprivileged members. In supergroups and channels, there are more detailed options for administrator privileges
//@can_be_edited True, if the current user can edit the administrator privileges for the called user
//@can_change_info True, if the administrator can change the chat title, photo, and other settings
//@can_post_messages True, if the administrator can create channel posts; applicable to channels only
//@can_edit_messages True, if the administrator can edit messages of other users and pin messages; applicable to channels only
//@can_delete_messages True, if the administrator can delete messages of other users
//@can_invite_users True, if the administrator can invite new users to the chat
//@can_restrict_members True, if the administrator can restrict, ban, or unban chat members
//@can_pin_messages True, if the administrator can pin messages; applicable to supergroups only
//@can_promote_members True, if the administrator can add new administrators with a subset of his own privileges or demote administrators that were directly or indirectly promoted by him
chatMemberStatusAdministrator can_be_edited:Bool can_change_info:Bool can_post_messages:Bool can_edit_messages:Bool can_delete_messages:Bool can_invite_users:Bool can_restrict_members:Bool can_pin_messages:Bool can_promote_members:Bool = ChatMemberStatus;

//@description The user is a member of a chat, without any additional privileges or restrictions
chatMemberStatusMember = ChatMemberStatus;

//@description The user is under certain restrictions in the chat. Not supported in basic groups and channels
//@is_member True, if the user is a member of the chat
//@restricted_until_date Point in time (Unix timestamp) when restrictions will be lifted from the user; 0 if never. If the user is restricted for more than 366 days or for less than 30 seconds from the current time, the user is considered to be restricted forever
//@can_send_messages True, if the user can send text messages, contacts, locations, and venues
//@can_send_media_messages True, if the user can send audio files, documents, photos, videos, video notes, and voice notes. Implies can_send_messages permissions
//@can_send_other_messages True, if the user can send animations, games, and stickers and use inline bots. Implies can_send_media_messages permissions
//@can_add_web_page_previews True, if the user may add a web page preview to his messages. Implies can_send_messages permissions
chatMemberStatusRestricted is_member:Bool restricted_until_date:int32 can_send_messages:Bool can_send_media_messages:Bool can_send_other_messages:Bool can_add_web_page_previews:Bool = ChatMemberStatus;

//@description The user is not a chat member
chatMemberStatusLeft = ChatMemberStatus;

//@description The user was banned (and hence is not a member of the chat). Implies the user can't return to the chat or view messages
//@banned_until_date Point in time (Unix timestamp) when the user will be unbanned; 0 if never. If the user is banned for more than 366 days or for less than 30 seconds from the current time, the user is considered to be banned forever
chatMemberStatusBanned banned_until_date:int32 = ChatMemberStatus;


//@description A user with information about joining/leaving a chat @user_id User identifier of the chat member @inviter_user_id Identifier of a user that invited/promoted/banned this member in the chat; 0 if unknown
//@joined_chat_date Point in time (Unix timestamp) when the user joined a chat @status Status of the member in the chat @bot_info If the user is a bot, information about the bot; may be null. Can be null even for a bot if the bot is not a chat member
chatMember user_id:int32 inviter_user_id:int32 joined_chat_date:int32 status:ChatMemberStatus bot_info:botInfo = ChatMember;

//@description Contains a list of chat members @total_count Approximate total count of chat members found @members A list of chat members
chatMembers total_count:int32 members:vector<chatMember> = ChatMembers;


//@class SupergroupMembersFilter @description Specifies the kind of chat members to return in getSupergroupMembers

//@description Returns recently active users in reverse chronological order
supergroupMembersFilterRecent = SupergroupMembersFilter;

//@description Returns the creator and administrators
supergroupMembersFilterAdministrators = SupergroupMembersFilter;

//@description Used to search for supergroup or channel members via a (string) query @query Query to search for
supergroupMembersFilterSearch query:string = SupergroupMembersFilter;

//@description Returns restricted supergroup members; can be used only by administrators @query Query to search for
supergroupMembersFilterRestricted query:string = SupergroupMembersFilter;

//@description Returns users banned from the supergroup or channel; can be used only by administrators @query Query to search for
supergroupMembersFilterBanned query:string = SupergroupMembersFilter;

//@description Returns bot members of the supergroup or channel
supergroupMembersFilterBots = SupergroupMembersFilter;


//@description Represents a basic group of 0-200 users (must be upgraded to a supergroup to accommodate more than 200 users)
//@id Group identifier
//@member_count Number of members in the group
//@status Status of the current user in the group
//@everyone_is_administrator True, if all members have been granted administrator rights in the group
//@is_active True, if the group is active
//@upgraded_to_supergroup_id Identifier of the supergroup to which this group was upgraded; 0 if none
basicGroup id:int32 member_count:int32 status:ChatMemberStatus everyone_is_administrator:Bool is_active:Bool upgraded_to_supergroup_id:int32 = BasicGroup;

//@description Contains full information about a basic group @creator_user_id User identifier of the creator of the group; 0 if unknown @members Group members @invite_link Invite link for this group; available only for the group creator and only after it has been generated at least once
basicGroupFullInfo creator_user_id:int32 members:vector<chatMember> invite_link:string = BasicGroupFullInfo;


//@description Represents a supergroup or channel with zero or more members (subscribers in the case of channels). From the point of view of the system, a channel is a special kind of a supergroup: only administrators can post and see the list of members, and posts from all administrators use the name and photo of the channel instead of individual names and profile photos. Unlike supergroups, channels can have an unlimited number of subscribers
//@id Supergroup or channel identifier
//@username Username of the supergroup or channel; empty for private supergroups or channels
//@date Point in time (Unix timestamp) when the current user joined, or the point in time when the supergroup or channel was created, in case the user is not a member
//@status Status of the current user in the supergroup or channel
//@member_count Member count; 0 if unknown. Currently it is guaranteed to be known only if the supergroup or channel was found through SearchPublicChats
//@anyone_can_invite True, if any member of the supergroup can invite other members. This field has no meaning for channels
//@sign_messages True, if messages sent to the channel should contain information about the sender. This field is only applicable to channels
//@is_channel True, if the supergroup is a channel
//@is_verified True, if the supergroup or channel is verified
//@restriction_reason If non-empty, contains the reason why access to this supergroup or channel must be restricted. Format of the string is "{type}: {description}".
//-{type} Contains the type of the restriction and at least one of the suffixes "-all", "-ios", "-android", or "-wp", which describe the platforms on which access should be restricted. (For example, "terms-ios-android". {description} contains a human-readable description of the restriction, which can be shown to the user)
supergroup id:int32 username:string date:int32 status:ChatMemberStatus member_count:int32 anyone_can_invite:Bool sign_messages:Bool is_channel:Bool is_verified:Bool restriction_reason:string = Supergroup;

//@description Contains full information about a supergroup or channel
//@param_description Supergroup or channel description
//@member_count Number of members in the supergroup or channel; 0 if unknown
//@administrator_count Number of privileged users in the supergroup or channel; 0 if unknown
//@restricted_count Number of restricted users in the supergroup; 0 if unknown
//@banned_count Number of users banned from chat; 0 if unknown
//@can_get_members True, if members of the chat can be retrieved
//@can_set_username True, if the chat can be made public
//@can_set_sticker_set True, if the supergroup sticker set can be changed
//@is_all_history_available True, if new chat members will have access to old messages. In public supergroups and both public and private channels, old messages are always available, so this option affects only private supergroups. The value of this field is only available for chat administrators
//@sticker_set_id Identifier of the supergroup sticker set; 0 if none
//@invite_link Invite link for this chat
//@pinned_message_id Identifier of the pinned message in the chat; 0 if none
//@upgraded_from_basic_group_id Identifier of the basic group from which supergroup was upgraded; 0 if none
//@upgraded_from_max_message_id Identifier of the last message in the basic group from which supergroup was upgraded; 0 if none
supergroupFullInfo description:string member_count:int32 administrator_count:int32 restricted_count:int32 banned_count:int32 can_get_members:Bool can_set_username:Bool can_set_sticker_set:Bool is_all_history_available:Bool sticker_set_id:int64 invite_link:string pinned_message_id:int53 upgraded_from_basic_group_id:int32 upgraded_from_max_message_id:int53 = SupergroupFullInfo;


//@class SecretChatState @description Describes the current secret chat state

//@description The secret chat is not yet created; waiting for the other user to get online
secretChatStatePending = SecretChatState;

//@description The secret chat is ready to use
secretChatStateReady = SecretChatState;

//@description The secret chat is closed
secretChatStateClosed = SecretChatState;


//@description Represents a secret chat
//@id Secret chat identifier
//@user_id Identifier of the chat partner
//@state State of the secret chat
//@is_outbound True, if the chat was created by the current user; otherwise false
//@ttl Current message Time To Live setting (self-destruct timer) for the chat, in seconds
//@key_hash Hash of the currently used key for comparison with the hash of the chat partner's key. This is a string of 36 bytes, which must be used to make a 12x12 square image with a color depth of 4. The first 16 bytes should be used to make a central 8x8 square, while the remaining 20 bytes should be used to construct a 2-pixel-wide border around that square.
//-Alternatively, the first 32 bytes of the hash can be converted to the hexadecimal format and printed as 32 2-digit hex numbers
//@layer Secret chat layer; determines features supported by the other client. Video notes are supported if the layer >= 66
secretChat id:int32 user_id:int32 state:SecretChatState is_outbound:Bool ttl:int32 key_hash:bytes layer:int32 = SecretChat;


//@class MessageForwardInfo @description Contains information about the initial sender of a forwarded message

//@description The message was originally written by a known user @sender_user_id Identifier of the user that originally sent this message @date Point in time (Unix timestamp) when the message was originally sent
//@forwarded_from_chat_id For messages forwarded to the chat with the current user (saved messages), the identifier of the chat from which the message was forwarded; 0 if unknown
//@forwarded_from_message_id For messages forwarded to the chat with the current user (saved messages) the identifier of the original message from which the new message was forwarded; 0 if unknown
messageForwardedFromUser sender_user_id:int32 date:int32 forwarded_from_chat_id:int53 forwarded_from_message_id:int53 = MessageForwardInfo;

//@description The message was originally a post in a channel @chat_id Identifier of the chat from which the message was forwarded @author_signature Post author signature
//@date Point in time (Unix timestamp) when the message was originally sent @message_id Message identifier of the original message from which the new message was forwarded; 0 if unknown
//@forwarded_from_chat_id For messages forwarded to the chat with the current user (saved messages), the identifier of the chat from which the message was forwarded; 0 if unknown
//@forwarded_from_message_id For messages forwarded to the chat with the current user (saved messages), the identifier of the original message from which the new message was forwarded; 0 if unknown
messageForwardedPost chat_id:int53 author_signature:string date:int32 message_id:int53 forwarded_from_chat_id:int53 forwarded_from_message_id:int53 = MessageForwardInfo;


//@class MessageSendingState @description Contains information about the sending state of the message

//@description The message is being sent now, but has not yet been delivered to the server
messageSendingStatePending = MessageSendingState;

//@description The message failed to be sent
messageSendingStateFailed = MessageSendingState;


//@description Describes a message
//@id Unique message identifier
//@sender_user_id Identifier of the user who sent the message; 0 if unknown. It is unknown for channel posts
//@chat_id Chat identifier
//@sending_state Information about the sending state of the message; may be null
//@is_outgoing True, if the message is outgoing
//@can_be_edited True, if the message can be edited
//@can_be_forwarded True, if the message can be forwarded
//@can_be_deleted_only_for_self True, if the message can be deleted only for the current user while other users will continue to see it
//@can_be_deleted_for_all_users True, if the message can be deleted for all users
//@is_channel_post True, if the message is a channel post. All messages to channels are channel posts, all other messages are not channel posts
//@contains_unread_mention True, if the message contains an unread mention for the current user
//@date Point in time (Unix timestamp) when the message was sent
//@edit_date Point in time (Unix timestamp) when the message was last edited
//@forward_info Information about the initial message sender; may be null
//@reply_to_message_id If non-zero, the identifier of the message this message is replying to; can be the identifier of a deleted message
//@ttl For self-destructing messages, the message's TTL (Time To Live), in seconds; 0 if none. TDLib will send updateDeleteMessages or updateMessageContent once the TTL expires
//@ttl_expires_in Time left before the message expires, in seconds
//@via_bot_user_id If non-zero, the user identifier of the bot through which this message was sent
//@author_signature For channel posts, optional author signature
//@views Number of times this message was viewed
//@media_album_id Unique identifier of an album this message belongs to. Only photos and videos can be grouped together in albums
//@content Content of the message
//@reply_markup Reply markup for the message; may be null
message id:int53 sender_user_id:int32 chat_id:int53 sending_state:MessageSendingState is_outgoing:Bool can_be_edited:Bool can_be_forwarded:Bool can_be_deleted_only_for_self:Bool can_be_deleted_for_all_users:Bool is_channel_post:Bool contains_unread_mention:Bool date:int32 edit_date:int32 forward_info:MessageForwardInfo reply_to_message_id:int53 ttl:int32 ttl_expires_in:double via_bot_user_id:int32 author_signature:string views:int32 media_album_id:int64 content:MessageContent reply_markup:ReplyMarkup = Message;

//@description Contains a list of messages @total_count Approximate total count of messages found @messages List of messages; messages may be null
messages total_count:int32 messages:vector<message> = Messages;

//@description Contains a list of messages found by a search @messages List of messages @next_from_search_id Value to pass as from_search_id to get more results
foundMessages messages:vector<message> next_from_search_id:int64 = FoundMessages;


//@class NotificationSettingsScope @description Describes the types of chats for which notification settings are applied

//@description Notification settings applied to a particular chat @chat_id Chat identifier
notificationSettingsScopeChat chat_id:int53 = NotificationSettingsScope;

//@description Notification settings applied to all private chats
notificationSettingsScopePrivateChats = NotificationSettingsScope;

//@description Notification settings applied to all basic groups and channels. (Supergroups have no common settings)
notificationSettingsScopeBasicGroupChats = NotificationSettingsScope;

//@description Notification settings applied to all chats
notificationSettingsScopeAllChats = NotificationSettingsScope;


//@description Contains information about notification settings for a chat or several chats @mute_for Time left before notifications will be unmuted, in seconds @sound An audio file name for notification sounds; only applies to iOS applications @show_preview True, if message content should be displayed in notifications
notificationSettings mute_for:int32 sound:string show_preview:Bool = NotificationSettings;


//@description Contains information about a message draft @reply_to_message_id Identifier of the message to reply to; 0 if none @input_message_text Content of the message draft; this should always be of type inputMessageText
draftMessage reply_to_message_id:int53 input_message_text:InputMessageContent = DraftMessage;


//@class ChatType @description Describes the type of a chat

//@description An ordinary chat with a user @user_id User identifier
chatTypePrivate user_id:int32 = ChatType;

//@description A basic group (i.e., a chat with 0-200 other users) @basic_group_id Basic group identifier
chatTypeBasicGroup basic_group_id:int32 = ChatType;

//@description A supergroup (i.e. a chat with up to GetOption("supergroup_max_size") other users), or channel (with unlimited members) @supergroup_id Supergroup or channel identifier @is_channel True, if the supergroup is a channel
chatTypeSupergroup supergroup_id:int32 is_channel:Bool = ChatType;

//@description A secret chat with a user @secret_chat_id Secret chat identifier @user_id User identifier of the secret chat peer
chatTypeSecret secret_chat_id:int32 user_id:int32 = ChatType;


//@description A chat. (Can be a private chat, basic group, supergroup, or secret chat)
//@id Chat unique identifier
//@type Type of the chat
//@title Chat title
//@photo Chat photo; may be null
//@last_message Last message in the chat; may be null
//@order Descending parameter by which chats are sorted in the main chat list. If the order number of two chats is the same, they must be sorted in descending order by ID. If 0, the position of the chat in the list is undetermined
//@is_pinned True, if the chat is pinned
//@can_be_reported True, if the chat can be reported to Telegram moderators through reportChat
//@unread_count Number of unread messages in the chat
//@last_read_inbox_message_id Identifier of the last read incoming message
//@last_read_outbox_message_id Identifier of the last read outgoing message
//@unread_mention_count Number of unread messages with a mention/reply in the chat
//@notification_settings Notification settings for this chat
//@reply_markup_message_id Identifier of the message from which reply markup needs to be used; 0 if there is no default custom reply markup in the chat
//@draft_message A draft of a message in the chat; may be null
//@client_data Contains client-specific data associated with the chat. (For example, the chat position or local chat notification settings can be stored here.) Persistent if a message database is used
chat id:int53 type:ChatType title:string photo:chatPhoto last_message:message order:int64 is_pinned:Bool can_be_reported:Bool unread_count:int32 last_read_inbox_message_id:int53 last_read_outbox_message_id:int53 unread_mention_count:int32 notification_settings:notificationSettings reply_markup_message_id:int53 draft_message:draftMessage client_data:string = Chat;

//@description Represents a list of chats @chat_ids List of chat identifiers
chats chat_ids:vector<int53> = Chats;


//@description Contains a chat invite link @invite_link Chat invite link
chatInviteLink invite_link:string = ChatInviteLink;

//@description Contains information about a chat invite link
//@chat_id Chat identifier of the invite link; 0 if the user is not a member of this chat
//@type Contains information about the type of the chat
//@title Title of the chat
//@photo Chat photo; may be null
//@member_count Number of members
//@member_user_ids User identifiers of some chat members that may be known to the current user
//@is_public True, if the chat is a public supergroup or channel with a username
chatInviteLinkInfo chat_id:int53 type:ChatType title:string photo:chatPhoto member_count:int32 member_user_ids:vector<int32> is_public:Bool = ChatInviteLinkInfo;


//@class KeyboardButtonType @description Describes a keyboard button type

//@description A simple button, with text that should be sent when the button is pressed
keyboardButtonTypeText = KeyboardButtonType;

//@description A button that sends the user's phone number when pressed; available only in private chats
keyboardButtonTypeRequestPhoneNumber = KeyboardButtonType;

//@description A button that sends the user's location when pressed; available only in private chats
keyboardButtonTypeRequestLocation = KeyboardButtonType;


//@description Represents a single button in a bot keyboard @text Text of the button @type Type of the button
keyboardButton text:string type:KeyboardButtonType = KeyboardButton;


//@class InlineKeyboardButtonType @description Describes the type of an inline keyboard button

//@description A button that opens a specified URL @url URL to open
inlineKeyboardButtonTypeUrl url:string = InlineKeyboardButtonType;

//@description A button that sends a special callback query to a bot @data Data to be sent to the bot via a callback query
inlineKeyboardButtonTypeCallback data:bytes = InlineKeyboardButtonType;

//@description A button with a game that sends a special callback query to a bot. This button must be in the first column and row of the keyboard and can be attached only to a message with content of the type messageGame
inlineKeyboardButtonTypeCallbackGame = InlineKeyboardButtonType;

//@description A button that forces an inline query to the bot to be inserted in the input field @query Inline query to be sent to the bot @in_current_chat True, if the inline query should be sent from the current chat
inlineKeyboardButtonTypeSwitchInline query:string in_current_chat:Bool = InlineKeyboardButtonType;

//@description A button to buy something. This button must be in the first column and row of the keyboard and can be attached only to a message with content of the type messageInvoice
inlineKeyboardButtonTypeBuy = InlineKeyboardButtonType;


//@description Represents a single button in an inline keyboard @text Text of the button @type Type of the button
inlineKeyboardButton text:string type:InlineKeyboardButtonType = InlineKeyboardButton;


//@class ReplyMarkup @description Contains a description of a custom keyboard and actions that can be done with it to quickly reply to bots

//@description Instructs clients to remove the keyboard once this message has been received. This kind of keyboard can't be received in an incoming message; instead, UpdateChatReplyMarkup with message_id == 0 will be sent
//@is_personal True, if the keyboard is removed only for the mentioned users or the target user of a reply
replyMarkupRemoveKeyboard is_personal:Bool = ReplyMarkup;

//@description Instructs clients to force a reply to this message
//@is_personal True, if a forced reply must automatically be shown to the current user. For outgoing messages, specify true to show the forced reply only for the mentioned users and for the target user of a reply
replyMarkupForceReply is_personal:Bool = ReplyMarkup;

//@description Contains a custom keyboard layout to quickly reply to bots
//@rows A list of rows of bot keyboard buttons
//@resize_keyboard True, if the client needs to resize the keyboard vertically
//@one_time True, if the client needs to hide the keyboard after use
//@is_personal True, if the keyboard must automatically be shown to the current user. For outgoing messages, specify true to show the keyboard only for the mentioned users and for the target user of a reply
replyMarkupShowKeyboard rows:vector<vector<keyboardButton>> resize_keyboard:Bool one_time:Bool is_personal:Bool = ReplyMarkup;

//@description Contains an inline keyboard layout
//@rows A list of rows of inline keyboard buttons
replyMarkupInlineKeyboard rows:vector<vector<inlineKeyboardButton>> = ReplyMarkup;


//@class RichText @description Describes a text object inside an instant-view web page

//@description A plain text @text Text
richTextPlain text:string = RichText;

//@description A bold rich text @text Text
richTextBold text:RichText = RichText;

//@description An italicized rich text @text Text
richTextItalic text:RichText = RichText;

//@description An underlined rich text @text Text
richTextUnderline text:RichText = RichText;

//@description A strike-through rich text @text Text
richTextStrikethrough text:RichText = RichText;

//@description A fixed-width rich text @text Text
richTextFixed text:RichText = RichText;

//@description A rich text URL link @text Text @url URL
richTextUrl text:RichText url:string = RichText;

//@description A rich text email link @text Text @email_address Email address
richTextEmailAddress text:RichText email_address:string = RichText;

//@description A concatenation of rich texts @texts Texts
richTexts texts:vector<RichText> = RichText;


//@class PageBlock @description Describes a block of an instant view web page

//@description The title of a page @title Title
pageBlockTitle title:RichText = PageBlock;

//@description The subtitle of a page @subtitle Subtitle
pageBlockSubtitle subtitle:RichText = PageBlock;

//@description The author and publishing date of a page @author Author @publish_date Point in time (Unix timestamp) when the article was published; 0 if unknown
pageBlockAuthorDate author:RichText publish_date:int32 = PageBlock;

//@description A header @header Header
pageBlockHeader header:RichText = PageBlock;

//@description A subheader @subheader Subheader
pageBlockSubheader subheader:RichText = PageBlock;

//@description A text paragraph @text Paragraph text
pageBlockParagraph text:RichText = PageBlock;

//@description A preformatted text paragraph @text Paragraph text @language Programming language for which the text should be formatted
pageBlockPreformatted text:RichText language:string = PageBlock;

//@description The footer of a page @footer Footer
pageBlockFooter footer:RichText = PageBlock;

//@description An empty block separating a page
pageBlockDivider = PageBlock;

//@description An invisible anchor on a page, which can be used in a URL to open the page from the specified anchor @name Name of the anchor
pageBlockAnchor name:string = PageBlock;

//@description A list of texts @items Texts @is_ordered True, if the items should be marked with numbers
pageBlockList items:vector<RichText> is_ordered:Bool = PageBlock;

//@description A block quote @text Quote text @caption Quote caption
pageBlockBlockQuote text:RichText caption:RichText = PageBlock;

//@description A pull quote @text Quote text @caption Quote caption
pageBlockPullQuote text:RichText caption:RichText = PageBlock;

//@description An animation @animation Animation file; may be null @caption Animation caption @need_autoplay True, if the animation should be played automatically
pageBlockAnimation animation:animation caption:RichText need_autoplay:Bool = PageBlock;

//@description An audio file @audio Audio file; may be null @caption Audio file caption
pageBlockAudio audio:audio caption:RichText = PageBlock;

//@description A photo @photo Photo file; may be null @caption Photo caption
pageBlockPhoto photo:photo caption:RichText = PageBlock;

//@description A video @video Video file; may be null @caption Video caption @need_autoplay True, if the video should be played automatically @is_looped True, if the video should be looped
pageBlockVideo video:video caption:RichText need_autoplay:Bool is_looped:Bool = PageBlock;

//@description A page cover @cover Cover
pageBlockCover cover:PageBlock = PageBlock;

//@description An embedded web page @url Web page URL, if available @html HTML-markup of the embedded page @poster_photo Poster photo, if available; may be null @width Block width @height Block height @caption Block caption @is_full_width True, if the block should be full width @allow_scrolling True, if scrolling should be allowed
pageBlockEmbedded url:string html:string poster_photo:photo width:int32 height:int32 caption:RichText is_full_width:Bool allow_scrolling:Bool = PageBlock;

//@description An embedded post @url Web page URL @author Post author @author_photo Post author photo @date Point in time (Unix timestamp) when the post was created; 0 if unknown @page_blocks Post content @caption Post caption
pageBlockEmbeddedPost url:string author:string author_photo:photo date:int32 page_blocks:vector<PageBlock> caption:RichText = PageBlock;

//@description A collage @page_blocks Collage item contents @caption Block caption
pageBlockCollage page_blocks:vector<PageBlock> caption:RichText = PageBlock;

//@description A slideshow @page_blocks Slideshow item contents @caption Block caption
pageBlockSlideshow page_blocks:vector<PageBlock> caption:RichText = PageBlock;

//@description A link to a chat @title Chat title @photo Chat photo; may be null @username Chat username, by which all other information about the chat should be resolved
pageBlockChatLink title:string photo:chatPhoto username:string = PageBlock;


//@description Describes an instant view page for a web page @page_blocks Content of the web page @is_full True, if the instant view contains the full page. A network request might be needed to get the full web page instant view
webPageInstantView page_blocks:vector<PageBlock> is_full:Bool = WebPageInstantView;


//@description Describes a web page preview @url Original URL of the link @display_url URL to display
//@type Type of the web page. Can be: article, photo, audio, video, document, profile, app, or something else
//@site_name Short name of the site (e.g., Google Docs, App Store) @title Title of the content @param_description Description of the content
//@photo Image representing the content; may be null
//@embed_url URL to show in the embedded preview
//@embed_type MIME type of the embedded preview, (e.g., text/html or video/mp4)
//@embed_width Width of the embedded preview
//@embed_height Height of the embedded preview
//@duration Duration of the content, in seconds
//@author Author of the content
//@animation Preview of the content as an animation, if available; may be null
//@audio Preview of the content as an audio file, if available; may be null
//@document Preview of the content as a document, if available (currently only available for small PDF files and ZIP archives); may be null
//@sticker Preview of the content as a sticker for small WEBP files, if available; may be null
//@video Preview of the content as a video, if available; may be null
//@video_note Preview of the content as a video note, if available; may be null
//@voice_note Preview of the content as a voice note, if available; may be null
//@has_instant_view True, if the web page has an instant view
webPage url:string display_url:string type:string site_name:string title:string description:string photo:photo embed_url:string embed_type:string embed_width:int32 embed_height:int32 duration:int32 author:string animation:animation audio:audio document:document sticker:sticker video:video video_note:videoNote voice_note:voiceNote has_instant_view:Bool = WebPage;


//@description Portion of the price of a product (e.g., "delivery cost", "tax amount") @label Label for this portion of the product price @amount Currency amount in minimal quantity of the currency
labeledPricePart label:string amount:int53 = LabeledPricePart;

//@description Product invoice @currency ISO 4217 currency code @price_parts A list of objects used to calculate the total price of the product @is_test True, if the payment is a test payment
//@need_name True, if the user's name is needed for payment @need_phone_number True, if the user's phone number is needed for payment @need_email_address True, if the user's email address is needed for payment
//@need_shipping_address True, if the user's shipping address is needed for payment @send_phone_number_to_provider True, if the user's phone number will be sent to the provider
//@send_email_address_to_provider True, if the user's email address will be sent to the provider @is_flexible True, if the total price depends on the shipping method
invoice currency:string price_parts:vector<labeledPricePart> is_test:Bool need_name:Bool need_phone_number:Bool need_email_address:Bool need_shipping_address:Bool send_phone_number_to_provider:Bool send_email_address_to_provider:Bool is_flexible:Bool = Invoice;

//@description Describes a shipping address @country_code Two-letter ISO 3166-1 alpha-2 country code @state State, if applicable @city City @street_line1 First line of the address @street_line2 Second line of the address @postal_code Address postal code
shippingAddress country_code:string state:string city:string street_line1:string street_line2:string postal_code:string = ShippingAddress;

//@description Order information @name Name of the user @phone_number Phone number of the user @email_address Email address of the user @shipping_address Shipping address for this order; may be null
orderInfo name:string phone_number:string email_address:string shipping_address:shippingAddress = OrderInfo;

//@description One shipping option @id Shipping option identifier @title Option title @price_parts A list of objects used to calculate the total shipping costs
shippingOption id:string title:string price_parts:vector<labeledPricePart> = ShippingOption;

//@description Contains information about saved card credentials @id Unique identifier of the saved credentials @title Title of the saved credentials
savedCredentials id:string title:string = SavedCredentials;

//@class InputCredentials @description Contains information about the payment method chosen by the user

//@description Applies if a user chooses some previously saved payment credentials. To use their previously saved credentials, the user must have a valid temporary password @saved_credentials_id Identifier of the saved credentials
inputCredentialsSaved saved_credentials_id:string = InputCredentials;

//@description Applies if a user enters new credentials on a payment provider website @data Contains JSON-encoded data with a credential identifier from the payment provider @allow_save True, if the credential identifier can be saved on the server side
inputCredentialsNew data:string allow_save:Bool = InputCredentials;

//@description Applies if a user enters new credentials using Android Pay @data JSON-encoded data with the credential identifier
inputCredentialsAndroidPay data:string = InputCredentials;

//@description Applies if a user enters new credentials using Apple Pay @data JSON-encoded data with the credential identifier
inputCredentialsApplePay data:string = InputCredentials;

//@description Stripe payment provider @publishable_key Stripe API publishable key @need_country True, if the user country must be provided @need_postal_code True, if the user ZIP/postal code must be provided @need_cardholder_name True, if the cardholder name must be provided
paymentsProviderStripe publishable_key:string need_country:Bool need_postal_code:Bool need_cardholder_name:Bool = PaymentsProviderStripe;

//@description Contains information about an invoice payment form @invoice Full information of the invoice @url Payment form URL @payments_provider Contains information about the payment provider, if available, to support it natively without the need for opening the URL; may be null
//@saved_order_info Saved server-side order information; may be null @saved_credentials Contains information about saved card credentials; may be null @can_save_credentials True, if the user can choose to save credentials @need_password True, if the user will be able to save credentials protected by a password they set up
paymentForm invoice:invoice url:string payments_provider:paymentsProviderStripe saved_order_info:orderInfo saved_credentials:savedCredentials can_save_credentials:Bool need_password:Bool = PaymentForm;

//@description Contains a temporary identifier of validated order information, which is stored for one hour. Also contains the available shipping options @order_info_id Temporary identifier of the order information @shipping_options Available shipping options
validatedOrderInfo order_info_id:string shipping_options:vector<shippingOption> = ValidatedOrderInfo;

//@description Contains the result of a payment request @success True, if the payment request was successful; otherwise the verification_url will be not empty @verification_url URL for additional payment credentials verification
paymentResult success:Bool verification_url:string = PaymentResult;

//@description Contains information about a successful payment @date Point in time (Unix timestamp) when the payment was made @payments_provider_user_id User identifier of the payment provider bot @invoice Contains information about the invoice
//@order_info Contains order information; may be null @shipping_option Chosen shipping option; may be null @credentials_title Title of the saved credentials
paymentReceipt date:int32 payments_provider_user_id:int32 invoice:invoice order_info:orderInfo shipping_option:shippingOption credentials_title:string = PaymentReceipt;


//@class MessageContent @description Contains the content of a message

//@description A text message @text Text of the message @web_page A preview of the web page that's mentioned in the text; may be null
messageText text:formattedText web_page:webPage = MessageContent;

//@description An animation message (GIF-style). @animation Message content @caption Animation caption @is_secret True, if the animation thumbnail must be blurred and the animation must be shown only while tapped
messageAnimation animation:animation caption:formattedText is_secret:Bool = MessageContent;

//@description An audio message @audio Message content @caption Audio caption
messageAudio audio:audio caption:formattedText = MessageContent;

//@description A document message (general file) @document Message content @caption Document caption
messageDocument document:document caption:formattedText = MessageContent;

//@description A photo message @photo Message content @caption Photo caption @is_secret True, if the photo must be blurred and must be shown only while tapped
messagePhoto photo:photo caption:formattedText is_secret:Bool = MessageContent;

//@description An expired photo message (self-destructed after TTL has elapsed)
messageExpiredPhoto = MessageContent;

//@description A sticker message @sticker Message content
messageSticker sticker:sticker = MessageContent;

//@description A video message @video Message content @caption Video caption @is_secret True, if the video thumbnail must be blurred and the video must be shown only while tapped
messageVideo video:video caption:formattedText is_secret:Bool = MessageContent;

//@description An expired video message (self-destructed after TTL has elapsed)
messageExpiredVideo = MessageContent;

//@description A video note message @video_note Message content @is_viewed True, if at least one of the recipients has viewed the video note @is_secret True, if the video note thumbnail must be blurred and the video note must be shown only while tapped
messageVideoNote video_note:videoNote is_viewed:Bool is_secret:Bool = MessageContent;

//@description A voice note message @voice_note Message content @caption Voice note caption @is_listened True, if at least one of the recipients has listened to the voice note
messageVoiceNote voice_note:voiceNote caption:formattedText is_listened:Bool = MessageContent;

//@description A message with a location @location Message content @live_period Time relative to the message sent date until which the location can be updated, in seconds
//@expires_in Left time for which the location can be updated, in seconds. updateMessageContent is not sent when this field changes
messageLocation location:location live_period:int32 expires_in:int32 = MessageContent;

//@description A message with information about a venue @venue Message content
messageVenue venue:venue = MessageContent;

//@description A message with a user contact @contact Message content
messageContact contact:contact = MessageContent;

//@description A message with a game @game Game
messageGame game:game = MessageContent;

//@description A message with an invoice from a bot @title Product title @param_description Product description @photo Product photo; may be null @currency Currency for the product price @total_amount Product total price in the minimal quantity of the currency
//@start_parameter Unique invoice bot start_parameter. To share an invoice use the URL https://t.me/{bot_username}?start={start_parameter} @is_test True, if the invoice is a test invoice
//@need_shipping_address True, if the shipping address should be specified @receipt_message_id The identifier of the message with the receipt, after the product has been purchased
messageInvoice title:string description:string photo:photo currency:string total_amount:int53 start_parameter:string is_test:Bool need_shipping_address:Bool receipt_message_id:int53 = MessageContent;

//@description A message with information about an ended call @discard_reason Reason why the call was discarded @duration Call duration, in seconds
messageCall discard_reason:CallDiscardReason duration:int32 = MessageContent;

//@description A newly created basic group @title Title of the basic group @member_user_ids User identifiers of members in the basic group
messageBasicGroupChatCreate title:string member_user_ids:vector<int32> = MessageContent;

//@description A newly created supergroup or channel @title Title of the supergroup or channel
messageSupergroupChatCreate title:string = MessageContent;

//@description An updated chat title @title New chat title
messageChatChangeTitle title:string = MessageContent;

//@description An updated chat photo @photo New chat photo
messageChatChangePhoto photo:photo = MessageContent;

//@description A deleted chat photo
messageChatDeletePhoto = MessageContent;

//@description New chat members were added @member_user_ids User identifiers of the new members
messageChatAddMembers member_user_ids:vector<int32> = MessageContent;

//@description A new member joined the chat by invite link
messageChatJoinByLink = MessageContent;

//@description A chat member was deleted @user_id User identifier of the deleted chat member
messageChatDeleteMember user_id:int32 = MessageContent;

//@description A basic group was upgraded to a supergroup and was deactivated as the result @supergroup_id Identifier of the supergroup to which the basic group was upgraded
messageChatUpgradeTo supergroup_id:int32 = MessageContent;

//@description A supergroup has been created from a basic group @title Title of the newly created supergroup @basic_group_id The identifier of the original basic group
messageChatUpgradeFrom title:string basic_group_id:int32 = MessageContent;

//@description A message has been pinned @message_id Identifier of the pinned message, can be an identifier of a deleted message
messagePinMessage message_id:int53 = MessageContent;

//@description A screenshot of a message in the chat has been taken
messageScreenshotTaken = MessageContent;

//@description The TTL (Time To Live) setting messages in a secret chat has been changed @ttl New TTL
messageChatSetTtl ttl:int32 = MessageContent;

//@description A non-standard action has happened in the chat @text Message text to be shown in the chat
messageCustomServiceAction text:string = MessageContent;

//@description A new high score was achieved in a game @game_message_id Identifier of the message with the game, can be an identifier of a deleted message @game_id Identifier of the game, may be different from the games presented in the message with the game @score New score
messageGameScore game_message_id:int53 game_id:int64 score:int32 = MessageContent;

//@description A payment has been completed @invoice_message_id Identifier of the message with the corresponding invoice; can be an identifier of a deleted message @currency Currency for the price of the product @total_amount Total price for the product, in the minimal quantity of the currency
messagePaymentSuccessful invoice_message_id:int53 currency:string total_amount:int53 = MessageContent;

//@description A payment has been completed; for bots only @invoice_message_id Identifier of the message with the corresponding invoice; can be an identifier of a deleted message @currency Currency for price of the product
//@total_amount Total price for the product, in the minimal quantity of the currency @invoice_payload Invoice payload @shipping_option_id Identifier of the shipping option chosen by the user, may be empty if not applicable @order_info Information about the order; may be null
//@telegram_payment_charge_id Telegram payment identifier @provider_payment_charge_id Provider payment identifier
messagePaymentSuccessfulBot invoice_message_id:int53 currency:string total_amount:int53 invoice_payload:bytes shipping_option_id:string order_info:orderInfo telegram_payment_charge_id:string provider_payment_charge_id:string = MessageContent;

//@description A contact has registered with Telegram
messageContactRegistered = MessageContent;

//@description The current user has connected a website by logging in using Telegram Login Widget on it @domain_name Domain name of the connected website
messageWebsiteConnected domain_name:string = MessageContent;

//@description Message content that is not supported by the client
messageUnsupported = MessageContent;


//@class TextEntityType @description Represents a part of the text which must be formatted differently

//@description A mention of a user by their username
textEntityTypeMention = TextEntityType;

//@description A hashtag text, beginning with "#"
textEntityTypeHashtag = TextEntityType;

//@description A cashtag text, beginning with "$" and consisting of capital english letters (i.e. "$USD")
textEntityTypeCashtag = TextEntityType;

//@description A bot command, beginning with "/". This shouldn't be highlighted if there are no bots in the chat
textEntityTypeBotCommand = TextEntityType;

//@description An HTTP URL
textEntityTypeUrl = TextEntityType;

//@description An email address
textEntityTypeEmailAddress = TextEntityType;

//@description A bold text
textEntityTypeBold = TextEntityType;

//@description An italic text
textEntityTypeItalic = TextEntityType;

//@description Text that must be formatted as if inside a code HTML tag
textEntityTypeCode = TextEntityType;

//@description Text that must be formatted as if inside a pre HTML tag
textEntityTypePre = TextEntityType;

//@description Text that must be formatted as if inside pre, and code HTML tags @language Programming language of the code; as defined by the sender
textEntityTypePreCode language:string = TextEntityType;

//@description A text description shown instead of a raw URL @url URL to be opened when the link is clicked
textEntityTypeTextUrl url:string = TextEntityType;

//@description A text shows instead of a raw mention of the user (e.g., when the user has no username) @user_id Identifier of the mentioned user
textEntityTypeMentionName user_id:int32 = TextEntityType;

//@description A phone number
textEntityTypePhoneNumber = TextEntityType;


//@description A thumbnail to be sent along with a file; should be in JPEG or WEBP format for stickers, and less than 200 kB in size @thumbnail Thumbnail file to send. Sending thumbnails by file_id is currently not supported
//@width Thumbnail width, usually shouldn't exceed 90. Use 0 if unknown @height Thumbnail height, usually shouldn't exceed 90. Use 0 if unknown
inputThumbnail thumbnail:InputFile width:int32 height:int32 = InputThumbnail;


//@class InputMessageContent @description The content of a message to send

//@description A text message @text Formatted text to be sent. Only Bold, Italic, Code, Pre, PreCode and TextUrl entities are allowed to be specified manually
//@disable_web_page_preview True, if rich web page previews for URLs in the message text should be disabled @clear_draft True, if a chat message draft should be deleted
inputMessageText text:formattedText disable_web_page_preview:Bool clear_draft:Bool = InputMessageContent;

//@description An animation message (GIF-style). @animation Animation file to be sent @thumbnail Animation thumbnail, if available @duration Duration of the animation, in seconds @width Width of the animation; may be replaced by the server @height Height of the animation; may be replaced by the server @caption Animation caption; 0-200 characters
inputMessageAnimation animation:InputFile thumbnail:inputThumbnail duration:int32 width:int32 height:int32 caption:formattedText = InputMessageContent;

//@description An audio message @audio Audio file to be sent @album_cover_thumbnail Thumbnail of the cover for the album, if available @duration Duration of the audio, in seconds; may be replaced by the server @title Title of the audio; 0-64 characters; may be replaced by the server
//@performer Performer of the audio; 0-64 characters, may be replaced by the server @caption Audio caption; 0-200 characters
inputMessageAudio audio:InputFile album_cover_thumbnail:inputThumbnail duration:int32 title:string performer:string caption:formattedText = InputMessageContent;

//@description A document message (general file) @document Document to be sent @thumbnail Document thumbnail, if available @caption Document caption; 0-200 characters
inputMessageDocument document:InputFile thumbnail:inputThumbnail caption:formattedText = InputMessageContent;

//@description A photo message @photo Photo to send @thumbnail Photo thumbnail to be sent, this is sent to the other party in secret chats only @added_sticker_file_ids File identifiers of the stickers added to the photo, if applicable @width Photo width @height Photo height @caption Photo caption; 0-200 characters
//@ttl Photo TTL (Time To Live), in seconds (0-60). A non-zero TTL can be specified only in private chats
inputMessagePhoto photo:InputFile thumbnail:inputThumbnail added_sticker_file_ids:vector<int32> width:int32 height:int32 caption:formattedText ttl:int32 = InputMessageContent;

//@description A sticker message @sticker Sticker to be sent @thumbnail Sticker thumbnail, if available @width Sticker width @height Sticker height
inputMessageSticker sticker:InputFile thumbnail:inputThumbnail width:int32 height:int32 = InputMessageContent;

//@description A video message @video Video to be sent @thumbnail Video thumbnail, if available @added_sticker_file_ids File identifiers of the stickers added to the video, if applicable
//@duration Duration of the video, in seconds @width Video width @height Video height @supports_streaming True, if the video should be tried to be streamed
//@caption Video caption; 0-200 characters @ttl Video TTL (Time To Live), in seconds (0-60). A non-zero TTL can be specified only in private chats
inputMessageVideo video:InputFile thumbnail:inputThumbnail added_sticker_file_ids:vector<int32> duration:int32 width:int32 height:int32 supports_streaming:Bool caption:formattedText ttl:int32 = InputMessageContent;

//@description A video note message @video_note Video note to be sent @thumbnail Video thumbnail, if available @duration Duration of the video, in seconds @length Video width and height; must be positive and not greater than 640
inputMessageVideoNote video_note:InputFile thumbnail:inputThumbnail duration:int32 length:int32 = InputMessageContent;

//@description A voice note message @voice_note Voice note to be sent @duration Duration of the voice note, in seconds @waveform Waveform representation of the voice note, in 5-bit format @caption Voice note caption; 0-200 characters
inputMessageVoiceNote voice_note:InputFile duration:int32 waveform:bytes caption:formattedText = InputMessageContent;

//@description A message with a location @location Location to be sent @live_period Period for which the location can be updated, in seconds; should bebetween 60 and 86400 for a live location and 0 otherwise
inputMessageLocation location:location live_period:int32 = InputMessageContent;

//@description A message with information about a venue @venue Venue to send
inputMessageVenue venue:venue = InputMessageContent;

//@description A message containing a user contact @contact Contact to send
inputMessageContact contact:contact = InputMessageContent;

//@description A message with a game; not supported for channels or secret chats @bot_user_id User identifier of the bot that owns the game @game_short_name Short name of the game
inputMessageGame bot_user_id:int32 game_short_name:string = InputMessageContent;

//@description A message with an invoice; can be used only by bots and only in private chats @invoice Invoice @title Product title; 1-32 characters @param_description Product description; 0-255 characters @photo_url Product photo URL; optional @photo_size Product photo size @photo_width Product photo width @photo_height Product photo height
//@payload The invoice payload @provider_token Payment provider token @provider_data JSON-encoded data about the invoice, which will be shared with the payment provider @start_parameter Unique invoice bot start_parameter for the generation of this invoice
inputMessageInvoice invoice:invoice title:string description:string photo_url:string photo_size:int32 photo_width:int32 photo_height:int32 payload:bytes provider_token:string provider_data:string start_parameter:string = InputMessageContent;

//@description A forwarded message @from_chat_id Identifier for the chat this forwarded message came from @message_id Identifier of the message to forward @in_game_share True, if a game message should be shared within a launched game; applies only to game messages
inputMessageForwarded from_chat_id:int53 message_id:int53 in_game_share:Bool = InputMessageContent;


//@class SearchMessagesFilter @description Represents a filter for message search results

//@description Returns all found messages, no filter is applied
searchMessagesFilterEmpty = SearchMessagesFilter;

//@description Returns only animation messages
searchMessagesFilterAnimation = SearchMessagesFilter;

//@description Returns only audio messages
searchMessagesFilterAudio = SearchMessagesFilter;

//@description Returns only document messages
searchMessagesFilterDocument = SearchMessagesFilter;

//@description Returns only photo messages
searchMessagesFilterPhoto = SearchMessagesFilter;

//@description Returns only video messages
searchMessagesFilterVideo = SearchMessagesFilter;

//@description Returns only voice note messages
searchMessagesFilterVoiceNote = SearchMessagesFilter;

//@description Returns only photo and video messages
searchMessagesFilterPhotoAndVideo = SearchMessagesFilter;

//@description Returns only messages containing URLs
searchMessagesFilterUrl = SearchMessagesFilter;

//@description Returns only messages containing chat photos
searchMessagesFilterChatPhoto = SearchMessagesFilter;

//@description Returns only call messages
searchMessagesFilterCall = SearchMessagesFilter;

//@description Returns only incoming call messages with missed/declined discard reasons
searchMessagesFilterMissedCall = SearchMessagesFilter;

//@description Returns only video note messages
searchMessagesFilterVideoNote = SearchMessagesFilter;

//@description Returns only voice and video note messages
searchMessagesFilterVoiceAndVideoNote = SearchMessagesFilter;

//@description Returns only messages with mentions of the current user, or messages that are replies to their messages
searchMessagesFilterMention = SearchMessagesFilter;

//@description Returns only messages with unread mentions of the current user or messages that are replies to their messages. When using this filter the results can't be additionally filtered by a query or by the sending user
searchMessagesFilterUnreadMention = SearchMessagesFilter;


//@class ChatAction @description Describes the different types of activity in a chat

//@description The user is typing a message
chatActionTyping = ChatAction;
//@description The user is recording a video
chatActionRecordingVideo = ChatAction;
//@description The user is uploading a video @progress Upload progress, as a percentage
chatActionUploadingVideo progress:int32 = ChatAction;
//@description The user is recording a voice note
chatActionRecordingVoiceNote = ChatAction;
//@description The user is uploading a voice note @progress Upload progress, as a percentage
chatActionUploadingVoiceNote progress:int32 = ChatAction;
//@description The user is uploading a photo @progress Upload progress, as a percentage
chatActionUploadingPhoto progress:int32 = ChatAction;
//@description The user is uploading a document @progress Upload progress, as a percentage
chatActionUploadingDocument progress:int32 = ChatAction;
//@description The user is picking a location or venue to send
chatActionChoosingLocation = ChatAction;
//@description The user is picking a contact to send
chatActionChoosingContact = ChatAction;
//@description The user has started to play a game
chatActionStartPlayingGame = ChatAction;
//@description The user is recording a video note
chatActionRecordingVideoNote = ChatAction;
//@description The user is uploading a video note @progress Upload progress, as a percentage
chatActionUploadingVideoNote progress:int32 = ChatAction;
//@description The user has cancelled the previous action
chatActionCancel = ChatAction;


//@class UserStatus @description Describes the last time the user was online

//@description The user status was never changed
userStatusEmpty = UserStatus;

//@description The user is online @expires Point in time (Unix timestamp) when the user's online status will expire
userStatusOnline expires:int32 = UserStatus;

//@description The user is offline @was_online Point in time (Unix timestamp) when the user was last online
userStatusOffline was_online:int32 = UserStatus;

//@description The user was online recently
userStatusRecently = UserStatus;

//@description The user is offline, but was online last week
userStatusLastWeek = UserStatus;

//@description The user is offline, but was online last month
userStatusLastMonth = UserStatus;


//@description Represents a list of stickers @stickers List of stickers
stickers stickers:vector<sticker> = Stickers;

//@description Represents a list of all emoji corresponding to a sticker in a sticker set. The list is only for informational purposes, because a sticker is always sent with a fixed emoji from the corresponding Sticker object @emojis List of emojis
stickerEmojis emojis:vector<string> = StickerEmojis;

//@description Represents a sticker set @id Identifier of the sticker set @title Title of the sticker set @name Name of the sticker set @is_installed True, if the sticker set has been installed by the current user
//@is_archived True, if the sticker set has been archived. A sticker set can't be installed and archived simultaneously @is_official True, if the sticker set is official @is_masks True, if the stickers in the set are masks
//@is_viewed True for already viewed trending sticker sets @stickers List of stickers in this set @emojis A list of emoji corresponding to the stickers in the same order
stickerSet id:int64 title:string name:string is_installed:Bool is_archived:Bool is_official:Bool is_masks:Bool is_viewed:Bool stickers:vector<sticker> emojis:vector<stickerEmojis> = StickerSet;

//@description Represents short information about a sticker set @id Identifier of the sticker set @title Title of the sticker set @name Name of the sticker set @is_installed True, if the sticker set has been installed by current user
//@is_archived True, if the sticker set has been archived. A sticker set can't be installed and archived simultaneously @is_official True, if the sticker set is official @is_masks True, if the stickers in the set are masks
//@is_viewed True for already viewed trending sticker sets @size Total number of stickers in the set @covers Contains up to the first 5 stickers from the set, depending on the context. If the client needs more stickers the full set should be requested
stickerSetInfo id:int64 title:string name:string is_installed:Bool is_archived:Bool is_official:Bool is_masks:Bool is_viewed:Bool size:int32 covers:vector<sticker> = StickerSetInfo;

//@description Represents a list of sticker sets @total_count Approximate total number of sticker sets found @sets List of sticker sets
stickerSets total_count:int32 sets:vector<stickerSetInfo> = StickerSets;


//@class CallDiscardReason @description Describes the reason why a call was discarded

//@description The call wasn't discarded, or the reason is unknown
callDiscardReasonEmpty = CallDiscardReason;

//@description The call was ended before the conversation started. It was cancelled by the caller or missed by the other party
callDiscardReasonMissed = CallDiscardReason;

//@description The call was ended before the conversation started. It was declined by the other party
callDiscardReasonDeclined = CallDiscardReason;

//@description The call was ended during the conversation because the users were disconnected
callDiscardReasonDisconnected = CallDiscardReason;

//@description The call was ended because one of the parties hung up
callDiscardReasonHungUp = CallDiscardReason;


//@description Specifies the supported call protocols @udp_p2p True, if UDP peer-to-peer connections are supported @udp_reflector True, if connection through UDP reflectors is supported @min_layer Minimum supported API layer; use 65 @max_layer Maximum supported API layer; use 65
callProtocol udp_p2p:Bool udp_reflector:Bool min_layer:int32 max_layer:int32 = CallProtocol;

//@description Describes the address of UDP reflectors @id Reflector identifier @ip IPv4 reflector address @ipv6 IPv6 reflector address @port Reflector port number @peer_tag Connection peer tag
callConnection id:int64 ip:string ipv6:string port:int32 peer_tag:bytes = CallConnection;


//@description Contains the call identifier @id Call identifier
callId id:int32 = CallId;


//@class CallState @description Describes the current call state

//@description The call is pending, waiting to be accepted by a user @is_created True, if the call has already been created by the server @is_received True, if the call has already been received by the other party
callStatePending is_created:Bool is_received:Bool = CallState;

//@description The call has been answered and encryption keys are being exchanged
callStateExchangingKeys = CallState;

//@description The call is ready to use @protocol Call protocols supported by the peer @connections Available UDP reflectors @config A JSON-encoded call config @encryption_key Call encryption key @emojis Encryption key emojis fingerprint
callStateReady protocol:callProtocol connections:vector<callConnection> config:string encryption_key:bytes emojis:vector<string> = CallState;

//@description The call is hanging up after discardCall has been called
callStateHangingUp = CallState;

//@description The call has ended successfully @reason The reason, why the call has ended @need_rating True, if the call rating should be sent to the server @need_debug_information True, if the call debug information should be sent to the server
callStateDiscarded reason:CallDiscardReason need_rating:Bool need_debug_information:Bool = CallState;

//@description The call has ended with an error @error Error. An error with the code 4005000 will be returned if an outgoing call is missed because of an expired timeout
callStateError error:error = CallState;


//@description Describes a call @id Call identifier, not persistent @user_id Peer user identifier @is_outgoing True, if the call is outgoing @state Call state
call id:int32 user_id:int32 is_outgoing:Bool state:CallState = Call;


//@description Represents a list of animations @animations List of animations
animations animations:vector<animation> = Animations;


//@description Represents the result of an ImportContacts request @user_ids User identifiers of the imported contacts in the same order as they were specified in the request; 0 if the contact is not yet a registered user
//@importer_count The number of users that imported the corresponding contact; 0 for already registered users or if unavailable
importedContacts user_ids:vector<int32> importer_count:vector<int32> = ImportedContacts;


//@class InputInlineQueryResult @description Represents a single result of an inline query; for bots only

//@description Represents a link to an animated GIF @id Unique identifier of the query result @title Title of the query result @thumbnail_url URL of the static result thumbnail (JPEG or GIF), if it exists
//@gif_url The URL of the GIF-file (file size must not exceed 1MB) @gif_duration Duration of the GIF, in seconds @gif_width Width of the GIF @gif_height Height of the GIF
//@reply_markup The message reply markup. Must be of type replyMarkupInlineKeyboard or null
//@input_message_content The content of the message to be sent. Must be one of the following types: InputMessageText, InputMessageAnimation, InputMessageLocation, InputMessageVenue or InputMessageContact
inputInlineQueryResultAnimatedGif id:string title:string thumbnail_url:string gif_url:string gif_duration:int32 gif_width:int32 gif_height:int32 reply_markup:ReplyMarkup input_message_content:InputMessageContent = InputInlineQueryResult;

//@description Represents a link to an animated (i.e. without sound) H.264/MPEG-4 AVC video @id Unique identifier of the query result @title Title of the result @thumbnail_url URL of the static result thumbnail (JPEG or GIF), if it exists
//@mpeg4_url The URL of the MPEG4-file (file size must not exceed 1MB) @mpeg4_duration Duration of the video, in seconds @mpeg4_width Width of the video @mpeg4_height Height of the video
//@reply_markup The message reply markup. Must be of type replyMarkupInlineKeyboard or null
//@input_message_content The content of the message to be sent. Must be one of the following types: InputMessageText, InputMessageAnimation, InputMessageLocation, InputMessageVenue or InputMessageContact
inputInlineQueryResultAnimatedMpeg4 id:string title:string thumbnail_url:string mpeg4_url:string mpeg4_duration:int32 mpeg4_width:int32 mpeg4_height:int32 reply_markup:ReplyMarkup input_message_content:InputMessageContent = InputInlineQueryResult;

//@description Represents a link to an article or web page @id Unique identifier of the query result @url URL of the result, if it exists @hide_url True, if the URL must be not shown @title Title of the result
//@param_description A short description of the result @thumbnail_url URL of the result thumbnail, if it exists @thumbnail_width Thumbnail width, if known @thumbnail_height Thumbnail height, if known
//@reply_markup The message reply markup. Must be of type replyMarkupInlineKeyboard or null
//@input_message_content The content of the message to be sent. Must be one of the following types: InputMessageText, InputMessageLocation, InputMessageVenue or InputMessageContact
inputInlineQueryResultArticle id:string url:string hide_url:Bool title:string description:string thumbnail_url:string thumbnail_width:int32 thumbnail_height:int32 reply_markup:ReplyMarkup input_message_content:InputMessageContent = InputInlineQueryResult;

//@description Represents a link to an MP3 audio file @id Unique identifier of the query result @title Title of the audio file @performer Performer of the audio file
//@audio_url The URL of the audio file @audio_duration Audio file duration, in seconds
//@reply_markup The message reply markup. Must be of type replyMarkupInlineKeyboard or null
//@input_message_content The content of the message to be sent. Must be one of the following types: InputMessageText, InputMessageAudio, InputMessageLocation, InputMessageVenue or InputMessageContact
inputInlineQueryResultAudio id:string title:string performer:string audio_url:string audio_duration:int32 reply_markup:ReplyMarkup input_message_content:InputMessageContent = InputInlineQueryResult;

//@description Represents a user contact @id Unique identifier of the query result @contact User contact @thumbnail_url URL of the result thumbnail, if it exists @thumbnail_width Thumbnail width, if known @thumbnail_height Thumbnail height, if known
//@reply_markup The message reply markup. Must be of type replyMarkupInlineKeyboard or null
//@input_message_content The content of the message to be sent. Must be one of the following types: InputMessageText, InputMessageLocation, InputMessageVenue or InputMessageContact
inputInlineQueryResultContact id:string contact:contact thumbnail_url:string thumbnail_width:int32 thumbnail_height:int32 reply_markup:ReplyMarkup input_message_content:InputMessageContent = InputInlineQueryResult;

//@description Represents a link to a file @id Unique identifier of the query result @title Title of the resulting file @param_description Short description of the result, if known @document_url URL of the file @mime_type MIME type of the file content; only "application/pdf" and "application/zip" are currently allowed
//@thumbnail_url The URL of the file thumbnail, if it exists @thumbnail_width Width of the thumbnail @thumbnail_height Height of the thumbnail
//@reply_markup The message reply markup. Must be of type replyMarkupInlineKeyboard or null
//@input_message_content The content of the message to be sent. Must be one of the following types: InputMessageText, InputMessageDocument, InputMessageLocation, InputMessageVenue or InputMessageContact
inputInlineQueryResultDocument id:string title:string description:string document_url:string mime_type:string thumbnail_url:string thumbnail_width:int32 thumbnail_height:int32 reply_markup:ReplyMarkup input_message_content:InputMessageContent = InputInlineQueryResult;

//@description Represents a game @id Unique identifier of the query result @game_short_name Short name of the game @reply_markup Message reply markup. Must be of type replyMarkupInlineKeyboard or null
inputInlineQueryResultGame id:string game_short_name:string reply_markup:ReplyMarkup = InputInlineQueryResult;

//@description Represents a point on the map @id Unique identifier of the query result @location Location result @live_period Amount of time relative to the message sent time until the location can be updated, in seconds @title Title of the result @thumbnail_url URL of the result thumbnail, if it exists @thumbnail_width Thumbnail width, if known @thumbnail_height Thumbnail height, if known
//@reply_markup The message reply markup. Must be of type replyMarkupInlineKeyboard or null
//@input_message_content The content of the message to be sent. Must be one of the following types: InputMessageText, InputMessageLocation, InputMessageVenue or InputMessageContact
inputInlineQueryResultLocation id:string location:location live_period:int32 title:string thumbnail_url:string thumbnail_width:int32 thumbnail_height:int32 reply_markup:ReplyMarkup input_message_content:InputMessageContent = InputInlineQueryResult;

//@description Represents link to a JPEG image @id Unique identifier of the query result @title Title of the result, if known @param_description A short description of the result, if known @thumbnail_url URL of the photo thumbnail, if it exists
//@photo_url The URL of the JPEG photo (photo size must not exceed 5MB) @photo_width Width of the photo @photo_height Height of the photo
//@reply_markup The message reply markup. Must be of type replyMarkupInlineKeyboard or null
//@input_message_content The content of the message to be sent. Must be one of the following types: InputMessageText, InputMessagePhoto, InputMessageLocation, InputMessageVenue or InputMessageContact
inputInlineQueryResultPhoto id:string title:string description:string thumbnail_url:string photo_url:string photo_width:int32 photo_height:int32 reply_markup:ReplyMarkup input_message_content:InputMessageContent = InputInlineQueryResult;

//@description Represents a link to a WEBP sticker @id Unique identifier of the query result @thumbnail_url URL of the sticker thumbnail, if it exists
//@sticker_url The URL of the WEBP sticker (sticker file size must not exceed 5MB) @sticker_width Width of the sticker @sticker_height Height of the sticker
//@reply_markup The message reply markup. Must be of type replyMarkupInlineKeyboard or null
//@input_message_content The content of the message to be sent. Must be one of the following types: InputMessageText, inputMessageSticker, InputMessageLocation, InputMessageVenue or InputMessageContact
inputInlineQueryResultSticker id:string thumbnail_url:string sticker_url:string sticker_width:int32 sticker_height:int32 reply_markup:ReplyMarkup input_message_content:InputMessageContent = InputInlineQueryResult;

//@description Represents information about a venue @id Unique identifier of the query result @venue Venue result @thumbnail_url URL of the result thumbnail, if it exists @thumbnail_width Thumbnail width, if known @thumbnail_height Thumbnail height, if known
//@reply_markup The message reply markup. Must be of type replyMarkupInlineKeyboard or null
//@input_message_content The content of the message to be sent. Must be one of the following types: InputMessageText, InputMessageLocation, InputMessageVenue or InputMessageContact
inputInlineQueryResultVenue id:string venue:venue thumbnail_url:string thumbnail_width:int32 thumbnail_height:int32 reply_markup:ReplyMarkup input_message_content:InputMessageContent = InputInlineQueryResult;

//@description Represents a link to a page containing an embedded video player or a video file @id Unique identifier of the query result @title Title of the result @param_description A short description of the result, if known
//@thumbnail_url The URL of the video thumbnail (JPEG), if it exists @video_url URL of the embedded video player or video file @mime_type MIME type of the content of the video URL, only "text/html" or "video/mp4" are currently supported
//@video_width Width of the video @video_height Height of the video @video_duration Video duration, in seconds
//@reply_markup The message reply markup. Must be of type replyMarkupInlineKeyboard or null
//@input_message_content The content of the message to be sent. Must be one of the following types: InputMessageText, InputMessageVideo, InputMessageLocation, InputMessageVenue or InputMessageContact
inputInlineQueryResultVideo id:string title:string description:string thumbnail_url:string video_url:string mime_type:string video_width:int32 video_height:int32 video_duration:int32 reply_markup:ReplyMarkup input_message_content:InputMessageContent = InputInlineQueryResult;

//@description Represents a link to an opus-encoded audio file within an OGG container, single channel audio @id Unique identifier of the query result @title Title of the voice note
//@voice_note_url The URL of the voice note file @voice_note_duration Duration of the voice note, in seconds
//@reply_markup The message reply markup. Must be of type replyMarkupInlineKeyboard or null
//@input_message_content The content of the message to be sent. Must be one of the following types: InputMessageText, InputMessageVoiceNote, InputMessageLocation, InputMessageVenue or InputMessageContact
inputInlineQueryResultVoiceNote id:string title:string voice_note_url:string voice_note_duration:int32 reply_markup:ReplyMarkup input_message_content:InputMessageContent = InputInlineQueryResult;


//@class InlineQueryResult @description Represents a single result of an inline query

//@description Represents a link to an article or web page @id Unique identifier of the query result @url URL of the result, if it exists @hide_url True, if the URL must be not shown @title Title of the result
//@param_description A short description of the result @thumbnail Result thumbnail; may be null
inlineQueryResultArticle id:string url:string hide_url:Bool title:string description:string thumbnail:photoSize = InlineQueryResult;

//@description Represents a user contact @id Unique identifier of the query result @contact A user contact @thumbnail Result thumbnail; may be null
inlineQueryResultContact id:string contact:contact thumbnail:photoSize = InlineQueryResult;

//@description Represents a point on the map @id Unique identifier of the query result @location Location result @title Title of the result @thumbnail Result thumbnail; may be null
inlineQueryResultLocation id:string location:location title:string thumbnail:photoSize = InlineQueryResult;

//@description Represents information about a venue @id Unique identifier of the query result @venue Venue result @thumbnail Result thumbnail; may be null
inlineQueryResultVenue id:string venue:venue thumbnail:photoSize = InlineQueryResult;

//@description Represents information about a game @id Unique identifier of the query result @game Game result
inlineQueryResultGame id:string game:game = InlineQueryResult;

//@description Represents an animation file @id Unique identifier of the query result @animation Animation file @title Animation title
inlineQueryResultAnimation id:string animation:animation title:string = InlineQueryResult;

//@description Represents an audio file @id Unique identifier of the query result @audio Audio file
inlineQueryResultAudio id:string audio:audio = InlineQueryResult;

//@description Represents a document @id Unique identifier of the query result @document Document @title Document title @param_description Document description
inlineQueryResultDocument id:string document:document title:string description:string = InlineQueryResult;

//@description Represents a photo @id Unique identifier of the query result @photo Photo @title Title of the result, if known @param_description A short description of the result, if known
inlineQueryResultPhoto id:string photo:photo title:string description:string = InlineQueryResult;

//@description Represents a sticker @id Unique identifier of the query result @sticker Sticker
inlineQueryResultSticker id:string sticker:sticker = InlineQueryResult;

//@description Represents a video @id Unique identifier of the query result @video Video @title Title of the video @param_description Description of the video
inlineQueryResultVideo id:string video:video title:string description:string = InlineQueryResult;

//@description Represents a voice note @id Unique identifier of the query result @voice_note Voice note @title Title of the voice note
inlineQueryResultVoiceNote id:string voice_note:voiceNote title:string = InlineQueryResult;


//@description Represents the results of the inline query. Use sendInlineQueryResultMessage to send the result of the query @inline_query_id Unique identifier of the inline query @next_offset The offset for the next request. If empty, there are no more results @results Results of the query
//@switch_pm_text If non-empty, this text should be shown on the button, which opens a private chat with the bot and sends the bot a start message with the switch_pm_parameter @switch_pm_parameter Parameter for the bot start message
inlineQueryResults inline_query_id:int64 next_offset:string results:vector<InlineQueryResult> switch_pm_text:string switch_pm_parameter:string = InlineQueryResults;


//@class CallbackQueryPayload @description Represents a payload of a callback query

//@description The payload from a general callback button @data Data that was attached to the callback button
callbackQueryPayloadData data:bytes = CallbackQueryPayload;

//@description The payload from a game callback button @game_short_name A short name of the game that was attached to the callback button
callbackQueryPayloadGame game_short_name:string = CallbackQueryPayload;


//@description Contains a bot's answer to a callback query @text Text of the answer @show_alert True, if an alert should be shown to the user instead of a toast notification @url URL to be opened
callbackQueryAnswer text:string show_alert:Bool url:string = CallbackQueryAnswer;


//@description Contains the result of a custom request @result A JSON-serialized result
customRequestResult result:string = CustomRequestResult;


//@description Contains one row of the game high score table @position Position in the high score table @user_id User identifier @score User score
gameHighScore position:int32 user_id:int32 score:int32 = GameHighScore;

//@description Contains a list of game high scores @scores A list of game high scores
gameHighScores scores:vector<gameHighScore> = GameHighScores;


//@class ChatEventAction @description Represents a chat event

//@description A message was edited @old_message The original message before the edit @new_message The message after it was edited
chatEventMessageEdited old_message:message new_message:message = ChatEventAction;

//@description A message was deleted @message Deleted message
chatEventMessageDeleted message:message = ChatEventAction;

//@description A message was pinned @message Pinned message
chatEventMessagePinned message:message = ChatEventAction;

//@description A message was unpinned
chatEventMessageUnpinned = ChatEventAction;

//@description A new member joined the chat
chatEventMemberJoined = ChatEventAction;

//@description A member left the chat
chatEventMemberLeft = ChatEventAction;

//@description A new chat member was invited @user_id New member user identifier @status New member status
chatEventMemberInvited user_id:int32 status:ChatMemberStatus = ChatEventAction;

//@description A chat member has gained/lost administrator status, or the list of their administrator privileges has changed @user_id Chat member user identifier @old_status Previous status of the chat member @new_status New status of the chat member
chatEventMemberPromoted user_id:int32 old_status:ChatMemberStatus new_status:ChatMemberStatus = ChatEventAction;

//@description A chat member was restricted/unrestricted or banned/unbanned, or the list of their restrictions has changed @user_id Chat member user identifier @old_status Previous status of the chat member @new_status New status of the chat member
chatEventMemberRestricted user_id:int32 old_status:ChatMemberStatus new_status:ChatMemberStatus = ChatEventAction;

//@description The chat title was changed @old_title Previous chat title @new_title New chat title
chatEventTitleChanged old_title:string new_title:string = ChatEventAction;

//@description The chat description was changed @old_description Previous chat description @new_description New chat description
chatEventDescriptionChanged old_description:string new_description:string = ChatEventAction;

//@description The chat username was changed @old_username Previous chat username @new_username New chat username
chatEventUsernameChanged old_username:string new_username:string = ChatEventAction;

//@description The chat photo was changed @old_photo Previous chat photo value; may be null @new_photo New chat photo value; may be null
chatEventPhotoChanged old_photo:chatPhoto new_photo:chatPhoto = ChatEventAction;

//@description The anyone_can_invite setting of a supergroup chat was toggled @anyone_can_invite New value of anyone_can_invite
chatEventInvitesToggled anyone_can_invite:Bool = ChatEventAction;

//@description The sign_messages setting of a channel was toggled @sign_messages New value of sign_messages
chatEventSignMessagesToggled sign_messages:Bool = ChatEventAction;

//@description The supergroup sticker set was changed @old_sticker_set_id Previous identifier of the chat sticker set; 0 if none @new_sticker_set_id New identifier of the chat sticker set; 0 if none
chatEventStickerSetChanged old_sticker_set_id:int64 new_sticker_set_id:int64 = ChatEventAction;

//@description The is_all_history_available setting of a supergroup was toggled @is_all_history_available New value of is_all_history_available
chatEventIsAllHistoryAvailableToggled is_all_history_available:Bool = ChatEventAction;

//@description Represents a chat event @id Chat event identifier @date Point in time (Unix timestamp) when the event happened @user_id Identifier of the user who performed the action that triggered the event @action Action performed by the user
chatEvent id:int64 date:int32 user_id:int32 action:ChatEventAction = ChatEvent;

//@description Contains a list of chat events @events List of events
chatEvents events:vector<chatEvent> = ChatEvents;

//@description Represents a set of filters used to obtain a chat event log
//@message_edits True, if message edits should be returned
//@message_deletions True, if message deletions should be returned
//@message_pins True, if pin/unpin events should be returned
//@member_joins True, if members joining events should be returned
//@member_leaves True, if members leaving events should be returned
//@member_invites True, if invited member events should be returned
//@member_promotions True, if member promotion/demotion events should be returned
//@member_restrictions True, if member restricted/unrestricted/banned/unbanned events should be returned
//@info_changes True, if changes in chat information should be returned
//@setting_changes True, if changes in chat settings should be returned
chatEventLogFilters message_edits:Bool message_deletions:Bool message_pins:Bool member_joins:Bool member_leaves:Bool member_invites:Bool member_promotions:Bool member_restrictions:Bool info_changes:Bool setting_changes:Bool = ChatEventLogFilters;


//@class DeviceToken @description Represents a data needed to subscribe for push notifications. To use specific push notification service, you must specify the correct application platform and upload valid server authentication data at https://my.telegram.org

//@description A token for Google Cloud Messaging @token Device registration token, may be empty to de-register a device
deviceTokenGoogleCloudMessaging token:string = DeviceToken;

//@description A token for Apple Push Notification service @device_token Device token, may be empty to de-register a device @is_app_sandbox True, if App Sandbox is enabled
deviceTokenApplePush device_token:string is_app_sandbox:Bool = DeviceToken;

//@description A token for Apple Push Notification service VoIP notifications @device_token Device token, may be empty to de-register a device @is_app_sandbox True, if App Sandbox is enabled
deviceTokenApplePushVoIP device_token:string is_app_sandbox:Bool = DeviceToken;

//@description A token for Windows Push Notification Services @access_token The access token that will be used to send notifications, may be empty to de-register a device
deviceTokenWindowsPush access_token:string = DeviceToken;

//@description A token for Microsoft Push Notification Service @channel_uri Push notification channel URI, may be empty to de-register a device
deviceTokenMicrosoftPush channel_uri:string = DeviceToken;

//@description A token for Microsoft Push Notification Service VoIP channel @channel_uri Push notification channel URI, may be empty to de-register a device
deviceTokenMicrosoftPushVoIP channel_uri:string = DeviceToken;

//@description A token for web Push API @endpoint Absolute URL exposed by the push service where the application server can send push messages, may be empty to de-register a device
//@p256dh_base64url Base64url-encoded P-256 elliptic curve Diffie-Hellman public key @auth_base64url Base64url-encoded authentication secret
deviceTokenWebPush endpoint:string p256dh_base64url:string auth_base64url:string = DeviceToken;

//@description A token for Simple Push API for Firefox OS @endpoint Absolute URL exposed by the push service where the application server can send push messages, may be empty to de-register a device
deviceTokenSimplePush endpoint:string = DeviceToken;

//@description A token for Ubuntu Push Client service @token Token, may be empty to de-register a device
deviceTokenUbuntuPush token:string = DeviceToken;

//@description A token for BlackBerry Push Service @token Token, may be empty to de-register a device
deviceTokenBlackBerryPush token:string = DeviceToken;

//@description A token for Tizen Push Service @reg_id Push service registration identifier, may be empty to de-register a device
deviceTokenTizenPush reg_id:string = DeviceToken;


//@description Contains information about a wallpaper @id Unique persistent wallpaper identifier @sizes Available variants of the wallpaper in different sizes. These photos can only be downloaded; they can't be sent in a message @color Main color of the wallpaper in RGB24 format; should be treated as background color if no photos are specified
wallpaper id:int32 sizes:vector<photoSize> color:int32 = Wallpaper;

//@description Contains a list of wallpapers @wallpapers A list of wallpapers
wallpapers wallpapers:vector<wallpaper> = Wallpapers;


//@description Contains a list of hashtags @hashtags A list of hashtags
hashtags hashtags:vector<string> = Hashtags;


//@class CheckChatUsernameResult @description Represents result of checking whether a username can be set for a chat

//@description The username can be set
checkChatUsernameResultOk = CheckChatUsernameResult;

//@description The username is invalid
checkChatUsernameResultUsernameInvalid = CheckChatUsernameResult;

//@description The username is occupied
checkChatUsernameResultUsernameOccupied = CheckChatUsernameResult;

//@description The user has too much public chats, one of them should be made private first
checkChatUsernameResultPublicChatsTooMuch = CheckChatUsernameResult;

//@description The user can't be a member of a public supergroup
checkChatUsernameResultPublicGroupsUnavailable = CheckChatUsernameResult;


//@class OptionValue @description Represents the value of an option

//@description Boolean option @value The value of the option
optionValueBoolean value:Bool = OptionValue;

//@description An unknown option or an option which has a default value
optionValueEmpty = OptionValue;

//@description An integer option @value The value of the option
optionValueInteger value:int32 = OptionValue;

//@description A string option @value The value of the option
optionValueString value:string = OptionValue;


//@class UserPrivacySettingRule @description Represents a single rule for managing privacy settings

//@description A rule to allow all users to do something
userPrivacySettingRuleAllowAll = UserPrivacySettingRule;

//@description A rule to allow all of a user's contacts to do something
userPrivacySettingRuleAllowContacts = UserPrivacySettingRule;

//@description A rule to allow certain specified users to do something @user_ids The user identifiers
userPrivacySettingRuleAllowUsers user_ids:vector<int32> = UserPrivacySettingRule;

//@description A rule to restrict all users from doing something
userPrivacySettingRuleRestrictAll = UserPrivacySettingRule;

//@description A rule to restrict all contacts of a user from doing something
userPrivacySettingRuleRestrictContacts = UserPrivacySettingRule;

//@description A rule to restrict all specified users from doing something @user_ids The user identifiers
userPrivacySettingRuleRestrictUsers user_ids:vector<int32> = UserPrivacySettingRule;

//@description A list of privacy rules. Rules are matched in the specified order. The first matched rule defines the privacy setting for a given user. If no rule matches, the action is not allowed @rules A list of rules
userPrivacySettingRules rules:vector<UserPrivacySettingRule> = UserPrivacySettingRules;

//@class UserPrivacySetting @description Describes available user privacy settings

//@description A privacy setting for managing whether the user's online status is visible
userPrivacySettingShowStatus = UserPrivacySetting;

//@description A privacy setting for managing whether the user can be invited to chats
userPrivacySettingAllowChatInvites = UserPrivacySetting;

//@description A privacy setting for managing whether the user can be called
userPrivacySettingAllowCalls = UserPrivacySetting;


//@description Contains information about the period of inactivity after which the current user's account will automatically be deleted @days Number of days of inactivity before the account will be flagged for deletion; should range from 30-366 days
accountTtl days:int32 = AccountTtl;


//@description Contains information about one session in a Telegram application used by the current user @id Session identifier @is_current True, if this session is the current session
//@api_id Telegram API identifier, as provided by the application @application_name Name of the application, as provided by the application
//@application_version The version of the application, as provided by the application @is_official_application True, if the application is an official application or uses the api_id of an official application
//@device_model Model of the device the application has been run or is running on, as provided by the application @platform Operating system the application has been run or is running on, as provided by the application
//@system_version Version of the operating system the application has been run or is running on, as provided by the application @log_in_date Point in time (Unix timestamp) when the user has logged in
//@last_active_date Point in time (Unix timestamp) when the session was last used @ip IP address from which the session was created, in human-readable format
//@country A two-letter country code for the country from which the session was created, based on the IP address @region Region code from which the session was created, based on the IP address
session id:int64 is_current:Bool api_id:int32 application_name:string application_version:string is_official_application:Bool device_model:string platform:string system_version:string log_in_date:int32 last_active_date:int32 ip:string country:string region:string = Session;

//@description Contains a list of sessions @sessions List of sessions
sessions sessions:vector<session> = Sessions;


//@description Contains information about one website the current user is logged in with Telegram
//@id Website identifier
//@domain_name The domain name of the website
//@bot_user_id User identifier of a bot linked with the website
//@browser The version of a browser used to log in
//@platform Operating system the browser is running on
//@log_in_date Point in time (Unix timestamp) when the user was logged in
//@last_active_date Point in time (Unix timestamp) when obtained authorization was last used
//@ip IP address from which the user was logged in, in human-readable format
//@location Human-readable description of a country and a region, from which the user was logged in, based on the IP address
connectedWebsite id:int64 domain_name:string bot_user_id:int32 browser:string platform:string log_in_date:int32 last_active_date:int32 ip:string location:string = ConnectedWebsite;

//@description Contains a list of websites the current user is logged in with Telegram @websites List of connected websites
connectedWebsites websites:vector<connectedWebsite> = ConnectedWebsites;


//@description Contains information about the availability of the "Report spam" action for a chat @can_report_spam True, if a prompt with the "Report spam" action should be shown to the user
chatReportSpamState can_report_spam:Bool = ChatReportSpamState;

//@class ChatReportReason @description Describes the reason why a chat is reported

//@description The chat contains spam messages
chatReportReasonSpam = ChatReportReason;

//@description The chat promotes violence
chatReportReasonViolence = ChatReportReason;

//@description The chat contains pornographic messages
chatReportReasonPornography = ChatReportReason;

//@description A custom reason provided by the user @text Report text
chatReportReasonCustom text:string = ChatReportReason;


//@description Contains a public HTTPS link to a message in a public supergroup or channel @link Message link @html HTML-code for embedding the message
publicMessageLink link:string html:string = PublicMessageLink;


//@class FileType @description Represents the type of a file

//@description The data is not a file
fileTypeNone = FileType;

//@description The file is an animation
fileTypeAnimation = FileType;

//@description The file is an audio file
fileTypeAudio = FileType;

//@description The file is a document
fileTypeDocument = FileType;

//@description The file is a photo
fileTypePhoto = FileType;

//@description The file is a profile photo
fileTypeProfilePhoto = FileType;

//@description The file was sent to a secret chat (the file type is not known to the server)
fileTypeSecret = FileType;

//@description The file is a sticker
fileTypeSticker = FileType;

//@description The file is a thumbnail of another file
fileTypeThumbnail = FileType;

//@description The file type is not yet known
fileTypeUnknown = FileType;

//@description The file is a video
fileTypeVideo = FileType;

//@description The file is a video note
fileTypeVideoNote = FileType;

//@description The file is a voice note
fileTypeVoiceNote = FileType;

//@description The file is a wallpaper
fileTypeWallpaper = FileType;

//@description The file is a thumbnail of a file from a secret chat
fileTypeSecretThumbnail = FileType;


//@description Contains the storage usage statistics for a specific file type @file_type File type @size Total size of the files @count Total number of files
storageStatisticsByFileType file_type:FileType size:int53 count:int32 = StorageStatisticsByFileType;

//@description Contains the storage usage statistics for a specific chat @chat_id Chat identifier; 0 if none @size Total size of the files in the chat @count Total number of files in the chat @by_file_type Statistics split by file types
storageStatisticsByChat chat_id:int53 size:int53 count:int32 by_file_type:vector<storageStatisticsByFileType> = StorageStatisticsByChat;

//@description Contains the exact storage usage statistics split by chats and file type @size Total size of files @count Total number of files @by_chat Statistics split by chats
storageStatistics size:int53 count:int32 by_chat:vector<storageStatisticsByChat> = StorageStatistics;

//@description Contains approximate storage usage statistics, excluding files of unknown file type @files_size Approximate total size of files @file_count Approximate number of files @database_size Size of the database
storageStatisticsFast files_size:int53 file_count:int32 database_size:int53 = StorageStatisticsFast;


//@class NetworkType @description Represents the type of a network

//@description The network is not available
networkTypeNone = NetworkType;

//@description A mobile network
networkTypeMobile = NetworkType;

//@description A mobile roaming network
networkTypeMobileRoaming = NetworkType;

//@description A Wi-Fi network
networkTypeWiFi = NetworkType;

//@description A different network type (e.g., Ethernet network)
networkTypeOther = NetworkType;


//@class NetworkStatisticsEntry @description Contains statistics about network usage

//@description Contains information about the total amount of data that was used to send and receive files @file_type Type of the file the data is part of @network_type Type of the network the data was sent through. Call setNetworkType to maintain the actual network type
//@sent_bytes Total number of bytes sent @received_bytes Total number of bytes received
networkStatisticsEntryFile file_type:FileType network_type:NetworkType sent_bytes:int53 received_bytes:int53 = NetworkStatisticsEntry;

//@description Contains information about the total amount of data that was used for calls @network_type Type of the network the data was sent through. Call setNetworkType to maintain the actual network type
//@sent_bytes Total number of bytes sent @received_bytes Total number of bytes received @duration Total call duration, in seconds
networkStatisticsEntryCall network_type:NetworkType sent_bytes:int53 received_bytes:int53 duration:double = NetworkStatisticsEntry;

//@description A full list of available network statistic entries @since_date Point in time (Unix timestamp) when the app began collecting statistics @entries Network statistics entries
networkStatistics since_date:int32 entries:vector<NetworkStatisticsEntry> = NetworkStatistics;


//@class ConnectionState @description Describes the current state of the connection to Telegram servers

//@description Currently waiting for the network to become available. Use SetNetworkType to change the available network type
connectionStateWaitingForNetwork = ConnectionState;

//@description Currently establishing a connection with a proxy server
connectionStateConnectingToProxy = ConnectionState;

//@description Currently establishing a connection to the Telegram servers
connectionStateConnecting = ConnectionState;

//@description Downloading data received while the client was offline
connectionStateUpdating = ConnectionState;

//@description There is a working connection to the Telegram servers
connectionStateReady = ConnectionState;


//@class TopChatCategory @description Represents the categories of chats for which a list of frequently used chats can be retrieved

//@description A category containing frequently used private chats with non-bot users
topChatCategoryUsers = TopChatCategory;

//@description A category containing frequently used private chats with bot users
topChatCategoryBots = TopChatCategory;

//@description A category containing frequently used basic groups and supergroups
topChatCategoryGroups = TopChatCategory;

//@description A category containing frequently used channels
topChatCategoryChannels = TopChatCategory;

//@description A category containing frequently used chats with inline bots sorted by their usage in inline mode
topChatCategoryInlineBots = TopChatCategory;

//@description A category containing frequently used chats used for calls
topChatCategoryCalls = TopChatCategory;


//@class TMeUrlType @description Describes the type of a URL linking to an internal Telegram entity

//@description A URL linking to a user @user_id Identifier of the user
tMeUrlTypeUser user_id:int32 = TMeUrlType;

//@description A URL linking to a public supergroup or channel @supergroup_id Identifier of the supergroup or channel
tMeUrlTypeSupergroup supergroup_id:int53 = TMeUrlType;

//@description A chat invite link @info Chat invite link info
tMeUrlTypeChatInvite info:chatInviteLinkInfo = TMeUrlType;

//@description A URL linking to a sticker set @sticker_set_id Identifier of the sticker set
tMeUrlTypeStickerSet sticker_set_id:int64 = TMeUrlType;

//@description Represents a URL linking to an internal Telegram entity @url URL @type Type of the URL
tMeUrl url:string type:TMeUrlType = TMeUrl;

//@description Contains a list of t.me URLs @urls List of URLs
tMeUrls urls:vector<tMeUrl> = TMeUrls;


//@description Contains a counter @count Count
count count:int32 = Count;

//@description Contains some text @text Text
text text:string = Text;


//@class TextParseMode @description Describes the way the text should be parsed for TextEntities

//@description The text should be parsed in markdown-style
textParseModeMarkdown = TextParseMode;

//@description The text should be parsed in HTML-style
textParseModeHTML = TextParseMode;


//@class Proxy @description Contains information about a proxy server

//@description An empty proxy server
proxyEmpty = Proxy;

//@description A SOCKS5 proxy server @server Proxy server IP address @port Proxy server port @username Username for logging in @password Password for logging in
proxySocks5 server:string port:int32 username:string password:string = Proxy;


//@description Describes a sticker that should be added to a sticker set @png_sticker PNG image with the sticker; must be up to 512 kB in size and fit in a 512x512 square @emojis Emoji corresponding to the sticker @mask_position For masks, position where the mask should be placed; may be null
inputSticker png_sticker:InputFile emojis:string mask_position:maskPosition = InputSticker;


//@class Update @description Contains notifications about data changes

//@description The user authorization state has changed @authorization_state New authorization state
updateAuthorizationState authorization_state:AuthorizationState = Update;

//@description A new message was received; can also be an outgoing message @message The new message @disable_notification True, if this message must not generate a notification @contains_mention True, if the message contains a mention of the current user
updateNewMessage message:message disable_notification:Bool contains_mention:Bool = Update;

//@description A request to send a message has reached the Telegram server. This doesn't mean that the message will be sent successfully or even that the send message request will be processed. This update will be sent only if the option "use_quick_ack" is set to true. This update may be sent multiple times for the same message
//@chat_id The chat identifier of the sent message @message_id A temporary message identifier
updateMessageSendAcknowledged chat_id:int53 message_id:int53 = Update;

//@description A message has been successfully sent @message Information about the sent message. Usually only the message identifier, date, and content are changed, but almost all other fields can also change @old_message_id The previous temporary message identifier
updateMessageSendSucceeded message:message old_message_id:int53 = Update;

//@description A message failed to send. Be aware that some messages being sent can be irrecoverably deleted, in which case updateDeleteMessages will be received instead of this update
//@message Contains information about the message that failed to send @old_message_id The previous temporary message identifier @error_code An error code @error_message Error message
updateMessageSendFailed message:message old_message_id:int53 error_code:int32 error_message:string = Update;

//@description The message content has changed @chat_id Chat identifier @message_id Message identifier @new_content New message content
updateMessageContent chat_id:int53 message_id:int53 new_content:MessageContent = Update;

//@description A message was edited. Changes in the message content will come in a separate updateMessageContent @chat_id Chat identifier @message_id Message identifier @edit_date Point in time (Unix timestamp) when the message was edited @reply_markup New message reply markup; may be null
updateMessageEdited chat_id:int53 message_id:int53 edit_date:int32 reply_markup:ReplyMarkup = Update;

//@description The view count of the message has changed @chat_id Chat identifier @message_id Message identifier @views New value of the view count
updateMessageViews chat_id:int53 message_id:int53 views:int32 = Update;

//@description The message content was opened. Updates voice note messages to "listened", video note messages to "viewed" and starts the TTL timer for self-destructing messages @chat_id Chat identifier @message_id Message identifier
updateMessageContentOpened chat_id:int53 message_id:int53 = Update;

//@description A message with an unread mention was read @chat_id Chat identifier @message_id Message identifier @unread_mention_count The new number of unread mention messages left in the chat
updateMessageMentionRead chat_id:int53 message_id:int53 unread_mention_count:int32 = Update;

//@description A new chat has been loaded/created. This update is guaranteed to come before the chat identifier is returned to the client. The chat field changes will be reported through separate updates @chat The chat
updateNewChat chat:chat = Update;

//@description The title of a chat was changed @chat_id Chat identifier @title The new chat title
updateChatTitle chat_id:int53 title:string = Update;

//@description A chat photo was changed @chat_id Chat identifier @photo The new chat photo; may be null
updateChatPhoto chat_id:int53 photo:chatPhoto = Update;

//@description The last message of a chat was changed. If last_message is null then the last message in the chat became unknown. Some new unknown messages might be added to the chat in this case @chat_id Chat identifier @last_message The new last message in the chat; may be null @order New value of the chat order
updateChatLastMessage chat_id:int53 last_message:message order:int64 = Update;

//@description The order of the chat in the chats list has changed. Instead of this update updateChatLastMessage, updateChatIsPinned or updateChatDraftMessage might be sent @chat_id Chat identifier @order New value of the order
updateChatOrder chat_id:int53 order:int64 = Update;

//@description A chat was pinned or unpinned @chat_id Chat identifier @is_pinned New value of is_pinned @order New value of the chat order
updateChatIsPinned chat_id:int53 is_pinned:Bool order:int64 = Update;

//@description Incoming messages were read or number of unread messages has been changed @chat_id Chat identifier @last_read_inbox_message_id Identifier of the last read incoming message @unread_count The number of unread messages left in the chat
updateChatReadInbox chat_id:int53 last_read_inbox_message_id:int53 unread_count:int32 = Update;

//@description Outgoing messages were read @chat_id Chat identifier @last_read_outbox_message_id Identifier of last read outgoing message
updateChatReadOutbox chat_id:int53 last_read_outbox_message_id:int53 = Update;

//@description The chat unread_mention_count has changed @chat_id Chat identifier @unread_mention_count The number of unread mention messages left in the chat
updateChatUnreadMentionCount chat_id:int53 unread_mention_count:int32 = Update;

//@description Notification settings for some chats were updated @scope Types of chats for which notification settings were updated @notification_settings The new notification settings
updateNotificationSettings scope:NotificationSettingsScope notification_settings:notificationSettings = Update;

//@description The default chat reply markup was changed. Can occur because new messages with reply markup were received or because an old reply markup was hidden by the user
//@chat_id Chat identifier @reply_markup_message_id Identifier of the message from which reply markup needs to be used; 0 if there is no default custom reply markup in the chat
updateChatReplyMarkup chat_id:int53 reply_markup_message_id:int53 = Update;

//@description A draft has changed. Be aware that the update may come in the currently opened chat but with old content of the draft. If the user has changed the content of the draft, this update shouldn't be applied @chat_id Chat identifier @draft_message The new draft message; may be null @order New value of the chat order
updateChatDraftMessage chat_id:int53 draft_message:draftMessage order:int64 = Update;

//@description Some messages were deleted @chat_id Chat identifier @message_ids Identifiers of the deleted messages
//@is_permanent True, if the messages are permanently deleted by a user (as opposed to just becoming unaccessible)
//@from_cache True, if the messages are deleted only from the cache and can possibly be retrieved again in the future
updateDeleteMessages chat_id:int53 message_ids:vector<int53> is_permanent:Bool from_cache:Bool = Update;

//@description User activity in the chat has changed @chat_id Chat identifier @user_id Identifier of a user performing an action @action The action description
updateUserChatAction chat_id:int53 user_id:int32 action:ChatAction = Update;

//@description The user went online or offline @user_id User identifier @status New status of the user
updateUserStatus user_id:int32 status:UserStatus = Update;

//@description Some data of a user has changed. This update is guaranteed to come before the user identifier is returned to the client @user New data about the user
updateUser user:user = Update;

//@description Some data of a basic group has changed. This update is guaranteed to come before the basic group identifier is returned to the client @basic_group New data about the group
updateBasicGroup basic_group:basicGroup = Update;

//@description Some data of a supergroup or a channel has changed. This update is guaranteed to come before the supergroup identifier is returned to the client @supergroup New data about the supergroup
updateSupergroup supergroup:supergroup = Update;

//@description Some data of a secret chat has changed. This update is guaranteed to come before the secret chat identifier is returned to the client @secret_chat New data about the secret chat
updateSecretChat secret_chat:secretChat = Update;

//@description Some data from userFullInfo has been changed @user_id User identifier @user_full_info New full information about the user
updateUserFullInfo user_id:int32 user_full_info:userFullInfo = Update;

//@description Some data from basicGroupFullInfo has been changed @basic_group_id Identifier of a basic group @basic_group_full_info New full information about the group
updateBasicGroupFullInfo basic_group_id:int32 basic_group_full_info:basicGroupFullInfo = Update;

//@description Some data from supergroupFullInfo has been changed @supergroup_id Identifier of the supergroup or channel @supergroup_full_info New full information about the supergroup
updateSupergroupFullInfo supergroup_id:int32 supergroup_full_info:supergroupFullInfo = Update;

//@description Service notification from the server. Upon receiving this the client must show a popup with the content of the notification @type Notification type @content Notification content
updateServiceNotification type:string content:MessageContent = Update;

//@description Information about a file was updated @file New data about the file
updateFile file:file = Update;

//@description The file generation process needs to be started by the client
//@generation_id Unique identifier for the generation process
//@original_path The path to a file from which a new file is generated, may be empty
//@destination_path The path to a file that should be created and where the new file should be generated
//@conversion String specifying the conversion applied to the original file. If conversion is "#url#" than original_path contains a HTTP/HTTPS URL of a file, which should be downloaded by the client
updateFileGenerationStart generation_id:int64 original_path:string destination_path:string conversion:string = Update;

//@description File generation is no longer needed @generation_id Unique identifier for the generation process
updateFileGenerationStop generation_id:int64 = Update;

//@description New call was created or information about a call was updated @call New data about a call
updateCall call:call = Update;

//@description Some privacy setting rules have been changed @setting The privacy setting @rules New privacy rules
updateUserPrivacySettingRules setting:UserPrivacySetting rules:userPrivacySettingRules = Update;

//@description Number of unread messages has changed. This update is sent only if a message database is used @unread_count Total number of unread messages @unread_unmuted_count Total number of unread messages in unmuted chats
updateUnreadMessageCount unread_count:int32 unread_unmuted_count:int32 = Update;

//@description An option changed its value @name The option name @value The new option value
updateOption name:string value:OptionValue = Update;

//@description The list of installed sticker sets was updated @is_masks True, if the list of installed mask sticker sets was updated @sticker_set_ids The new list of installed ordinary sticker sets
updateInstalledStickerSets is_masks:Bool sticker_set_ids:vector<int64> = Update;

//@description The list of trending sticker sets was updated or some of them were viewed @sticker_sets The new list of trending sticker sets
updateTrendingStickerSets sticker_sets:stickerSets = Update;

//@description The list of recently used stickers was updated @is_attached True, if the list of stickers attached to photo or video files was updated, otherwise the list of sent stickers is updated @sticker_ids The new list of file identifiers of recently used stickers
updateRecentStickers is_attached:Bool sticker_ids:vector<int32> = Update;

//@description The list of favorite stickers was updated @sticker_ids The new list of file identifiers of favorite stickers
updateFavoriteStickers sticker_ids:vector<int32> = Update;

//@description The list of saved animations was updated @animation_ids The new list of file identifiers of saved animations
updateSavedAnimations animation_ids:vector<int32> = Update;

//@description The connection state has changed @state The new connection state
updateConnectionState state:ConnectionState = Update;

//@description A new incoming inline query; for bots only @id Unique query identifier @sender_user_id Identifier of the user who sent the query @user_location User location, provided by the client; may be null @query Text of the query @offset Offset of the first entry to return
updateNewInlineQuery id:int64 sender_user_id:int32 user_location:location query:string offset:string = Update;

//@description The user has chosen a result of an inline query; for bots only @sender_user_id Identifier of the user who sent the query @user_location User location, provided by the client; may be null @query Text of the query @result_id Identifier of the chosen result @inline_message_id Identifier of the sent inline message, if known
updateNewChosenInlineResult sender_user_id:int32 user_location:location query:string result_id:string inline_message_id:string = Update;

//@description A new incoming callback query; for bots only @id Unique query identifier @sender_user_id Identifier of the user who sent the query @chat_id Identifier of the chat, in which the query was sent
//@message_id Identifier of the message, from which the query originated @chat_instance Identifier that uniquely corresponds to the chat to which the message was sent @payload Query payload
updateNewCallbackQuery id:int64 sender_user_id:int32 chat_id:int53 message_id:int53 chat_instance:int64 payload:CallbackQueryPayload = Update;

//@description A new incoming callback query from a message sent via a bot; for bots only @id Unique query identifier @sender_user_id Identifier of the user who sent the query @inline_message_id Identifier of the inline message, from which the query originated
//@chat_instance An identifier uniquely corresponding to the chat a message was sent to @payload Query payload
updateNewInlineCallbackQuery id:int64 sender_user_id:int32 inline_message_id:string chat_instance:int64 payload:CallbackQueryPayload = Update;

//@description A new incoming shipping query; for bots only. Only for invoices with flexible price @id Unique query identifier @sender_user_id Identifier of the user who sent the query @invoice_payload Invoice payload @shipping_address User shipping address
updateNewShippingQuery id:int64 sender_user_id:int32 invoice_payload:string shipping_address:shippingAddress = Update;

//@description A new incoming pre-checkout query; for bots only. Contains full information about a checkout @id Unique query identifier @sender_user_id Identifier of the user who sent the query @currency Currency for the product price @total_amount Total price for the product, in the minimal quantity of the currency
//@invoice_payload Invoice payload @shipping_option_id Identifier of a shipping option chosen by the user; may be empty if not applicable @order_info Information about the order; may be null
updateNewPreCheckoutQuery id:int64 sender_user_id:int32 currency:string total_amount:int53 invoice_payload:bytes shipping_option_id:string order_info:orderInfo = Update;

//@description A new incoming event; for bots only @event A JSON-serialized event
updateNewCustomEvent event:string = Update;

//@description A new incoming query; for bots only @id The query identifier @data JSON-serialized query data @timeout Query timeout
updateNewCustomQuery id:int64 data:string timeout:int32 = Update;


//@description A simple object containing a number; for testing only @value Number
testInt value:int32 = TestInt;
//@description A simple object containing a string; for testing only @value String
testString value:string = TestString;
//@description A simple object containing a sequence of bytes; for testing only @value Bytes
testBytes value:bytes = TestBytes;
//@description A simple object containing a vector of numbers; for testing only @value Vector of numbers
testVectorInt value:vector<int32> = TestVectorInt;
//@description A simple object containing a vector of objects that hold a number; for testing only @value Vector of objects
testVectorIntObject value:vector<testInt> = TestVectorIntObject;
//@description A simple object containing a vector of strings; for testing only @value Vector of strings
testVectorString value:vector<string> = TestVectorString;
//@description A simple object containing a vector of objects that hold a string; for testing only @value Vector of objects
testVectorStringObject value:vector<testString> = TestVectorStringObject;
