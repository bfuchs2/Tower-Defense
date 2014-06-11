using UnityEngine;
using System.Collections;

public class EnemyBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator walk(){
		yield return new WaitForSeconds(0.1f);
	}
}
