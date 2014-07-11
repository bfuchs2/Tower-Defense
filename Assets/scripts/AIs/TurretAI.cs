using UnityEngine;
using System.Collections;

abstract public class TurretAI : MonoBehaviour {

	protected GameObject target;
	public static GameController gc;
	protected EnemyBehaviourScript targetScript;
	protected Transform gun;
	protected bool firing;
	//public float wait;
	protected float shotTime;
	//public float radius;
	//public float rotationSpeed;

	/**range, damage, rotationSpeed, wait*/
	public int[] upgradeCosts;
	/**range, damage, rotationSpeed, wait*/
	public float[] maxs;
	/**range, damage, rotationSpeed, wait*/
	public float[] att;

	//maximum upgrade values
	/* public float maxRange;
	public float maxDamage;
	public float maxSpeed;
	public float minWait;//e.i. minimum Hertz, maximum rate */
	
	void Awake(){
		gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}
	// Use this for initialization
	void Start (){
		shotTime = Time.time;
		transform.position = new Vector3((int) transform.position.x, 0, (int) transform.position.z);
		gun = transform.GetChild(1).transform;
		init();
	}

	Quaternion desRot;
	Collider[] enemies;
	void Update(){
		if(target == null || target.gameObject == null || (target.transform.position - transform.position).magnitude > att[0]){
			target = null;
			targetScript = null;
		}
		if(target == null){
			enemies = Physics.OverlapSphere(transform.position, att[0]);
			EnemyBehaviourScript temp;
			foreach(Collider c in enemies){
				if(c.tag == "Enemy"){
					temp = c.gameObject.GetComponent<EnemyBehaviourScript>();
					if(target == null || temp.create < targetScript.create){
						target = c.gameObject;
						targetScript = temp;
					}
				}
			}
		}
		if(target != null){
			desRot = Quaternion.LookRotation(getDesRot());
			if(Quaternion.Angle(gun.rotation, desRot) <= att[2] * Time.deltaTime){
			 	gun.rotation = desRot;
				firing = true;
				fire(getDesRot());
			}else{
				gun.rotation = Quaternion.RotateTowards(gun.rotation, desRot, att[2] * Time.deltaTime);
				//TODO play machine whiiring sound
				firing = false;
			}
		}else{
			firing = false;
		}
		shouldAnimate(firing);
	}

	public string Upgrade(int factor){// != used as XOR
		if((att[factor-1] >= maxs[factor-1])  !=  (factor == 4)) return "Maxed out";
		if(!gc.AddToResources(0-upgradeCosts[factor-1])) return "Insufficient Funds!";

		upgradeCosts[factor-1] = (int) (upgradeCosts[factor-1] * 1.25f);
		if(factor == 4){
			att[factor-1] /= 1.25f;
		}else{
			att[factor-1] *= 1.25f;
		}
		CheckAttributes();
		return "";
	}
	
	abstract protected Vector3 getDesRot();
	abstract protected void init();
	abstract protected void CheckAttributes();
	abstract protected void shouldAnimate(bool b);
	abstract protected void fire(Vector3 dir);
		
}
