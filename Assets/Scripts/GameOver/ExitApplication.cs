using UnityEngine;

public class ExitAppButton : MonoBehaviour
{
    // This method can be assigned to the OnClick event of the button
    public void ExitApplication()
    {
        // Close the application in a build
        Application.Quit();

        // This log appears only in the editor to confirm the closure
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
