using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.Instance.nextScene = "Gameplay";
        GameManager.Instance.loadingDuration = 2.5f;
        SceneManager.LoadScene("Loading");
    }
}
