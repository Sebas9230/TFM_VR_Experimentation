using UnityEngine;

public class SceneTracker : MonoBehaviour
{
    public static SceneTracker Instance;
    public string PreviousScene;

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
}
