using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class Door : MonoBehaviour {

    #region public variables

    public Room[] rooms = null; //rooms this door connects to, it will fade them in when open

    public Transform leftDoor = null;
    public Transform rightDoor = null;

    public AudioClip openSound = null;

    public float animTime = 0.5f;

    public bool isLocked = false;

    #endregion

    #region protected variables

    protected bool isOpen = false;
    protected bool hasOpened = false;

    protected Vector3 vel = Vector3.zero;

    #endregion

    #region monobehaviour methods

	// Use this for initialization
	void Start () 
    {
	    
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (isOpen)
        {
            if(!hasOpened)
            {
                hasOpened = true;
                for(int i = 0; i < rooms.Length; i++)
                {
                    rooms[i].Lighten();
                }
            }

            leftDoor.transform.localPosition = Vector3.SmoothDamp(leftDoor.transform.localPosition, new Vector3(-2,0,0), ref vel, animTime);
            rightDoor.transform.localPosition = -leftDoor.transform.localPosition;
        } else
        {
            leftDoor.transform.localPosition = Vector3.SmoothDamp(leftDoor.transform.localPosition, Vector3.zero, ref vel, animTime);
            rightDoor.transform.localPosition = -leftDoor.transform.localPosition;
        }
	}

    void FixedUpdate()
    {
        isOpen = false;
    }

    void OnTriggerEnter2D(Collider2D _other)
    {
        if (!isLocked)
        {
            FAFAudio.Instance.PlayOnce(openSound, 0.1f);
        }
    }

    void OnTriggerStay2D(Collider2D _other)
    {
        if (!isLocked)
        {
            if (_other.tag == "Player")
                isOpen = true;
        }
    }

    #endregion
}
