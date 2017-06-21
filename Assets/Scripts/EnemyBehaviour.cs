using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour {

	public float health = 150f;
	public float projectileSpeed;
	public float enemyFiringRate;
	public GameObject projectilePrefab;
	public float shotsPerSecond = 0.5f;
	public int scoreValue = 150;
	public AudioClip enemyBeamSound;
	public AudioClip deathSound;

	private ScoreKeeper scoreKeeper;


    private void Start()
    {
		scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();

	}
    private void Update()
    {
		//get frequency of shots fired per second
		float probability = shotsPerSecond * Time.deltaTime;
		//turn probability into decision
		if(Random.value < probability){
			SpawnProjectiles ();

		}

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
		//checks if it's a collision with a laser
		Projectile missile = collision.gameObject.GetComponent<Projectile> ();
		if(missile){
			//if it is, apply damage and check if it's less or equal to 0, if it is, destroy the enemy clone
			health -= missile.GetDamage();

			if(health <= 0){
				Die ();

			}
		}

    }

	void Die(){
		AudioSource.PlayClipAtPoint (deathSound, transform.position);
		Destroy (gameObject);
		projectileSpeed = projectileSpeed + 0.1f;
		scoreKeeper.Score (scoreValue);

	}



	void SpawnProjectiles ()
	{
		Vector3 startPosition = transform.position + new Vector3 (0, -1, 0);
		GameObject enemyBeam = Instantiate (projectilePrefab, startPosition, Quaternion.identity) as GameObject;

		enemyBeam.rigidbody2D.velocity = new Vector2 (0, -projectileSpeed);
		AudioSource.PlayClipAtPoint (enemyBeamSound, transform.position);
	}
}
