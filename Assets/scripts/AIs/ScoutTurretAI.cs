using UnityEngine;
using System.Collections;

public class ScoutTurretAI : TurretAI{
		
	public GameObject shot;
	private Transform cannon1, cannon2;
	private Animator anim;
	
	override protected void init(){
		cannon1 = gun.GetChild(0).transform;
		cannon2 = gun.GetChild(1).transform;
		anim = GetComponent<Animator>();
		anim.speed = 0.5f/att[3];
	}

	override protected void shouldAnimate(bool b){
		if(!b) whichCan = false;
		anim.SetBool("Is Firing", b);
	}

	
	override protected Vector3 getDesRot(){
		return (target.transform.position - gun.position + (Vector3.up * 0.4f)).normalized;
	}

	override protected void CheckAttributes(){
		anim.speed = 0.5f/att[3];
	}
	
	private bool whichCan = true;
	override protected void fire(Vector3 dir){
		if(shotTime <= Time.time){
			audio.Play();
			GameObject go = Instantiate(shot, whichCan ? cannon1.position : cannon2.position, gun.rotation) as GameObject;
			StandardBullet sb = go.GetComponent<StandardBullet>();
			sb.direction = dir;
			sb.damage = att[1];
			sb.Go();
			whichCan = !whichCan;
			shotTime = Time.time + att[3];
		}
	}

}

