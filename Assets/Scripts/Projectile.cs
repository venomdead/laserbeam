using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float damage = 100f;


	public float GetDamage(){
		return damage;
	}

	public void Hit(){

		Destroy (gameObject);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
		Destroy (gameObject);
    }
}
