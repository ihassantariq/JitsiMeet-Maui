using System;
using Foundation;
using UIKit;
using ObjCRuntime;

namespace JitsiMeet.iOS.Binding
{
    // @interface JitsiMeetUserInfo : NSObject
    [BaseType(typeof(NSObject))]
    interface JitsiMeetUserInfo
    {
        // @property (copy, nonatomic) NSString * _Nullable displayName;
        [NullAllowed, Export("displayName")]
        string DisplayName { get; set; }

        // @property (copy, nonatomic) NSString * _Nullable email;
        [NullAllowed, Export("email")]
        string Email { get; set; }

        // @property (copy, nonatomic) NSURL * _Nullable avatar;
        [NullAllowed, Export("avatar", ArgumentSemantic.Copy)]
        NSUrl Avatar { get; set; }

        // -(instancetype _Nullable)initWithDisplayName:(NSString * _Nullable)displayName andEmail:(NSString * _Nullable)email andAvatar:(NSURL * _Nullable)avatar;
        [Export("initWithDisplayName:andEmail:andAvatar:")]
        IntPtr Constructor([NullAllowed] string displayName, [NullAllowed] string email, [NullAllowed] NSUrl avatar);
    }

    // @interface JitsiMeetConferenceOptionsBuilder : NSObject
    [BaseType(typeof(NSObject))]
    interface JitsiMeetConferenceOptionsBuilder
    {
        // @property (copy, nonatomic) NSURL * _Nullable serverURL;
        [NullAllowed, Export("serverURL", ArgumentSemantic.Copy)]
        NSUrl ServerURL { get; set; }

        // @property (copy, nonatomic) NSString * _Nullable room;
        [NullAllowed, Export("room")]
        string Room { get; set; }

        // @property (copy, nonatomic) NSString * _Nullable token;
        [NullAllowed, Export("token")]
        string Token { get; set; }

        // @property (readonly, nonatomic) NSDictionary * _Nonnull featureFlags;
        [Export("featureFlags")]
        NSDictionary FeatureFlags { get; }

        // @property (readonly, nonatomic) NSDictionary * _Nonnull config;
        [Export("config")]
        NSDictionary Config { get; }

        // @property (nonatomic) JitsiMeetUserInfo * _Nullable userInfo;
        [NullAllowed, Export("userInfo", ArgumentSemantic.Assign)]
        JitsiMeetUserInfo UserInfo { get; set; }

        // -(void)setFeatureFlag:(NSString * _Nonnull)flag withBoolean:(BOOL)value;
        [Export("setFeatureFlag:withBoolean:")]
        void SetFeatureFlag(string flag, bool value);

        // -(void)setFeatureFlag:(NSString * _Nonnull)flag withValue:(id _Nonnull)value;
        [Export("setFeatureFlag:withValue:")]
        void SetFeatureFlag(string flag, NSObject value);

        // -(void)setConfigOverride:(NSString * _Nonnull)config withBoolean:(BOOL)value;
        [Export("setConfigOverride:withBoolean:")]
        void SetConfigOverride(string config, bool value);

        // -(void)setConfigOverride:(NSString * _Nonnull)config withValue:(id _Nonnull)value;
        [Export("setConfigOverride:withValue:")]
        void SetConfigOverride(string config, NSObject value);

        // -(void)setConfigOverride:(NSString * _Nonnull)config withDictionary:(NSDictionary * _Nonnull)dictionary;
        [Export("setConfigOverride:withDictionary:")]
        void SetConfigOverride(string config, NSDictionary dictionary);

        // -(void)setConfigOverride:(NSString * _Nonnull)config withArray:(NSArray * _Nonnull)array;
        [Export("setConfigOverride:withArray:")]
        void SetConfigOverride(string config, NSArray array);

        // -(void)setAudioOnly:(BOOL)audioOnly;
        [Export("setAudioOnly:")]
        void SetAudioOnly(bool audioOnly);

        // -(void)setAudioMuted:(BOOL)audioMuted;
        [Export("setAudioMuted:")]
        void SetAudioMuted(bool audioMuted);

        // -(void)setVideoMuted:(BOOL)videoMuted;
        [Export("setVideoMuted:")]
        void SetVideoMuted(bool videoMuted);

        // -(void)setCallHandle:(NSString * _Nonnull)callHandle;
        [Export("setCallHandle:")]
        void SetCallHandle(string callHandle);

        // -(void)setCallUUID:(NSUUID * _Nonnull)callUUID;
        [Export("setCallUUID:")]
        void SetCallUUID(NSUuid callUUID);

        // -(void)setSubject:(NSString * _Nonnull)subject;
        [Export("setSubject:")]
        void SetSubject(string subject);
    }

    // @interface JitsiMeetConferenceOptions : NSObject
    [BaseType(typeof(NSObject))]
    [DisableDefaultCtor]
    interface JitsiMeetConferenceOptions
    {
        // @property (readonly, copy, nonatomic) NSURL * _Nullable serverURL;
        [NullAllowed, Export("serverURL", ArgumentSemantic.Copy)]
        NSUrl ServerURL { get; }

        // @property (readonly, copy, nonatomic) NSString * _Nullable room;
        [NullAllowed, Export("room")]
        string Room { get; }

        // @property (readonly, copy, nonatomic) NSString * _Nullable token;
        [NullAllowed, Export("token")]
        string Token { get; }

        // @property (readonly, nonatomic) NSDictionary * _Nonnull featureFlags;
        [Export("featureFlags")]
        NSDictionary FeatureFlags { get; }

        // @property (nonatomic) JitsiMeetUserInfo * _Nullable userInfo;
        [NullAllowed, Export("userInfo", ArgumentSemantic.Assign)]
        JitsiMeetUserInfo UserInfo { get; set; }

        // +(instancetype _Nonnull)fromBuilder:(void (^ _Nonnull)(JitsiMeetConferenceOptionsBuilder * _Nonnull))initBlock;
        [Static]
        [Export("fromBuilder:")]
        JitsiMeetConferenceOptions FromBuilder(Action<JitsiMeetConferenceOptionsBuilder> initBlock);
    }

    // @protocol JitsiMeetViewDelegate <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface JitsiMeetViewDelegate
    {
        // @optional -(void)conferenceJoined:(NSDictionary * _Nullable)data;
        [Export("conferenceJoined:")]
        void ConferenceJoined([NullAllowed] NSDictionary data);

        // @optional -(void)conferenceTerminated:(NSDictionary * _Nullable)data;
        [Export("conferenceTerminated:")]
        void ConferenceTerminated([NullAllowed] NSDictionary data);

        // @optional -(void)conferenceWillJoin:(NSDictionary * _Nullable)data;
        [Export("conferenceWillJoin:")]
        void ConferenceWillJoin([NullAllowed] NSDictionary data);

        // @optional -(void)enterPictureInPicture:(NSDictionary * _Nullable)data;
        [Export("enterPictureInPicture:")]
        void EnterPictureInPicture([NullAllowed] NSDictionary data);

        // @optional -(void)participantJoined:(NSDictionary * _Nullable)data;
        [Export("participantJoined:")]
        void ParticipantJoined([NullAllowed] NSDictionary data);

        // @optional -(void)participantLeft:(NSDictionary * _Nullable)data;
        [Export("participantLeft:")]
        void ParticipantLeft([NullAllowed] NSDictionary data);

        // @optional -(void)audioMutedChanged:(NSDictionary * _Nullable)data;
        [Export("audioMutedChanged:")]
        void AudioMutedChanged([NullAllowed] NSDictionary data);

        // @optional -(void)endpointTextMessageReceived:(NSDictionary * _Nullable)data;
        [Export("endpointTextMessageReceived:")]
        void EndpointTextMessageReceived([NullAllowed] NSDictionary data);

        // @optional -(void)screenShareToggled:(NSDictionary * _Nullable)data;
        [Export("screenShareToggled:")]
        void ScreenShareToggled([NullAllowed] NSDictionary data);

        // @optional -(void)chatMessageReceived:(NSDictionary * _Nullable)data;
        [Export("chatMessageReceived:")]
        void ChatMessageReceived([NullAllowed] NSDictionary data);

        // @optional -(void)chatToggled:(NSDictionary * _Nullable)data;
        [Export("chatToggled:")]
        void ChatToggled([NullAllowed] NSDictionary data);

        // @optional -(void)videoMutedChanged:(NSDictionary * _Nullable)data;
        [Export("videoMutedChanged:")]
        void VideoMutedChanged([NullAllowed] NSDictionary data);

        // @optional -(void)readyToClose:(NSDictionary * _Nullable)data;
        [Export("readyToClose:")]
        void ReadyToClose([NullAllowed] NSDictionary data);

        // @optional -(void)transcriptionChunkReceived:(NSDictionary * _Nullable)data;
        [Export("transcriptionChunkReceived:")]
        void TranscriptionChunkReceived([NullAllowed] NSDictionary data);

        // @optional -(void)customButtonPressed:(NSDictionary * _Nullable)data;
        [Export("customButtonPressed:")]
        void CustomButtonPressed([NullAllowed] NSDictionary data);

        // @optional -(void)conferenceUniqueIdSet:(NSDictionary * _Nullable)data;
        [Export("conferenceUniqueIdSet:")]
        void ConferenceUniqueIdSet([NullAllowed] NSDictionary data);

        // @optional -(void)recordingStatusChanged:(NSDictionary * _Nullable)data;
        [Export("recordingStatusChanged:")]
        void RecordingStatusChanged([NullAllowed] NSDictionary data);
    }

    // @interface JitsiMeetView : UIView
    [BaseType(typeof(UIView))]
    interface JitsiMeetView
    {
        // @property (nonatomic, weak) id<JitsiMeetViewDelegate> _Nullable delegate;
        [NullAllowed, Export("delegate", ArgumentSemantic.Weak)]
        NSObject Delegate { get; set; }

        // -(void)join:(JitsiMeetConferenceOptions * _Nullable)options;
        [Export("join:")]
        void Join([NullAllowed] JitsiMeetConferenceOptions options);

        // -(void)leave;
        [Export("leave")]
        void Leave();

        // -(void)hangUp;
        [Export("hangUp")]
        void HangUp();

        // -(void)setAudioMuted:(BOOL)muted;
        [Export("setAudioMuted:")]
        void SetAudioMuted(bool muted);

        // -(void)sendEndpointTextMessage:(NSString * _Nonnull)message :(NSString * _Nullable)to;
        [Export("sendEndpointTextMessage::")]
        void SendEndpointTextMessage(string message, [NullAllowed] string to);

        // -(void)toggleScreenShare:(BOOL)enabled;
        [Export("toggleScreenShare:")]
        void ToggleScreenShare(bool enabled);

        // -(void)retrieveParticipantsInfo:(void (^ _Nonnull)(NSArray * _Nullable))completionHandler;
        [Export("retrieveParticipantsInfo:")]
        void RetrieveParticipantsInfo(Action<NSArray> completionHandler);

        // -(void)openChat:(NSString * _Nullable)to;
        [Export("openChat:")]
        void OpenChat([NullAllowed] string to);

        // -(void)closeChat;
        [Export("closeChat")]
        void CloseChat();

        // -(void)sendChatMessage:(NSString * _Nonnull)message :(NSString * _Nullable)to;
        [Export("sendChatMessage::")]
        void SendChatMessage(string message, [NullAllowed] string to);

        // -(void)setVideoMuted:(BOOL)muted;
        [Export("setVideoMuted:")]
        void SetVideoMuted(bool muted);

        // -(void)setClosedCaptionsEnabled:(BOOL)enabled;
        [Export("setClosedCaptionsEnabled:")]
        void SetClosedCaptionsEnabled(bool enabled);

        // -(void)toggleCamera;
        [Export("toggleCamera")]
        void ToggleCamera();

        // -(void)showNotification:(NSString * _Nonnull)appearance :(NSString * _Nullable)description :(NSString * _Nullable)timeout :(NSString * _Nullable)title :(NSString * _Nullable)uid;
        [Export("showNotification:::::")]
        void ShowNotification(string appearance, [NullAllowed] string description, [NullAllowed] string timeout, [NullAllowed] string title, [NullAllowed] string uid);

        // -(void)hideNotification:(NSString * _Nullable)uid;
        [Export("hideNotification:")]
        void HideNotification([NullAllowed] string uid);

        // -(void)startRecording:(RecordingMode)mode :(NSString * _Nullable)dropboxToken :(BOOL)shouldShare :(NSString * _Nullable)rtmpStreamKey :(NSString * _Nullable)rtmpBroadcastID :(NSString * _Nullable)youtubeStreamKey :(NSString * _Nullable)youtubeBroadcastID :(NSDictionary * _Nullable)extraMetadata :(BOOL)transcription;
        [Export("startRecording:::::::::")]
        void StartRecording(RecordingMode mode, [NullAllowed] string dropboxToken, bool shouldShare, [NullAllowed] string rtmpStreamKey, [NullAllowed] string rtmpBroadcastID, [NullAllowed] string youtubeStreamKey, [NullAllowed] string youtubeBroadcastID, [NullAllowed] NSDictionary extraMetadata, bool transcription);

        // -(void)stopRecording:(RecordingMode)mode :(BOOL)transcription;
        [Export("stopRecording::")]
        void StopRecording(RecordingMode mode, bool transcription);

        // -(void)overwriteConfig:(NSDictionary * _Nonnull)config;
        [Export("overwriteConfig:")]
        void OverwriteConfig(NSDictionary config);

        // -(void)sendCameraFacingModeMessage:(NSString * _Nonnull)to :(NSString * _Nullable)facingMode;
        [Export("sendCameraFacingModeMessage::")]
        void SendCameraFacingModeMessage(string to, [NullAllowed] string facingMode);
    }

    // @interface JitsiMeet : NSObject
    [BaseType(typeof(NSObject), Name = "JitsiMeet")]
    interface JitsiMeetSDK
    {
        // @property (copy, nonatomic) NSString * _Nullable conferenceActivityType;
        [NullAllowed, Export("conferenceActivityType")]
        string ConferenceActivityType { get; set; }

        // @property (copy, nonatomic) NSString * _Nullable customUrlScheme;
        [NullAllowed, Export("customUrlScheme")]
        string CustomUrlScheme { get; set; }

        // @property (copy, nonatomic) NSArray<NSString *> * _Nullable universalLinkDomains;
        [NullAllowed, Export("universalLinkDomains", ArgumentSemantic.Copy)]
        string[] UniversalLinkDomains { get; set; }

        // @property (nonatomic) JitsiMeetConferenceOptions * _Nullable defaultConferenceOptions;
        [NullAllowed, Export("defaultConferenceOptions", ArgumentSemantic.Assign)]
        JitsiMeetConferenceOptions DefaultConferenceOptions { get; set; }

        // @property (nonatomic, strong) id _Nullable rtcAudioDevice;
        [NullAllowed, Export("rtcAudioDevice", ArgumentSemantic.Strong)]
        NSObject RtcAudioDevice { get; set; }

        // @property (assign, nonatomic) WebRTCLoggingSeverity webRtcLoggingSeverity;
        [Export("webRtcLoggingSeverity", ArgumentSemantic.Assign)]
        WebRTCLoggingSeverity WebRtcLoggingSeverity { get; set; }

        // +(instancetype _Nonnull)sharedInstance;
        [Static]
        [Export("sharedInstance")]
        JitsiMeetSDK SharedInstance { get; }

        // -(BOOL)application:(UIApplication * _Nonnull)application didFinishLaunchingWithOptions:(NSDictionary * _Nonnull)launchOptions;
        [Export("application:didFinishLaunchingWithOptions:")]
        bool Application(UIApplication application, NSDictionary launchOptions);

        // -(BOOL)application:(UIApplication * _Nonnull)application continueUserActivity:(NSUserActivity * _Nonnull)userActivity restorationHandler:(void (^ _Nullable)(NSArray<id<UIUserActivityRestoring>> * _Nonnull))restorationHandler;
        [Export("application:continueUserActivity:restorationHandler:")]
        bool Application(UIApplication application, NSUserActivity userActivity, [NullAllowed] Action<NSArray> restorationHandler);

        // -(BOOL)application:(UIApplication * _Nonnull)app openURL:(NSURL * _Nonnull)url options:(NSDictionary<NSString *,id> * _Nonnull)options;
        [Export("application:openURL:options:")]
        bool Application(UIApplication app, NSUrl url, NSDictionary options);

        // -(UIInterfaceOrientationMask)application:(UIApplication * _Nonnull)application supportedInterfaceOrientationsForWindow:(UIWindow * _Nullable)window;
        [Export("application:supportedInterfaceOrientationsForWindow:")]
        UIInterfaceOrientationMask Application(UIApplication application, [NullAllowed] UIWindow window);

        // -(void)instantiateReactNativeBridge;
        [Export("instantiateReactNativeBridge")]
        void InstantiateReactNativeBridge();

        // -(void)destroyReactNativeBridge;
        [Export("destroyReactNativeBridge")]
        void DestroyReactNativeBridge();

        // -(JitsiMeetConferenceOptions * _Nonnull)getInitialConferenceOptions;
        [Export("getInitialConferenceOptions")]
        JitsiMeetConferenceOptions GetInitialConferenceOptions();

        // -(BOOL)isCrashReportingDisabled;
        [Export("isCrashReportingDisabled")]
        bool IsCrashReportingDisabled();

        // -(void)showSplashScreen;
        [Export("showSplashScreen")]
        void ShowSplashScreen();
    }
}
