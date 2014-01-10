using UnityEngine;
using System.Collections;

public class WanderingAIController : MonoBehaviour 
{
    public bool canWander = false;

    public Entity entity;

    Vector2 dir = Vector2.zero;

    float waitLookTick = 0;
    float moveTick = 0;

    Vector3 lastCollisionNormal = Vector3.zero;

	// Use this for initialization
	void Start () 
    {
		entity = GetComponentInChildren<Entity>();
	}
	
	// Update is called once per frame
	void Update () 
    {
		if (entity)
        {
            if(waitLookTick <= 0)
            {
                entity.TryLook(new Vector2(Random.Range(-1,1),Random.Range(-1,1)));
                waitLookTick = Random.Range(0.5f,1.5f);
            }
            else
            {
                waitLookTick -= Time.deltaTime;
            }

            if(canWander)
            {
                if(moveTick <= 0)
                {
                    moveTick = Random.Range(0.5f, 3f);

                    //move if stopped, chance of not moving
                    if(dir == Vector2.zero || Random.value < 0.7f)
                    {
                        dir = new Vector2(Random.Range(-1f,1f),Random.Range(-1f,1f));
                    }
                    else
                    {
                        dir = Vector2.zero;
                    }
                }
                else
                {
                    moveTick -= Time.deltaTime;
                    entity.TryMove(dir);
                    if(dir != Vector2.zero)
                        entity.TryLook(dir);
                }
            }
            else
            {
                entity.TryMove(Vector2.zero);
            }
        }
	}

    void OnCollisionEnter2D(Collision2D _collision)
    {
        Vector3 normal = _collision.contacts [0].normal;
        if (normal != lastCollisionNormal)
        {
            lastCollisionNormal = normal;
			dir = normal;
			entity.TryMove(dir);
        }
    }
}
