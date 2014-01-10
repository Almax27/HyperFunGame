using UnityEngine;
using System.Collections.Generic;

public class PathingAIController : MonoBehaviour 
{
    #region public types

    public enum PathWrapType
    {
        Loop,
        PingPong,
        Clamp
    }

    #endregion

    #region public variables

    public Entity entity;
    public Path path;
    public PathWrapType pathWrapType = PathWrapType.Clamp;
    public float nodeThreshold = 0.2f;

    #endregion

    #region protected variables


    protected int nextNode = 0;


    List<PathEvent> activeEvents = new List<PathEvent>();

    public bool isMoving = false;
    public bool isMovingForwards = true;
    public bool isAutoLook = true;

    #endregion

    #region private variables


    #endregion

    #region protected variables

    protected void ProcessEvents()
    {
        for(int i = 0; i < activeEvents.Count; i++)
        {

            if(activeEvents[i].delay > 0)
                activeEvents[i].delay -= Time.deltaTime;
            else if(activeEvents[i].duration > 0)
                activeEvents[i].duration -= Time.deltaTime;
            else
            {
                switch(activeEvents[i].type)
                {
                    case PathEvent.Type.STOP:
                        isMoving = true;
                        break;
                    case PathEvent.Type.LOOK:
                        isAutoLook = true;
                        break;
                    case PathEvent.Type.TURN:
                        break;
                }

                activeEvents.RemoveAt(i--);
                continue;
            }

            switch(activeEvents[i].type)
            {
                case PathEvent.Type.STOP:
                    isMoving = false;
                    break;
                case PathEvent.Type.LOOK:
                    isAutoLook = false;
                    entity.TryLook(activeEvents[i].direction);
                    break;
                case PathEvent.Type.TURN:
                    entity.TryTurn(activeEvents[i].direction);
                    break;
            }
        }
    }

    protected void TryNextNode()
    {
        if (path == null || path.nodes.Count < 2)
            return;

        Debug.Log("Moving from: " + nextNode + " to: " + (nextNode + (isMovingForwards ? 1 : -1)));
        nextNode += isMovingForwards ? 1 : -1;

        switch (pathWrapType)
        {
            case PathWrapType.Clamp:
                if(nextNode < 0)
                {
                    nextNode = 0;
                    isMoving = true;
                }
                else if(nextNode >= path.nodes.Count)
                {
                    nextNode = path.nodes.Count - 1;
                    isMoving = false;
                }
                break;
            case PathWrapType.Loop:
                if(nextNode < 0)
                {
                    nextNode = path.nodes.Count;
                }
                else if(nextNode >= path.nodes.Count)
                {
                    nextNode = 0;
                }
                break;
            case PathWrapType.PingPong:
                if(nextNode < 0)
                {
                    nextNode = 1;
                    isMovingForwards = true;
                }
                else if(nextNode >= path.nodes.Count)
                {
                    nextNode = path.nodes.Count - 2;
                    isMovingForwards = false;
                }
                break;
        }
    }

    #endregion

    #region monobehaviour methods

	// Use this for initialization
	void Start () 
    {
	    
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (DialogueUI.isDialogueOpen ||
            (!path || path.nodes.Count == 0))
        {
            entity.TryMove(Vector2.zero);
            return;
        }

        ProcessEvents();

        if (isMoving)
        {
            PathNode pathNode = path.nodes [nextNode];
            Vector3 delta = pathNode.transform.position - transform.position;

            if (delta.magnitude > nodeThreshold)
            {
                entity.TryMove(path.nodes [nextNode].transform.position - entity.transform.position);
            } else
            {
                for (int i = 0; i < pathNode.events.Length; i++)
                {
                    PathEvent pathEvent = pathNode.events [i].Clone() as PathEvent;

                    //FIXME: this randomness should be applied by an inspector or something more centralised
                    pathEvent.delay += Random.Range(0, pathEvent.maxRandDelay);
                    pathEvent.duration += Random.Range(0, pathEvent.maxRandDuration);

                    activeEvents.Add(pathEvent);
                }
                TryNextNode();
            }
        } else
            entity.TryMove(Vector2.zero);

        if (isAutoLook)
        {
            entity.TryLook(entity.Heading);
        }
	}

    #endregion
}
