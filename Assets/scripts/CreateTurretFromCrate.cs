using UnityEngine;
using System.Collections;

public class CreateTurretFromCrate : MonoBehaviour {


	public GameObject turret;
	public GameObject fragment;
	
	void OnTriggerEnter(Collider other){
		Instantiate(turret, transform.position, Quaternion.identity);
		//start generating debris
		Instantiate(fragment, transform.position - Vector3.right * 0.15f, Quaternion.identity);
		Instantiate(fragment, transform.position + Vector3.right * 0.15f, Quaternion.identity);
		Instantiate(fragment, transform.position - Vector3.up * 0.15f, Quaternion.Euler(new Vector3(0, 0, 90)));
		Instantiate(fragment, transform.position + Vector3.up * 0.15f, Quaternion.Euler(new Vector3(0, 0, 90)));
		//Instantiate(fragment, transform.position - Vector3.right * 0.15f, Quaternion.Euler(new Vector3(0, 90, 0)));
		//Instantiate(fragment, transform.position + Vector3.right * 0.15f, Quaternion.Euler(new Vector3(0, 90, 0)));
		Destroy(this.gameObject);
	}
}
