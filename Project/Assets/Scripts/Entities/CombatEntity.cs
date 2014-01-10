using UnityEngine;
using System.Collections;

public class CombatEntity : Entity 
{
	#region public variables

	public float knockbackScale = 1.0f;

	public MeleeAttack meleeAreaDamage;
	
	public Projectile magicPrefab;

	public Animator animator;

	#endregion

	#region private variables
	 
	float knockBackTime = 0;
	Vector3 knockBackVelocity = Vector3.zero;

	#endregion

	#region public methods
	
	public void TryMeleeAttack()
	{
		meleeAreaDamage.damageScale = 1;
		meleeAreaDamage.ApplyDamage();

		animator.SetTrigger("melee");
	}
	
	public void TryDashSlash()
	{
		
	}
	
	public void TryCastMagic()
	{
		animator.SetTrigger("magic");
		GameObject gobj = GameObject.Instantiate(magicPrefab.gameObject) as GameObject;

		Projectile projectile = gobj.GetComponent<Projectile>();

		projectile.damageScale = 1;

		projectile.rigidbody2D.velocity = lookBody.up * 10;
		projectile.transform.position = lookBody.position;
		projectile.transform.rotation = lookBody.rotation;
	}

	public override void OnDamage (int _damage, Vector3 _knockBackVelocity)
	{
		base.OnDamage (_damage, _knockBackVelocity);
		animator.SetTrigger("damaged");

		knockBackTime = 0.07f;
		knockBackVelocity = _knockBackVelocity * knockbackScale;
	}
	
	#endregion

	#region monobehaviour methods

	protected override void Update ()
	{
		if(knockBackTime < 0)
			base.Update ();
		else
		{
			rigidbody2D.velocity = knockBackVelocity;
			knockBackTime -= Time.deltaTime;
		}

		if(meleeAreaDamage)
		{
			meleeAreaDamage.transform.position = lookBody.position;
			meleeAreaDamage.transform.rotation = lookBody.rotation;
		}
	}

	#endregion


}
