using UnityEngine;
using System.Collections.Generic;

//the player controller controls an eneity via input from the user

public class PlayerController : MonoBehaviour 
{

	#region static variables

	public static List<PlayerController> currentPlayers = new List<PlayerController>();

	#endregion

    #region public variables

	public int playerIndex = -1;
	public bool inputLocked = false;

	public float meleeCooldown = 0.5f;
	public float magicCooldown = 0.3f;

	public float magicChargeTime = 1.0f;

	public CombatEntity entity;

    #endregion

    #region private variables

	float meleeCooldownRemaining = 0;
	float magicCooldownRemaining = 0;

    #endregion

    #region public methods

    #endregion

    #region private methods

    void TryMove()
    {
        //Handle input 
        Vector2 dir = Vector2.zero;
        if(Input.GetKey(KeyCode.W))
        {
            dir.y = 1;
        }
        else if (Input.GetKey (KeyCode.S)) 
        {
            dir.y = -1;
        }
        if (Input.GetKey (KeyCode.A))
        {
            dir.x = -1;
        }
        else if (Input.GetKey (KeyCode.D))
        {
            dir.x = 1;
        }
        
        entity.TryMove(dir);
    }

    void TryLook()
    {

	        Vector3 botScreenPos = Camera.main.WorldToScreenPoint(entity.transform.position);
	        entity.TryLook(Input.mousePosition - botScreenPos);
	
    }

    void TryAction()
    {
		if (Input.GetMouseButtonDown(0) && meleeCooldownRemaining <= 0)
        {
			meleeCooldownRemaining += meleeCooldown;
			entity.TryMeleeAttack();
        }
		else if (Input.GetMouseButtonDown(1) && magicCooldownRemaining <= 0)
		{
			magicCooldownRemaining += magicCooldown;
			entity.TryCastMagic();
		}
    }

    #endregion

    #region monobehaviour methods

	// Use this for initialization
	void Awake () 
    {
		currentPlayers.Add(this);
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (entity)
        {
			if(!inputLocked)
			{
                TryMove();
                TryLook();
                TryAction();
			}
        }
		meleeCooldownRemaining = Mathf.Max(0, meleeCooldownRemaining - Time.deltaTime);
		magicCooldownRemaining = Mathf.Max(0, magicCooldownRemaining - Time.deltaTime);


		//FIXME: DEBUG RESET
		if(Input.GetKeyDown(KeyCode.R))
		{
			Application.LoadLevel(Application.loadedLevel);
		}
	}

    void OnDisable()
    {
        entity.TryMove(Vector2.zero);
    }

	void OnDestroy()
	{
		currentPlayers.Remove(this);
	}

    #endregion
}
