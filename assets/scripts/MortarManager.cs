using UnityEngine;
using System.Collections;

public class MortarManager : MonoBehaviour {

	public float radius;
	public float damage;
	public GameObject explosion;

	void OnTriggerEnter(Collider other){
		if(other.tag == "Enemy"|| other.tag == "Landscape" || other.tag == "Turret" || other.tag == "Path"){
			GameObject go = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
			ExplotionManager em = go.GetComponent<ExplotionManager>();
			em.lifeSpan = 2.4f;
			em.scale = radius + 1;
			foreach(Collider c in Physics.OverlapSphere(transform.position, radius)){
				if(c.tag == "Enemy"){
					c.GetComponent<EnemyBehaviourScript>().damage(
						damage * Mathf.Log(
							1 + radius - (transform.position - c.transform.position).magnitude
						)
					);
				}
			}
			Destroy(this.gameObject);
		}
	}
}
