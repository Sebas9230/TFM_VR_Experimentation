using UnityEngine;
using UnityEngine.SceneManagement;

public class HatStandTrigger : MonoBehaviour
{
    [Header("Nombre correcto del sombrero para este stand")]
    public string correctHatName;

    private static int correctHatsPlaced = 0;
    private static int totalCorrect = 5;

    private bool hatAlreadyPlaced = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hatAlreadyPlaced && other.CompareTag("Hat"))
        {
            if (other.name.Contains(correctHatName))
            {
                hatAlreadyPlaced = true;
                correctHatsPlaced++;
                Debug.Log($"✅ Sombrero correcto '{other.name}' en el stand de '{correctHatName}'. Total correctos: {correctHatsPlaced}");

                if (correctHatsPlaced >= totalCorrect)
                {
                    Debug.Log("🎉 ¡Todos los sombreros están en el lugar correcto!");
                    SceneManager.LoadScene("GameOver_Puzzle");
                }
            }
            else
            {
                Debug.LogWarning($"❌ Sombrero incorrecto '{other.name}' en el stand de '{correctHatName}'");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (hatAlreadyPlaced && other.CompareTag("Hat") && other.name.Contains(correctHatName))
        {
            hatAlreadyPlaced = false;
            correctHatsPlaced--;
            Debug.Log($"🔄 Sombrero '{other.name}' removido del stand de '{correctHatName}'. Total correctos: {correctHatsPlaced}");
        }
    }
}
