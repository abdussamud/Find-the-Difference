﻿using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public string nextScene;

    private void Awake()
    {
        Instance = this;
    }
}