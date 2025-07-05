using UnityEngine;

public class SceneTracker : MonoBehaviour
{
    public static SceneTracker Instance;
    public string PreviousScene;
    
    // Game results storage
    [Header("Game Results")]
    public int shooterScore = 0;
    public float obstacleCourseTime = 0f;
    public float obstacleCourseSuccessRate = 0f;
    public float puzzleTime = 0f;
    public float puzzleAccuracy = 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persistente entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Methods to store game results
    public void SetShooterResults(int score)
    {
        shooterScore = score;
    }
    
    public void SetObstacleCourseResults(float time, float successRate)
    {
        obstacleCourseTime = time;
        obstacleCourseSuccessRate = successRate;
    }
    
    public void SetPuzzleResults(float time, float accuracy)
    {
        puzzleTime = time;
        puzzleAccuracy = accuracy;
    }
}
