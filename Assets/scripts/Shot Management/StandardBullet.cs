using UnityEngine;
using System.Collections;

public class StandardBullet : MonoBehaviour {

	public Vector3 direction;
	public float speed;
	public float damage;

	public void Go(){
		rigidbody.velocity = direction.normalized * speed;
		transform.rotation = Quaternion.LookRotation(Vector3.down, rigidbody.velocity);
		//transform.rotation = Quaternion.Euler(90, transform.rotation.eulerAngles.y, 0);
	}
	
	void OnTriggerEnter(Collider other){
		if(other.tag == "Enemy"){
			EnemyBehaviourScript ebs = other.GetComponent<EnemyBehaviourScript>();
			ebs.damage(damage);
			Destroy(this.gameObject);
		}
	}
}
