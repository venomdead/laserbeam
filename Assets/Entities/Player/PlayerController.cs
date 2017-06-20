using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{

	public float moveSpeed = 0.7f;
	public float padding = 1.0f;
	public float laserSpeed;
	public float firingRate = 0.2f;
	public float health = 250f;
	public GameObject laserPrefab;
	public Text playerHealth;
	public AudioClip laserSound;
	public AudioClip hitSound;
	public AudioClip destroySound;

	private LevelManager levelManager;

	float xmin;
	float xmax;

	// Use this for initialization
	void Start ()
	{


		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftmost = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, distance));
		Vector3 rightmost = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, distance));
		xmin = leftmost.x + padding;
		xmax = rightmost.x - padding;




	}

	// Update is called once per frame
	void Update ()
	{
		//call method to move
		moveWithKeyStrokes ();

		//call method to shoot laser beam, invoke and cancel invoke to start and continue and stop firing
		if (Input.GetKeyDown (KeyCode.Space)) {
			InvokeRepeating ("laserSpawner", 0.000001f, firingRate);
		}

		if (Input.GetKeyUp (KeyCode.Space)) {
			CancelInvoke ("laserSpawner");
		}

		playerHealth.text = health.ToString ();
	}

	void moveWithKeyStrokes ()
	{
		//move the ship with keystrokes, adjust speed

		if (Input.GetKey (KeyCode.LeftArrow)) {
			//transform.position += new Vector3 (-moveSpeed * Time.deltaTime, 0, 0);
			transform.position += Vector3.left * moveSpeed * Time.deltaTime;

		} else if (Input.GetKey (KeyCode.RightArrow)) {

			//transform.position += new Vector3 (+moveSpeed * Time.deltaTime, 0, 0);
			transform.position += Vector3.right * moveSpeed * Time.deltaTime;
		}
		//restrict movement with screen resolution
		float newX = Mathf.Clamp (transform.position.x, xmin, xmax);
		transform.position = new Vector3 (newX, transform.position.y, transform.position.z);
	}

	void laserSpawner ()
	{

		Vector3 startPosition = transform.position + new Vector3 (0, 1, 0);
		GameObject beam = Instantiate (laserPrefab, startPosition, Quaternion.identity) as GameObject;
		//give initial velocity to laser beam
		beam.rigidbody2D.velocity = new Vector3 (0, laserSpeed, 0);

		AudioSource.PlayClipAtPoint (laserSound, transform.position);

	}

	private void OnTriggerEnter2D (Collider2D collision)
	{
		//checks if it's a collision with a laser
		Projectile missile = collision.gameObject.GetComponent<Projectile> ();
		if (missile) {
			//if it is, apply damage and check if it's less or equal to 0, if it is, destroy the enemy clone
			AudioSource.PlayClipAtPoint (hitSound, transform.position);
			health -= missile.GetDamage ();

			if (health <= 0) {
				AudioSource.PlayClipAtPoint (destroySound, transform.position);
				Die ();
			}
		}
	}


	void Die ()
	{

		levelManager = Object.FindObjectOfType<LevelManager> ();

		Destroy (gameObject);
		levelManager.LoadLevel ("Lose");
	}
}
