using UnityEngine;
using TMPro;

public class GameOverResultsManager : MonoBehaviour
{
    [Header("UI Configuration")]
    [Tooltip("TextMesh component donde se mostrarán los resultados")]
    public TextMeshProUGUI resultText;
    
    void Start()
    {
        // Si no se asigna un TextMesh, intentar encontrarlo en el mismo GameObject
        if (resultText == null)
        {
            resultText = GetComponent<TextMeshProUGUI>();
        }
        
        if (resultText == null)
        {
            Debug.LogError("GameOverResultsManager: No se encontró un TextMeshProUGUI. Asigna uno en el inspector o coloca este script en un GameObject con TextMeshProUGUI.");
            return;
        }
        
        DisplayResults();
    }
    
    void DisplayResults()
    {
        if (SceneTracker.Instance == null)
        {
            resultText.text = "Error: No se pudo obtener información de la escena anterior";
            return;
        }
        
        string previousScene = SceneTracker.Instance.PreviousScene;
        string results = "";
        
        switch (previousScene)
        {
            case "ShooterScene":
                results = $"Score: {SceneTracker.Instance.shooterScore}";
                break;
                
            case "ObstacleCourseScene":
                int minutes = Mathf.FloorToInt(SceneTracker.Instance.obstacleCourseTime / 60F);
                int seconds = Mathf.FloorToInt(SceneTracker.Instance.obstacleCourseTime % 60F);
                results = $"Time: {minutes:00}:{seconds:00}      Success rate: {SceneTracker.Instance.obstacleCourseSuccessRate:F1}%";
                break;
                
            case "PuzzleScene":
                int puzzleMinutes = Mathf.FloorToInt(SceneTracker.Instance.puzzleTime / 60F);
                int puzzleSeconds = Mathf.FloorToInt(SceneTracker.Instance.puzzleTime % 60F);
                results = $"Time: {puzzleMinutes:00}:{puzzleSeconds:00}        Accuracy: {SceneTracker.Instance.puzzleAccuracy:F1}%";
                break;
                
            default:
                results = $"Escena no reconocida: {previousScene}";
                break;
        }
        
        resultText.text = results;
        Debug.Log($"GameOverResultsManager: Mostrando resultados para {previousScene} - {results}");
    }
} 