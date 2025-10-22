using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class EndGameTeleport : MonoBehaviour
{
    public string sceneToLoad = "";
    private bool isTeleporting = false;

    private void OnTriggerEnter(Collider other)
    {
        // Make sure the object entering is the XR Rig or a VR Controller
        if ((other.CompareTag("Player") || other.GetComponent<XRController>()) && !isTeleporting)
        {
            // Stop timer and log completion
            Cronometer cronometer = FindObjectOfType<Cronometer>();
            if (cronometer != null)
            {
                cronometer.DetenerCronometro();
            }
            
            if (!string.IsNullOrEmpty(sceneToLoad))
            {
                StartCoroutine(TeleportAfterDelay(1f)); // Wait 1 second before teleporting
            }
        }
    }

// Teleport to the specified scene after a delay
private IEnumerator TeleportAfterDelay(float delay)
{
    isTeleporting = true;
    yield return new WaitForSeconds(delay);

    if (SceneTracker.Instance != null)
    {
        SceneTracker.Instance.PreviousScene = SceneManager.GetActiveScene().name;
    }

    SceneManager.LoadScene(sceneToLoad);
}

}
