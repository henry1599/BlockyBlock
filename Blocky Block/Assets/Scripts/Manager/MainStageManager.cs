using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using BlockyBlock.Utils;
using BlockyBlock.Events;
using BlockyBlock.Enums;
using DG.Tweening;

namespace BlockyBlock.Managers
{
    public class MainStageManager : MonoBehaviour
    {
        public float Delay;
        public VideoClip Clip;
        VideoPlayer videoPlayer;
        void Start()
        {
            // Will attach a VideoPlayer to the main camera.
            GameObject camera = GameObject.Find("Main Camera");

            // VideoPlayer automatically targets the camera backplane when it is added
            // to a camera object, no need to change videoPlayer.targetCamera.
            videoPlayer = camera.AddComponent<UnityEngine.Video.VideoPlayer>();

            // Play on awake defaults to true. Set it to false to avoid the url set
            // below to auto-start playback since we're in Start().
            videoPlayer.playOnAwake = false;
            videoPlayer.isLooping = false;

            // By default, VideoPlayers added to a camera will use the far plane.
            // Let's target the near plane instead.
            videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;

            // This will cause our Scene to be visible through the video being played.
            // videoPlayer.targetCameraAlpha = 0.5F;

            // Set the video to play. URL supports local absolute or relative paths.
            // Here, using absolute.
            videoPlayer.clip = Clip;

            // Skip the first 100 frames.
            videoPlayer.frame = 100;

            // Restart from beginning when done.
            videoPlayer.isLooping = true;
            videoPlayer.loopPointReached += HandleEndVideo;

            Logo.ON_LOGO_FINISHED += HandleLogoFinish;
        }
        void OnDestroy()
        {
            Logo.ON_LOGO_FINISHED -= HandleLogoFinish;
            videoPlayer.loopPointReached -= HandleEndVideo;
        }
        void HandleEndVideo(VideoPlayer vp)
        {
            GameManager.Instance.TransitionIn(() => 
                {
                    GameEvents.LOAD_LEVEL?.Invoke(LevelID.HOME);
                }
            );
            vp.Pause();
        }
        void HandleLogoFinish()
        {
            DOVirtual.DelayedCall(Delay, () => videoPlayer.Play());
        }
    }
}
