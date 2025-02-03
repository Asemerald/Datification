using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Agora.Rtc;

#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
using UnityEngine.Android;
#endif

public class JoinChannelVideo : MonoBehaviour
{
    // Fill in your app ID
    private string _appID= "586b77717e4649d6aebeb9d4cb43d8da";
    // Fill in your channel name
    private string _channelName = "Datopia";
    // Fill in your Token
    private string _token = "007eJxTYLDgVb0tK/JeeOev6p67Vz8muDhEXND98sX5pIjonfZQL0UFBlMLsyRzc3ND81QTMxPLFLPE1KTUJMsUk+QkE+MUi5TEw+sXpDcEMjJcb5/DwsgAgSA+O4NLYkl+QWYiAwMAvi8hrA==";
    internal VideoSurface LocalView;
    internal VideoSurface RemoteView;
    internal IRtcEngine RtcEngine;

    #if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
    private ArrayList permissionList = new ArrayList() { Permission.Camera, Permission.Microphone };
    #endif

    void Start()
    {
        SetupVideoSDKEngine();
        InitEventHandler();
        SetupUI();
        PreviewSelf();
    }

    void Update()
    {
        CheckPermissions();
    }

    void OnApplicationQuit()
    {
        if (RtcEngine != null)
        {
            Leave();
            // Destroy IRtcEngine
            RtcEngine.Dispose();
            RtcEngine = null;
        }
    }

    private void CheckPermissions() {
    #if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
            foreach (string permission in permissionList)
            {
                if (!Permission.HasUserAuthorizedPermission(permission))
                {
                    Permission.RequestUserPermission(permission);
                }
            }
    #endif
    }

    private void PreviewSelf()
    {
        // Enable video module
        RtcEngine.EnableVideo();
        // Start local video preview
        RtcEngine.StartPreview();
        // Set local video display
        LocalView.SetForUser(0, "");
        // Start rendering video
        LocalView.SetEnable(true);
    }

    private void SetupUI()
    {
        GameObject go = GameObject.Find("LocalView");
        LocalView = go.AddComponent<VideoSurface>();
        go.transform.Rotate(0.0f, 0.0f, -180.0f);
        go = GameObject.Find("RemoteView");
        RemoteView = go.AddComponent<VideoSurface>();
        go.transform.Rotate(0.0f, 0.0f, -180.0f);
        go = GameObject.Find("Leave");
        go.GetComponent<Button>().onClick.AddListener(Leave);
        go = GameObject.Find("Join");
        go.GetComponent<Button>().onClick.AddListener(Join);
        Debug.Log("Ui setup done");
    }

    private void SetupVideoSDKEngine()
    {
        // Create IRtcEngine instance
        RtcEngine = Agora.Rtc.RtcEngine.CreateAgoraRtcEngine();
        RtcEngineContext context = new RtcEngineContext();
            context.appId = _appID;
            context.channelProfile = CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_LIVE_BROADCASTING;
            context.audioScenario = AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_DEFAULT;
        // Initialize IRtcEngine
        RtcEngine.Initialize(context);
    }

    // Create an instance of the user callback class and set the callback
    private void InitEventHandler()
    {
        UserEventHandler handler = new UserEventHandler(this);
        RtcEngine.InitEventHandler(handler);
    }

    public void Join()
    {
        // Set channel media options
        ChannelMediaOptions options = new ChannelMediaOptions();
        // Start video rendering
        LocalView.SetEnable(true);
        // Publish microphone audio stream
        options.publishMicrophoneTrack.SetValue(true);
        // Publish camera video stream
        options.publishCameraTrack.SetValue(true);
        // Automatically subscribe to all audio streams
        options.autoSubscribeAudio.SetValue(true);
        // Automatically subscribe to all video streams
        options.autoSubscribeVideo.SetValue(true);
        // Set the channel profile to live broadcasting
        options.channelProfile.SetValue(CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_LIVE_BROADCASTING);
        // Set the user role to broadcaster
        options.clientRoleType.SetValue(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        // Join the channel
        RtcEngine.JoinChannel(_token, _channelName, 0, options);
    }

    public void Leave()
    {
        Debug.Log("Leaving _channelName");
        // Disable video module
        RtcEngine.StopPreview();
        // Leave the channel
        RtcEngine.LeaveChannel();
        // Stop remote video rendering
        RemoteView.SetEnable(false);
    }

    // Implement your own callback class by inheriting from the IRtcEngineEventHandler interface class
    internal class UserEventHandler : IRtcEngineEventHandler
    {
        private readonly JoinChannelVideo _videoSample;

        internal UserEventHandler(JoinChannelVideo videoSample)
        {
            _videoSample = videoSample;
        }

        // Callback triggered when an error occurs
        public override void OnError(int err, string msg)
        {
        }

        // Callback triggered when the local user successfully joins the channel
        public override void OnJoinChannelSuccess(RtcConnection connection, int elapsed)
        {
        }

        // OnUserJoined callback is triggered when the SDK receives and successfully decodes the first frame of remote video
        public override void OnUserJoined(RtcConnection connection, uint uid, int elapsed)
        {
            // Set remote video display
            _videoSample.RemoteView.SetForUser(uid, connection.channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
            // Start video rendering
            _videoSample.RemoteView.SetEnable(true);
            Debug.Log("Remote user joined");
        }

        // Callback triggered when a remote user leaves the current channel
        public override void OnUserOffline(RtcConnection connection, uint uid, USER_OFFLINE_REASON_TYPE reason)
        {
            _videoSample.RemoteView.SetEnable(false);
            Debug.Log("Remote user offline");
        }
    }
}