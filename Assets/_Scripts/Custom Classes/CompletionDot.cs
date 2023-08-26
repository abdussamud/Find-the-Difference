using UnityEngine;

public class CompletionDot : MonoBehaviour
{
    public int dotID;
    public GameObject innerGO;
    private GameController Gc => GameController.gc;

    private void Start()
    {
        if (Gc.totalDifferences <= dotID)
        {
            gameObject.SetActive(false);
        }
    }
}
