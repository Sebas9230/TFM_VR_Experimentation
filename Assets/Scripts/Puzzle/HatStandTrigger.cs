using UnityEngine;

public class HatStandTrigger : MonoBehaviour
{
    [Header("Nombre correcto del sombrero para este stand")]
    public string correctHatName;

    private static int correctHatsPlaced = 0;

    private bool hatAlreadyPlaced = false;

    private void OnTriggerEnter(Collider other)
    {
        // Validar que el stand esté correctamente configurado
        if (string.IsNullOrEmpty(correctHatName))
        {
            Debug.LogWarning($"HatStandTrigger en '{gameObject.name}' no tiene correctHatName configurado. Ignorando trigger.");
            return;
        }
        
        if (!hatAlreadyPlaced && other.CompareTag("Hat"))
        {
            if (other.name == correctHatName)
            {
                hatAlreadyPlaced = true;
                correctHatsPlaced++;
                Debug.Log($"Sombrero correcto '{other.name}' en el stand de '{correctHatName}'. Total correctos: {correctHatsPlaced}");
                
                // Registrar en logs
                if (PuzzleLogsManager.Instance != null)
                {
                    PuzzleLogsManager.Instance.RegistrarColocacionCorrecta(other.name, correctHatName);
                }
            }
            else
            {
                Debug.LogWarning($"Sombrero incorrecto '{other.name}' en el stand de '{correctHatName}' (GameObject: {gameObject.name})");
                
                // Registrar en logs solo si no es el sombrero correcto para ESTE stand específico
                if (PuzzleLogsManager.Instance != null)
                {
                    PuzzleLogsManager.Instance.RegistrarColocacionIncorrecta(other.name, correctHatName);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Validar que el stand esté correctamente configurado
        if (string.IsNullOrEmpty(correctHatName))
        {
            return;
        }
        
        if (hatAlreadyPlaced && other.CompareTag("Hat") && other.name == correctHatName)
        {
            hatAlreadyPlaced = false;
            correctHatsPlaced--;
            Debug.Log($"Sombrero '{other.name}' removido del stand de '{correctHatName}'. Total correctos: {correctHatsPlaced}");
            
            // Registrar en logs
            if (PuzzleLogsManager.Instance != null)
            {
                PuzzleLogsManager.Instance.RegistrarRemoverSombrero(other.name, correctHatName);
            }
        }
    }


}
