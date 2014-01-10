using UnityEngine;
using System.Collections;

[System.Serializable]
public class Dialogue
{
    #region public variables
    
    public Texture2D portrait = null;
    public string nameText = "Test";
    public string textLine1 = "Line 1";
    public string textLine2 = "Line 2";
    public AudioClip sound = null;
    public Transform cameraTarget = null;
    public Activator activator = new Activator();
    
    #endregion
}
