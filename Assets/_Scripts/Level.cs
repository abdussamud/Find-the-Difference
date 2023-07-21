using System;
[Serializable]
public class Level
{
    public string levelName;
    public ClickableObject[] upperSimilar;
    public ClickableObject[] lowerSimilar;
    public ClickableObject[] different;
}
