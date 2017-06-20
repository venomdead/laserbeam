using UnityEngine;
using System.Collections;

public class Position : MonoBehaviour {

    private void OnDrawGizmos()
    {
		Gizmos.DrawWireSphere (transform.position, 1);
    }
}
