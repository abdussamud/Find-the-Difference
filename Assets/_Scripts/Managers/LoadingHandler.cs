using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingHandler : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private Slider loadingslider;


    private void Start()
    {
        sceneName = GameManager.Instance.nextScene;
        if (sceneName == string.Empty )
        {
            sceneName = "Main Menu";
        }
        //_ = loadingSlider.DoValue(1, 3);
        _ = StartCoroutine(StartLoading());
    }

    private IEnumerator StartLoading()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        yield return new WaitForSeconds(3);
        operation.allowSceneActivation = true;
        yield return null;
    }
}
