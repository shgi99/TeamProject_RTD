using UnityEditor;
using UnityEngine;

namespace MagicPigGames
{
    public class SupportFilesChecker : EditorWindow
    {
        private const string FileName = "InfinityEditor.cs"; // Full filename including extension
        private const string SessionPrefKey = "SupportFilesChecker_FileFound";
        private static bool _windowOpened = false; // To ensure only one window opens
        private static bool _initialized = false; // Prevent multiple initializations

        [InitializeOnLoadMethod]
        private static void CheckSupportFiles()
        {
            // Ensure this method runs only once per editor session
            if (_initialized) return;
            _initialized = true;

            
            // If the session preference indicates the file was already found, skip the check
            if (SessionState.GetBool(SessionPrefKey, false)) return;

            // Search the project for the file
            if (AssetDatabase.FindAssets(FileName.Replace(".cs", "")).Length > 0)
            {
                // Mark as found in the current session
                SessionState.SetBool(SessionPrefKey, true);
                return;
            }
            

            // If not found and window not already opened, open the popup window
            if (!_windowOpened)
            {
                _windowOpened = true; // Prevent further windows from being created
                OpenSupportFilesNotice();
            }
        }

        private static void OpenSupportFilesNotice()
        {
            Debug.LogError("Opening Support Files Notice window.");
            var window = CreateInstance<SupportFilesChecker>();
            window.titleContent = new GUIContent("Support Files Notice");
            window.ShowUtility();
        }

        private void OnDestroy()
        {
            _windowOpened = false; // Reset the flag when the window is closed
        }

        private void OnGUI()
        {
            GUILayout.Label("Install the Support Files", EditorStyles.boldLabel);

            // Style for wrapping text
            GUIStyle wrapStyle = new GUIStyle(GUI.skin.label)
            {
                wordWrap = true
            };

            // Wrapped text
            GUILayout.Label(
                "The support files have the shared files for the Magic Pig Games packages. Please install it from the Package Manager.\n\n" +
                "If you do not have it yet, you can get it (free) on the Asset Store, and then install it from the Package Manager.",
                wrapStyle
            );

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Close"))
            {
                this.Close();
            }

            if (GUILayout.Button("View in Asset Store"))
            {
                Application.OpenURL("https://assetstore.unity.com/packages/tools/utilities/infinity-pbr-support-files-160921?aid=1100lxWw");
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}