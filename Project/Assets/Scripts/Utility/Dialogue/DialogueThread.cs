using UnityEngine;
using System.Collections;

public class DialogueThread : MonoBehaviour 
{
    #region public variables

    public Dialogue[] dialogues = new Dialogue[0];
    public bool triggerOnStart = false;
    public bool triggerOnEnter = true;
    public bool destroyOnEnd = false;

    public DialogueUI dialogueUI = null;

    public Activator activatorOnDone = new Activator();

    #endregion

    #region protected variables

    protected bool isActive = false;
    protected int currentDialogueIndex = 0;

    #endregion

    #region public methods

    public void StartDialogueThread()
    {
        isActive = true;

        DisplayDialogue(0);
    }

    public void EndDialogueThread()
    {
        isActive = false;

        dialogueUI.TryHideDialogue();

        activatorOnDone.Process();

        if (destroyOnEnd)
            Destroy(this.gameObject);
    }

    #endregion

    #region protected methods

    protected void DisplayDialogue(int _index)
    {
        if (_index < dialogues.Length)
        {
            currentDialogueIndex = _index;

            Debug.Log(name + ": Displaying Dialogue " + _index);

            dialogueUI.TryDisplayDialogue(dialogues[_index]);

        } else
        {
            Debug.Log(name + ": Dialogue " + _index + " does not exist");
        }
    }

    #endregion

    #region monobehaviour methods

	// Use this for initialization
	void Start () 
    {
        if (triggerOnStart)
        {
            StartDialogueThread();
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (isActive)
        {
            if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                if(dialogueUI.GetState() == DialogueUI.State.WritingText)
                {
                    dialogueUI.SetState(DialogueUI.State.DisplayingText);
                }
                else if(dialogueUI.GetState() == DialogueUI.State.DisplayingText)
                {
                    currentDialogueIndex++;
                    if(currentDialogueIndex < dialogues.Length)
                    {
                        DisplayDialogue(currentDialogueIndex);
                    }
                    else
                    {
                        EndDialogueThread();
                    }
                }
            }
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                EndDialogueThread();
            }
        }
	}

    void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.tag == "Player")
        {
            StartDialogueThread();
        }
    }

    #endregion
}
