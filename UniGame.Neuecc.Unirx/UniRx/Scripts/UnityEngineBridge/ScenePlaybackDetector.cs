#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.Callbacks;

namespace UniRx
{
    [InitializeOnLoad]
    public class ScenePlaybackDetector
    {
        private static bool _isPlaying = false;

        private static bool AboutToStartScene
        {
            get => EditorPrefs.GetBool("AboutToStartScene");
            set => EditorPrefs.SetBool("AboutToStartScene", value);
        }

        public static bool IsPlaying
        {
            get => _isPlaying;
            set => _isPlaying = value;
        }

        // This callback is notified after scripts have been reloaded.
        [DidReloadScripts]
        public static void OnDidReloadScripts()
        {
            // Filter DidReloadScripts callbacks to the moment where playmodeState transitions into isPlaying.
            if (AboutToStartScene)
            {
                IsPlaying = true;
            }
        }

        // InitializeOnLoad ensures that this constructor is called when the Unity Editor is started.
        static ScenePlaybackDetector()
        {
            IsPlaying = EditorApplication.isPlaying;
            
            EditorApplication.playModeStateChanged += e =>
            {
                switch (e)
                {
                    case PlayModeStateChange.EnteredEditMode:
                    case PlayModeStateChange.ExitingEditMode:
                    case PlayModeStateChange.ExitingPlayMode:
                        IsPlaying = false;
                        break;
                    case PlayModeStateChange.EnteredPlayMode:
                        IsPlaying = true;
                        break;
                }
                // Before scene start:          isPlayingOrWillChangePlaymode = false;  isPlaying = false
                // Pressed Playback button:     isPlayingOrWillChangePlaymode = true;   isPlaying = false
                // Playing:                     isPlayingOrWillChangePlaymode = false;  isPlaying = true
                // Pressed stop button:         isPlayingOrWillChangePlaymode = true;   isPlaying = true
                if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
                {
                    AboutToStartScene = true;
                }
                else
                {
                    AboutToStartScene = false;
                }
            };
        }
    }
}

#endif