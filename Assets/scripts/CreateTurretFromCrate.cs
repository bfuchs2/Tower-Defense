using UnityEngine;
using System.Collections;

public class CreateTurretFromCrate : MonoBehaviour {


	public GameObject turret;
	public GameObject fragment;
	public GameObject explosion;
	private GameController gc;

	void Start(){
		gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Landscape"){
			Instantiate(explosion, transform.position + Vector3.up * 0.5f, Quaternion.identity);
			//start generating debris
			Instantiate(fragment, transform.position - Vector3.right * 0.15f, Quaternion.identity);
			Instantiate(fragment, transform.position + Vector3.right * 0.15f, Quaternion.identity);
			Instantiate(fragment, transform.position + Vector3.up * 0.85f, Quaternion.Euler(new Vector3(0, 0, 90)));
			Instantiate(fragment, transform.position + Vector3.up * 0.15f, Quaternion.Euler(new Vector3(0, 0, 90)));
			//Instantiate(fragment, transform.position - Vector3.right * 0.15f, Quaternion.Euler(new Vector3(0, 90, 0)));
			//Instantiate(fragment, transform.position + Vector3.right * 0.15f, Quaternion.Euler(new Vector3(0, 90, 0)));
			if(turret != null) Instantiate(turret, transform.position, Quaternion.identity);
			Destroy(this.gameObject);
		}else if(other.tag == "Enemy"){
			Destroy(other.gameObject);
			gc.AddToResources(20);
		}else if(other.tag == "Turret" && turret != null){
			gc.Recycle(other.gameObject);
		}
	}
}
