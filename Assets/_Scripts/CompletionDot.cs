using UnityEngine;

public class CompletionDot : MonoBehaviour
{
    public int dotID;
    public GameObject innerGO;

    private void Start()
    {
        if (GameController.Instance.totalDifferences <= dotID)
        {
            gameObject.SetActive(false);
        }
    }
}
