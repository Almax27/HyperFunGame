using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class PathEvent : ICloneable
{
    public enum Type
    {
        STOP,
        LOOK,
        TURN
    }

    public Type type;
    public float delay = 0;
    public float maxRandDelay = 0;
    public float duration = 1.0f;
    public float maxRandDuration = 0;
    public Vector3 direction;

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}

public class PathNode : MonoBehaviour 
{

    public PathEvent[] events = new PathEvent[0];
   
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}
