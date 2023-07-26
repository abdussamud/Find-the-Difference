using System;
using UnityEngine;

[Serializable]
public class Level
{
    public string levelName;
    public ClickableObject[] upperSimilar;
    public ClickableObject[] lowerSimilar;
    public ClickableObject[] different;
    public GameObject levelEnvironment;
}
