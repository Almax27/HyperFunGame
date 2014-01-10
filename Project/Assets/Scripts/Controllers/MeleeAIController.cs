using UnityEngine;
using System.Collections;

public class MeleeAIController : MonoBehaviour {

	#region public types

	public enum State
	{
		Idle,
		Attacking,
		Fleeing
	}

	#endregion

	#region public variables

	public Entity entity;

	public State currentState = State.Idle;

	public int damage = 1;
	public float attackRate = 0.5f;
	public float knockbackVelocity = 5;
	public float agressiveness = 0.4f;

	public float fleeHealth = 1;
	public float fleeDistance = 2f;

	public bool CanAttack { get { return Time.time > lastAttackTime + attackRate; } }

	#endregion

	#region private variables

	float stateTick = 0;
	float lastAttackTime = 0;

	Killable target;

	float idleLookTick = 0;

	#endregion

	void SetState(State newState)
	{
		currentState = newState;
		stateTick = 0;
	}

	void OnAttack()
	{
		lastAttackTime = Time.time;
		if(stateTick > 1.0f)
		{
			SetState(State.Idle);
		}
	}

	// Use this for initialization
	void Start () 
	{
		entity = GetComponentInChildren<Entity>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(target == null)
			SetState(State.Idle);

		stateTick += Time.deltaTime;

		switch(currentState)
		{
		case State.Idle:
		{
			entity.TryMove(Vector2.zero);

			//pick closest target
			float minDist = float.MaxValue;
			for(int i = 0; i < PlayerController.currentPlayers.Count; i++)
			{
				PlayerController player = PlayerController.currentPlayers[i];
				if((player.entity.transform.position - this.entity.transform.position).magnitude < minDist)
				{
					target = player.entity;
				}
			}

			//choose something to do
			if(Random.value < 0.1f)
			{
				if(entity.health < fleeHealth || Random.value > agressiveness)
					SetState(State.Fleeing);
				else if(target)
					SetState(State.Attacking);
			}

			idleLookTick -= Time.deltaTime;
			if(idleLookTick < 0)
			{
				entity.TryLook(new Vector2(Random.Range(-1,1),Random.Range(-1,1)));
				idleLookTick = 0.5f + Random.value*1.5f;
			}
			break;
		}
		case State.Attacking:
		{
			if(CanAttack)
			{
				Vector3 delta = target.transform.position - this.entity.transform.position;
				entity.TryMove(delta);
				entity.TryLook(delta);
			}

			if(stateTick > 1.0f)
			{
				SetState(State.Idle);
			}

			break;
		}
		case State.Fleeing:
		{
			Vector3 delta = this.entity.transform.position - target.transform.position;
			if(delta.magnitude < fleeDistance)
			{
				entity.TryMove(delta);
				entity.TryLook(delta);
			}

			if(stateTick > 1.0f)
			{
				SetState(State.Idle);
			}

			break;
		}
		}
	}
	
	void OnCollisionStay2D(Collision2D _collision)
	{
		if(CanAttack)
		{
			Killable killable = _collision.collider.GetComponent<Killable>();
			if(killable && killable == target)
			{
				killable.OnDamage(damage, (target.transform.position - this.entity.transform.position).normalized * knockbackVelocity);
				OnAttack();
			}
		}
	}
}
