using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private GameManager Gm => GameManager.Instance;
    public void StartGame()
    {
        Gm.nextScene = "Gameplay";
        Gm.loadingDuration = 2.5f;
        SceneManager.LoadScene("Loading");
    }
}
