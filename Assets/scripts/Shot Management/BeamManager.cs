using UnityEngine;
using System.Collections;

public class BeamManager : MonoBehaviour {

	public float lifeSpan;
	public float damage;

	void OnTriggerStay(Collider other){
		if(other.tag == "Enemy"){
			other.gameObject.GetComponent<EnemyBehaviourScript>().damage(damage * Time.deltaTime);
		}
	}
}
