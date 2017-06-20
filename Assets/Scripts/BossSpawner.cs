using UnityEngine;
using System.Collections;

public class BossSpawner : MonoBehaviour {

	public GameObject bossPrefab;
	public float width = 10f;
	public float height = 5f;
	public float speed = .5f;


	private bool movingRight = false;
	private float xmax;
	private float xmin;

	// Use this for initialization
	void Start () {
		
		//Setting boundaries
		float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftBoundary = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, distanceToCamera));
		Vector3 rightBoundary = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, distanceToCamera));

		xmax = rightBoundary.x;
		xmin = leftBoundary.x;
	
	}


	public void OnDrawGizmos ()
	{
		//gizmo for enemies
		Gizmos.DrawWireCube (transform.position, new Vector3 (width, height));

	}

	// Update is called once per frame
	void Update () {

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
	
	}

	public void BossSpawn ()
	{
		foreach(Transform child in transform){
			GameObject boss = Instantiate (bossPrefab, child.position, Quaternion.identity) as GameObject;
			//boss.transform.position = child;
		}

	}
}
