using UnityEngine;
using System.Collections;

public class RangedTurretAI : TurretAI {
	
	public GameObject shot;
	Vector3 direction;
	public float shotSpeed;

	override protected Vector3 getDesRot(){
		direction = (target.transform.position - gun.position);
		direction.y = 0;
		float angle = Mathf.Asin(direction.magnitude * Physics.gravity.magnitude/(shotSpeed * shotSpeed)) * 0.5f;
		if(angle > Mathf.PI/4 || float.IsNaN(angle)) return Vector3.one;
		direction.y = direction.magnitude * Mathf.Tan(angle);
		return direction.normalized;
	}
	
	override protected void fire(Vector3 dir){
		if(shotTime > Time.time) return;
		GameObject go = Instantiate(shot, gun.position, Quaternion.identity) as GameObject;
		go.rigidbody.velocity = dir * shotSpeed;
		go.GetComponent<MortarManager>().damage = att[1];
		audio.Play();
		shotTime = Time.time + att[3];
	}

	override protected void CheckAttributes(){
		shotSpeed = Mathf.Sqrt(att[0] * Physics.gravity.magnitude/0.99f);
	}	

	override protected void shouldAnimate(bool b){}
		
	override protected void init(){
		att[0] = 0.99f * (shotSpeed * shotSpeed /Physics.gravity.magnitude);
	}
}
	