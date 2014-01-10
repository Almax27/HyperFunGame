using UnityEngine;
using System.Collections;

public class Killable : MonoBehaviour 
{
    public int health = 10;

    public AudioClip deathSound = null;
    public AudioClip damageSound = null;

    public GameObject[] spawnOnDeath = new GameObject[0];
	public GameObject[] spawnOnHit = new GameObject[0];

	public GameObject damageTextPrefab = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public virtual void OnDamage(int _damage, Vector3 _knockbackVelocity)
    {
        health -= _damage;
        if (health <= 0)
        {
            OnDeath();
        }
        FAFAudio.Instance.PlayOnce(damageSound, 0.8f);

		for (int i = 0; i < spawnOnHit.Length; i++)
		{
			GameObject gobj = Instantiate(spawnOnHit[i]) as GameObject;
			gobj.transform.position = this.transform.position;
		}

		GameObject text = Instantiate(damageTextPrefab) as GameObject;
		text.transform.position = this.transform.position;
		text.GetComponentInChildren<TextMesh>().text = _damage.ToString();

		Debug.Log(name + " took " + (int)_damage + " damage");
    }
    
    public virtual void OnDeath()
    {
        health = 0;
        
        //do some fancy shit here
        FAFAudio.Instance.PlayOnce(deathSound, 1);

        for (int i = 0; i < spawnOnDeath.Length; i++)
        {
            GameObject gobj = Instantiate(spawnOnDeath[i]) as GameObject;
            gobj.transform.position = this.transform.position;
        }
        
        Destroy(this.gameObject);
    }
}
