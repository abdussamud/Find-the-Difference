using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public List<ClickableObject> clickableObjects;
    public float health = 100, maxHealth = 100;


    private void Awake()
    {
        Instance = this;
    }
}
