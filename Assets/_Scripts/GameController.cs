using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public List<ClickableObject> clickableObjects;


    private void Awake()
    {
        Instance = this;
    }
}
