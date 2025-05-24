using UnityEngine;

public class GameOverUISelector : MonoBehaviour
{
    public GameObject shootGameUI;
    public GameObject obstacleCourseUI;
    public GameObject puzzleUI;

    void Start()
    {
        string prevScene = SceneTracker.Instance != null ? SceneTracker.Instance.PreviousScene : "";

        if (prevScene == "ShooterScene")
            shootGameUI.SetActive(true);
        else if (prevScene == "ObstacleCourseScene")
            obstacleCourseUI.SetActive(true);
        else if (prevScene == "PuzzleScene")
            puzzleUI.SetActive(true);
    }
}
