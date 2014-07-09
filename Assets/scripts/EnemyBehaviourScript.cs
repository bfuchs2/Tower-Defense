using UnityEngine;
using System.Collections;

public class EnemyBehaviourScript : MonoBehaviour {

	public Vector3[] tgts;
	private int target = 0;
	private Vector3 dir;
	private const float speed = 0.55f;
	public float speedFactor;
	public float err;
	public float health = 100;
	public float create;
	public GameController gameController;
			
	void Start () {
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		create = Time.time;
		transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
		tgts = new Vector3[]{
			new Vector3(1, 0, -6),
			new Vector3(1, 0, -10),
			new Vector3(5, 0, -10),
			new Vector3(5, 0, -2),
			new Vector3(12, 0, -2),
			new Vector3(12, 0, -13),
			new Vector3(17, 0, -13)
		};
		this.animation["Take 001"].speed = speedFactor;
		this.animation.Play();
		dir = tgts[target] - transform.position;
		dir.y = 0;
		dir.Normalize();
		dir *= speed * speedFactor;
		transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
		rigidbody.velocity = dir;
	}

	void FixedUpdate(){
		if((transform.position - tgts[target]).magnitude < err){
			target++;
		}
		if(target == tgts.Length){
			Destroy(this.gameObject);
			//TODO make the player lose the game if this happpens
		}else{
			setDir();
		}
	}
		
	void setDir(){
		dir = tgts[target] - transform.position;
		dir.y = 0;
		dir.Normalize();
		dir *= speed * speedFactor;
		moveTowards(dir, Quaternion.LookRotation(dir, Vector3.up));
	}

	public void setSpeed(float speed){
		speedFactor = speed;
		this.animation["Take 001"].speed = speed;
	}

	Vector3 error;
	void moveTowards(Vector3 desVel, Quaternion desRot){
		error = (desVel - rigidbody.velocity).normalized * speed * speedFactor;
		if(speedFactor > 1) error *= speedFactor;
		rigidbody.AddForce(error);
		transform.rotation = Quaternion.LookRotation(rigidbody.velocity, Vector3.up);
	}

	public void damage(float dam){
		health -= dam;
		if(health <= 0){
			gameController.AddToResources(20);
			Destroy(this.gameObject);
		}
	}
}
