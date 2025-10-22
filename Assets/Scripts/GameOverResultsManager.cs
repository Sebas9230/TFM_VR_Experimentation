using UnityEngine;
using TMPro;

public class GameOverResultsManager : MonoBehaviour
{
    [Header("UI Configuration")]
    [Tooltip("TextMesh component where results will be displayed")]
    public TextMeshProUGUI resultText;
    
    // Initializes the results display system
    void Start()
    {
        // If no TextMesh is assigned, try to find one in the same GameObject
        if (resultText == null)
        {
            resultText = GetComponent<TextMeshProUGUI>();
        }
        
        if (resultText == null)
        {
            Debug.LogError("GameOverResultsManager: No TextMeshProUGUI found. Assign one in the inspector or place this script on a GameObject with TextMeshProUGUI.");
            return;
        }
        
        DisplayResults();
    }
    
    // Displays game results based on the previous scene
    void DisplayResults()
    {
        if (SceneTracker.Instance == null)
        {
            resultText.text = "Error: Could not get information from previous scene";
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
                results = $"Unrecognized scene: {previousScene}";
                break;
        }
        
        resultText.text = results;
        Debug.Log($"GameOverResultsManager: Showing results for {previousScene} - {results}");
    }
} 