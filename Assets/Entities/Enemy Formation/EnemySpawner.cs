using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{

	public GameObject enemyPrefab;
	public GameObject enemy2Prefab;
	public float width = 10f;
	public float height = 5f;
	public float speed = .5f;
	public float spawnDelay = 0.5f;

	private bool spawnSwitch = true;
	private bool movingRight = false;
	private float xmax;
	private float xmin;

	void Start ()
	{
		//Setting boundaries
		float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftBoundary = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, distanceToCamera));
		Vector3 rightBoundary = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, distanceToCamera));

		xmax = rightBoundary.x;
		xmin = leftBoundary.x;

		SpawnUntilFull ();
	}

	public void OnDrawGizmos ()
	{
		//gizmo for enemies
		Gizmos.DrawWireCube (transform.position, new Vector3 (width, height));

	}



	// Update is called once per frame
	void Update ()
	{


		//moving formation on every frame update
		if (movingRight) {
			transform.position += Vector3.right * speed * Time.deltaTime;
		} else {
			transform.position += Vector3.left * speed * Time.deltaTime;
		}

		// checking if the formation is in the edges
		float rightEdgeOfFormation = transform.position.x + (width / 2);
		float leftEdgeOfFormation = transform.position.x - (width / 2);

		if (leftEdgeOfFormation < xmin) {
			//flipping inverse of formation 
			movingRight = true;
		} else if (rightEdgeOfFormation > xmax) {

			movingRight = false;
		}

		if (AllMembersDead () & spawnSwitch) {
			Debug.Log ("Empty Formation, spawning minions");
			//bossSpawner.BossSpawn ();
			SpawnUntilFull ();		
		} else if (AllMembersDead () & !spawnSwitch){
			SpawnSecondEnemy ();
		}
	}


	//method spawns enemies one by one until no gizmo empty is left
	void SpawnUntilFull ()
	{
		Transform freePosition = NextFreePosition ();
		if (freePosition) {
			GameObject enemy = Instantiate (enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = freePosition;
		}
		//Recursion, when a method calls itself
		if (NextFreePosition ()) {
			Invoke ("SpawnUntilFull", spawnDelay);
		}

		spawnSwitch = false;
	}



	//method returns if there is a fee position when we destroy enemy
		Transform NextFreePosition ()
		{
			foreach (Transform childPositionGameObject in transform) {
				if (childPositionGameObject.childCount == 0) {
					return childPositionGameObject;
				}
			}
			return null;
		}


		
	//method checks if all enemies are dead

		bool AllMembersDead ()
		{
			foreach (Transform childPositionGameObject in transform) {
				if (childPositionGameObject.childCount > 0) {
					return false;
				}
			}
			return true;
		}

		
	void SpawnSecondEnemy(){

		Transform freePosition = NextFreePosition ();
		if (freePosition) {
			GameObject enemy = Instantiate (enemy2Prefab, freePosition.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = freePosition;
		}
		//Recursion, when a method calls itself
		if (NextFreePosition ()) {
			Invoke ("SpawnSecondEnemy", spawnDelay);

		}

		spawnSwitch = true;

	}


	}

