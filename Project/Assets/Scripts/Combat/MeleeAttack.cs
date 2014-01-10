using UnityEngine;
using System.Collections.Generic;

public class MeleeAttack : MonoBehaviour 
{
	#region public variables

	public int baseDamage = 1;
	public float damageScale = 1;
	public float knockbackSpeed = 10;

	public float duration = 0.1f;

	#endregion

	#region private variables

	List<Killable> processed = new List<Killable>();
	float tick = 0;

	#endregion

	#region public methods

	public void ApplyDamage()
	{
		processed.Clear();

		this.collider2D.enabled = true;
		tick = 0;
	}

	#endregion

	#region monobehaviour methods

	void Awake()
	{
		if(this.collider2D == null)
		{
			Debug.LogError("Missing Collider2D on AreaDamage");
			enabled = false;
		}
	}

	void Update()
	{
		tick += Time.deltaTime;
		if(tick > duration)
		{
			this.collider2D.enabled = false;
		}
	}

	void OnTriggerEnter2D(Collider2D _other)
	{
		Killable killable = _other.GetComponent<Killable>();
		if(killable && !processed.Contains(killable))
		{
			int realDamage = (int)(baseDamage * damageScale);
			killable.OnDamage(realDamage, (killable.transform.position - transform.position).normalized * knockbackSpeed);
			processed.Add(killable);
		}
	}

	#endregion
}
