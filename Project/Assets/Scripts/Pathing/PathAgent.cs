using UnityEngine;
using System.Collections;

public class PathAgent : MonoBehaviour 
{

    #region public variables

    public Path path;

    public PathNode NextNode
    {
        get {
            return path.nodes[nextNode];
        }
    }

    #endregion

    #region protected variables

    int nextNode = 0;

    #endregion

    #region public methods



    #endregion

    #region monobehaviour methods

	// Use this for initialization
	void Start () 
    {

	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    #endregion
}
