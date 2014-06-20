using UnityEngine;
using System.Collections;

public class RangedTurretAI : MonoBehaviour {


	
	public float shotSpeed;
	public GameObject shot;
	protected GameObject target;
	protected Transform gun;
	protected bool firing;
	public float wait = 0.5f;
	protected float shotTime;
	private float radius;
	
	// Use this for initialization
	void Start () {
		radius = shotSpeed * shotSpeed /Physics.gravity.magnitude;
		shotTime = Time.time;
		transform.position = new Vector3((int) transform.position.x, 0, (int) transform.position.z);
		gun = transform.GetChild(1).transform;
		init();
		//anim = this.GetComponent<Animation>();
	}

	void Update () {
		if(target != null){
			if(target.GetComponent<EnemyBehaviourScript>().health <= 0){
				Destroy(target);
				target = null;
			}else if((target.transform.position - transform.position).magnitude > radius){
				target = null;
			}
		}
		if(target == null){
			Collider[] enemies = Physics.OverlapSphere(transform.position, radius);
			foreach(Collider c in enemies){
				if(c.tag == "Enemy"){
					target = c.gameObject;
				}
			}
			firing = target != null;
		}
		if(firing){
			Vector3 direction = (target.transform.position - gun.position);
			direction.y = 0;
			float angle = Mathf.Asin(direction.magnitude * Physics.gravity.magnitude/(shotSpeed * shotSpeed)) * 0.5f;
			if(angle > Mathf.PI/4 || float.IsNaN(angle)) return;
			direction.y = direction.magnitude * Mathf.Tan(angle);
			gun.rotation = Quaternion.LookRotation(direction);
			fire(direction);
		}
	}

	void fire(Vector3 dir){
		if(shotTime > Time.time) return;
		GameObject go = Instantiate(shot, gun.position, Quaternion.identity) as GameObject;
		go.rigidbody.velocity = dir.normalized * shotSpeed;
		shotTime = Time.time + wait;
	}

	protected void shouldAnimate(bool b){}

	protected void init(){}
}
