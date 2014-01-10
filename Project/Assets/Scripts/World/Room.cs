using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour 
{

    #region public variables

    public Renderer[] darkness = new Renderer[0];
    public float fadeTime = 1;
    public bool autoLighten = false;

    #endregion

    #region protected variables

    protected bool isLit = false;
    protected float alpha = 1;
    protected float animVelocity;

    #endregion

    #region public methods

    public void Lighten()
    {
        Debug.Log("Lights on: " + name);

        for(int i = 0; i < darkness.Length; i++)
            darkness[i].enabled = true;

        isLit = true;
    }

    public void Darken()
    {
        for(int i = 0; i < darkness.Length; i++)
            darkness[i].enabled = true;

        isLit = false;
    }

    #endregion

	// Use this for initialization
	void Start () 
    {
        if (autoLighten)
            Lighten();
        else
        {
            //ensure darkness is on
            for (int i = 0; i < darkness.Length; i++)
                darkness [i].enabled = true;
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (isLit)
        {
            alpha = Mathf.SmoothDamp(alpha, 0, ref animVelocity, fadeTime);
            if(alpha == 0)
            {
                for(int i = 0; i < darkness.Length; i++)
                    darkness[i].enabled = false;
            }
        } else
        {
            alpha = Mathf.SmoothDamp(alpha, 1, ref animVelocity, fadeTime);
        }

        Color c;
        for(int i = 0; i < darkness.Length; i++)
        {
            c = darkness[i].material.color;
            c.a = alpha;
            darkness[i].material.color = c;
        }
	}
}
