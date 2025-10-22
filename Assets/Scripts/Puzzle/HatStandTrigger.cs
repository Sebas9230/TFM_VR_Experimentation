using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class HatStandTrigger : MonoBehaviour
{
    [Header("Correct hat name for this stand")]
    public string correctHatName;

    private static int correctHatsPlaced = 0;
    private static int totalCorrect = 5;

    private bool hatAlreadyPlaced = false;
    private static bool gameFinished = false;

    // Handles hat placement on stand
    private void OnTriggerEnter(Collider other)
    {
        if (!hatAlreadyPlaced && other.CompareTag("Hat"))
        {
            if (other.name == correctHatName)
            {
                hatAlreadyPlaced = true;
                correctHatsPlaced++;
                Debug.Log($"✅ Correct hat '{other.name}' placed on '{correctHatName}' stand. Total correct: {correctHatsPlaced}");

                // Update hat count text
                if (CronometerScore.Instance != null)
                    CronometerScore.Instance.ActualizarSombreros(correctHatsPlaced);

                if (correctHatsPlaced >= totalCorrect && !gameFinished)
                {
                    gameFinished = true;
                    Debug.Log("🎉 All hats are in the correct place!");
                    StartCoroutine(DelayAndLoadScene());
                }
            }
            else
            {
                Debug.LogWarning($"❌ Incorrect hat '{other.name}' placed on '{correctHatName}' stand");
            }
        }
    }

    // Handles hat removal from stand
    private void OnTriggerExit(Collider other)
    {
        if (hatAlreadyPlaced && other.CompareTag("Hat") && other.name == correctHatName)
        {
            hatAlreadyPlaced = false;
            correctHatsPlaced--;
            Debug.Log($"🔄 Hat '{other.name}' removed from '{correctHatName}' stand. Total correct: {correctHatsPlaced}");

            // Update hat count text
            if (CronometerScore.Instance != null)
                CronometerScore.Instance.ActualizarSombreros(correctHatsPlaced);
        }
    }

    // Delays scene transition after puzzle completion
    private IEnumerator DelayAndLoadScene()
    {
        yield return new WaitForSeconds(2f);
        if (SceneTracker.Instance != null)
        {
            SceneTracker.Instance.PreviousScene = SceneManager.GetActiveScene().name;
        }
        SceneManager.LoadScene("GameOverScene");
    }
}
