using UnityEngine;
using System.Collections;

public class TurretAI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.position = new Vector3((int) transform.position.x, 0, (int) transform.position.z);
	}
}
