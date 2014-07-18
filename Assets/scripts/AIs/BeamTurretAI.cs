using UnityEngine;
using System.Collections;

public class BeamTurretAI : TurretAI {

	private BeamManager bm;
	public float lifeSpan;
	private float downTime;

	override protected void fire(Vector3 dir){
		if(shotTime > Time.time) return;
		//this.animation.Play();
		bm.On();
		att[2] /= 3; //rotationSpeed is increased back in the shouldAnimate(bool b) method
		downTime = Time.time + lifeSpan;
		shotTime = Time.time + att[3];
	}
	
	override protected Vector3 getDesRot(){
		Vector3 desRot = target.transform.position - gun.position;
		desRot.y = 0;
		return desRot;
	}

	/**
	* returns true if the upgrade was successful and
	* false if that aspect is maxed out */
	override protected void CheckAttributes(){
		bm.damage = att[1];
	}

	override protected void shouldAnimate(bool b){
		if(downTime <= Time.time && bm.IsFiring()){
			bm.Off();
			att[2] *= 3;
		}
	}
	
	override protected void init(){
		bm = gun.GetChild(1).GetComponent<BeamManager>();
		bm.Off();
}
	
}
