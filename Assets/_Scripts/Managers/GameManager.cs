using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public string nextScene;
    public int level;
    public float loadingDuration;

    private void Awake()
    {
        Instance = this;
        level = PlayerPrefs.GetInt("level");
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
