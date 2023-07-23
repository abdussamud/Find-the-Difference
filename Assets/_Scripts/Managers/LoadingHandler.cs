using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingHandler : MonoBehaviour
{
    public float loadDuration;
    [SerializeField] private string sceneName;
    [SerializeField] private Image loadingImage;


    private void Start()
    {
        sceneName = GameManager.Instance.nextScene;
        if (sceneName == string.Empty)
        {
            sceneName = "Main Menu";
        }
        _ = StartCoroutine(StartLoading());
    }

    private IEnumerator StartLoading()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        yield return FillRadial(loadDuration, false);

        yield return FillRadial(loadDuration, true);

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
