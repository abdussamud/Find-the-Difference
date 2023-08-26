using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private float loadingDuration;
    [SerializeField] private string sceneName;
    [SerializeField] private Image loadingImage;
    private GameManager Gm => GameManager.Instance;

    private void Start()
    {
        sceneName = Gm.nextScene;
        loadingDuration = Gm.loadingDuration;
        if (loadingDuration == 0) { loadingDuration = 5f; }
        if (sceneName == string.Empty) { sceneName = "Main Menu"; }
        _ = StartCoroutine(StartLoading());
    }

    private IEnumerator StartLoading()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        yield return FillRadial(loadingDuration, false);

        yield return FillRadial(loadingDuration, true);

        operation.allowSceneActivation = true;
    }

    private IEnumerator FillRadial(float duration, bool clockwise)
    {
        float elapsedTime = 0;
        float startFillAmount = clockwise ? 1 : 0;
        float endFillAmount = clockwise ? 0 : 1;
        loadingImage.fillClockwise = clockwise;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            loadingImage.fillAmount = Mathf.Lerp(startFillAmount, endFillAmount, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        loadingImage.fillAmount = endFillAmount;
    }
}
