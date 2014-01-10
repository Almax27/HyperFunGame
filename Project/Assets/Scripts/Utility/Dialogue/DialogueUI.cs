using UnityEngine;
using System.Collections;

public class DialogueUI : MonoBehaviour 
{
    #region public types

    public enum State
    {
        Inactive,
        Entering,
        WritingText,
        DisplayingText,
        Exiting
    }

    #endregion
   
    #region public variables

    public static bool isDialogueOpen = false;

    public GUITexture dialoguePanel;
    public GUITexture dialoguePortrait;
    public GUIText dialogueName;
    public GUIText dialogueTextLine1;
    public GUIText dialogueTextLine2;

    public Easing.Helper transitionEnter;
    public Easing.Helper transitionExit;
    public Vector3 hiddenOffset;

    public float transitionTime = 0.5f;
    public float letterDelay = 0.02f;

    public AudioClip textSound = null;

    #endregion

    #region protected variables

    State state = State.Inactive;
    string desiredTextLine1;
    string desiredTextLine2;

    #endregion

    #region private vairables

    private Dialogue currentDialogue = null;

    private Vector3 animVel = Vector3.zero;
    private float writingTick = 0;

    #endregion

    #region public methods

    public void SetState(State _newState)
    {
        bool active = true;
        bool resetCamera = false;

        isDialogueOpen = true;

        switch (_newState)
        {
            case State.Inactive:

                active = false;
                resetCamera = true;

                transform.position = hiddenOffset;

                currentDialogue = null;

                isDialogueOpen = false;

                break;
            case State.Entering:
                Debug.Log("Entering Dialogue Thread");
                transitionEnter.Reset();
                break;
            case State.WritingText:

                writingTick = 0;

                break;
            case State.DisplayingText:

                dialogueTextLine1.text = desiredTextLine1;
                dialogueTextLine2.text = desiredTextLine2;

                transform.position = Vector3.zero;

                break;
            case State.Exiting:
                Debug.Log("Exiting Dialogue Thread");
                resetCamera = true;
                transitionExit.Reset();

                break;
        }

        gameObject.SetActive(active);

        if (resetCamera)
        {
            //do camera reset if necessary
        }

        state = _newState;
    }

    public State GetState()
    {
        return state;
    }

    public void TryDisplayDialogue(Dialogue _dialogue)
    {
        if (_dialogue == null)
        {
            Debug.Log("Null dialogue given");
            return;
        }

        if (currentDialogue == null)
            SetState(State.Entering);
        else
            SetState(State.WritingText);

        currentDialogue = _dialogue;

        dialoguePortrait.texture = _dialogue.portrait;

        dialogueTextLine1.text = dialogueTextLine2.text = "";
        desiredTextLine1 = _dialogue.textLine1;
        desiredTextLine2 = _dialogue.textLine2;

        dialogueName.text = _dialogue.nameText;

        FollowCamera followCam = Camera.main.GetComponent<FollowCamera>();
        if (followCam)
        {
            if (_dialogue.cameraTarget)
            {
                followCam.target = _dialogue.cameraTarget;
            }
        }

        FAFAudio.Instance.PlayOnce(_dialogue.sound, 0.8f);

        _dialogue.activator.Process();
    }

    public void TryHideDialogue()
    {
        if (state != State.Exiting || state != State.Inactive)
        {
            SetState(State.Exiting);
        }
    }


    #endregion
    
    #region monobehaviour methods

	// Use this for initialization
	void Awake () 
    {
        SetState(State.Inactive);
	}
	
	// Update is called once per frame
	void Update () 
    {
	    switch (state)
        {
            case State.Inactive:
            {
                break;
            }
            case State.Entering:
            {
                Vector3 pos;
                if(!transitionEnter.Update(Time.deltaTime, hiddenOffset, Vector3.zero, out pos))
                    SetState(State.WritingText);

                transform.position = pos;
                break;
            }
            case State.WritingText:
            {
                writingTick += Time.deltaTime;
                float t = writingTick/letterDelay;

                if(t < desiredTextLine1.Length)
                {
                    string subText = desiredTextLine1.Substring(0, Mathf.FloorToInt(t));

                    if(subText != dialogueTextLine1.text)
                        FAFAudio.Instance.PlayOnce(textSound, 0.3f);

                    dialogueTextLine1.text = subText;
                }
                else if(t < desiredTextLine1.Length + desiredTextLine2.Length)
                {
                    dialogueTextLine1.text = desiredTextLine1;

                    string subText = desiredTextLine2.Substring(0, Mathf.FloorToInt(t - desiredTextLine1.Length));
                    
                    if(subText != dialogueTextLine2.text)
                        FAFAudio.Instance.PlayOnce(textSound, 0.3f);
                    
                    dialogueTextLine2.text = subText;
                }
                else
                {
                    SetState(State.DisplayingText);
                }
                break;
            }
            case State.DisplayingText:
            {
                break;
            }
            case State.Exiting:
            {
                Vector3 pos;
                if(!transitionExit.Update(Time.deltaTime, Vector3.zero, hiddenOffset, out pos))
                    SetState(State.Inactive);

                transform.position = pos;
                break;
            }
        }
	}

    #endregion
}
