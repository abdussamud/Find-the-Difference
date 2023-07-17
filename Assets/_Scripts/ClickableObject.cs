using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    public GameObject rightSelection;
    public GameObject wrongSelection;
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
                obj.TurnOn(obj.rightSelection);
                TurnOn(rightSelection);
                return;
            }
        }
        TurnOn(wrongSelection);
    }

    private void TurnOff()
    {
        rightSelection.SetActive(false);
        wrongSelection.SetActive(false);
    }

    public void TurnOn(GameObject obj)
    {
        obj.SetActive(true);
        Invoke(nameof(TurnOff), 0.7f);
    }
}
