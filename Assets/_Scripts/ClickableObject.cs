using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    public bool isFound;
    [SerializeField] private int objectID;
    private GameController gameController;


    private void Start()
    {
        gameController = GameController.Instance;
        gameController.clickableObjects.Add(this);
    }

    private void OnMouseDown()
    {
        foreach (ClickableObject obj in gameController.clickableObjects)
        {
            if (obj.objectID == objectID && obj != this)
            {
                GameController.Instance.health -= 10;
                GameplayUI.Instance.SetHealthBar(GameController.Instance.health);
                Destroy(Instantiate(GameController.Instance.crossGO, transform), 0.7f);
                return;
            }
        }
        if (!isFound)
        {
            isFound = true;
            GameController.Instance.findDifferences++;
            Destroy(Instantiate(GameController.Instance.circleGO, transform), 1.2f);
            Destroy(Instantiate(GameController.Instance.circleGO, new Vector3(transform.position.x,
                transform.position.y * -1, transform.position.z),
                Quaternion.identity), 1.2f);
            Invoke(nameof(ActiavateCompletionDot), 0.5f);
            if (GameController.Instance.findDifferences >=
                GameController.Instance.totalDifferences)
            {
                GameplayUI.Instance.GameWon();
            }
        }
    }

    private void ActiavateCompletionDot()
    {
        GameplayUI.Instance.completionDot[GameController.Instance.findDifferences
            - 1].innerGO.SetActive(true);
    }
}
