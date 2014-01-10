using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Projectile : MonoBehaviour 
{
	public int baseDamage = 1;
	public float damageScale = 1;
	public float knockbackVelocity = 5;

	public bool destroyOnHit = true;
	public float lifeTime = 1;

	public GameObject[] spawnOnDestroy = new GameObject[0];

	void OnTriggerEnter2D(Collider2D _other)
	{
		Killable killable = _other.GetComponent<Killable>();
		if(killable)
		{
			int realDamage = (int)(baseDamage * damageScale);
			killable.OnDamage(realDamage, (killable.transform.position - this.transform.position).normalized * knockbackVelocity);
			if(destroyOnHit)
			{
				Destroy(gameObject);
			}
		}
	}

	void Update()
	{
		lifeTime -= Time.deltaTime;
		if(lifeTime <= 0)
		{
			Destroy(gameObject);
		}
	}

	void OnDestroy()
	{
		for(int i = 0; i < spawnOnDestroy.Length; i++)
		{
			GameObject gobj = GameObject.Instantiate(spawnOnDestroy[i]) as GameObject;
			gobj.transform.position = transform.position;
		}
	}

}
