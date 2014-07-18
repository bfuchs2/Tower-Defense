using UnityEngine;
using System.Collections;

public class FlakTurretAI : TurretAI {

	public float focus;
	public float flashDir;
	private Light l;
	private float flashTime;
	
	private Vector3 up, right;
	private RaycastHit hit;
	override protected void fire(Vector3 dir){
		if(shotTime > Time.time) return;
		l.intensity = 8;
		this.animation.Play();
		this.audio.Play();
		right = new Vector3(-dir.z, dir.y, dir.x);
		up = new Vector3(-dir.y, dir.x, dir.z);
		for(float x = -0.5f; x < 0.5f; x+=0.5f/focus){
			for(float y = -0.5f; y < 0.5f; y+=0.5f/focus){
				if(Physics.Raycast(gun.position, dir + (x * right) + (y * up), out hit)){
					if(hit.collider.tag == "Enemy"){
						hit.collider.GetComponent<EnemyBehaviourScript>().damage(att[1]/(focus * focus));
					}
				}
			}
		}
		flashTime = Time.time + flashDir;
		shotTime = Time.time + att[3];
	}

	override protected Vector3 getDesRot(){
		return (target.transform.position - gun.position).normalized;
	}

	
	override protected void CheckAttributes(){}
		

	override protected void shouldAnimate(bool b){
		if(l.intensity > 0 && Time.time > flashTime){
			l.intensity = 0;
		}
	}
	
	override protected void init(){
		l = gun.GetChild(0).GetChild(0).light;
	}
}
