using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public Sprite icon;
    public string title;
    [TextArea] public string[] sentences;
    public float autoHideInSeconds;
}
