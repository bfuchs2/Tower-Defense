using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject crate;
	public GameObject enemy;
	public float camSpeed;
	public float zoomSpeed;

	//gane boundaries
	private float xMin = -0.5f, zMax = 0.5f, zMin = -15.5f, xMax = 16.5f;

	void Start(){
		spawnEnemy();
	}

	
	// Update is called once per frame
	private Vector3 camPosn, camMove, bottomLeft, topRight;
	float m;
	private Ray zero, one;
	private bool check = true;
	void Update (){
		camMove = new Vector3(Input.GetAxisRaw("Horizontal")*m, -Input.GetAxisRaw("Mouse ScrollWheel")*zoomSpeed, Input.GetAxisRaw("Vertical")*m) * Time.deltaTime;
		if(camMove.magnitude != 0 || check){//the following computation is only necesarry if the camera will move
			camPosn = Camera.main.transform.position + camMove;
			//camPosn = new Vector3(camPosn.x, camPosn.y, camPosn.z);//prevents deep copying
			//makes sure that the camera is within certain bounds such that the skybox is not visible
			zero = Camera.main.camera.ViewportPointToRay(new Vector3(0, 0, 0));
			one = Camera.main.ViewportPointToRay(new Vector3(1, 1, 0));
			//the boundaries of the camera at a given y, as two vector3s
			bottomLeft = Camera.main.transform.position.y*zero.direction/zero.direction.y + new Vector3(xMin, 0, zMin);
			topRight = Camera.main.transform.position.y*one.direction/one.direction.y + new Vector3(xMax, 0, zMax);
			Camera.main.transform.position = new Vector3(
				Mathf.Clamp(camPosn.x, bottomLeft.x, topRight.x),
				Mathf.Clamp(camPosn.y, 2, 13.8f),
				Mathf.Clamp(camPosn.z, bottomLeft.z, topRight.z)
			);
			m = camPosn.y * camSpeed;
			check = !check;
		}
		//if there is a click, send a turret to that location
		if(Input.GetMouseButtonDown(0)){
			Vector3 mp = Input.mousePosition;
			Ray ray = Camera.main.ScreenPointToRay(mp);
			mp = ray.origin - (ray.origin.y * ray.direction / ray.direction.y);//find the point along the ray at which y = 0
			mp.Set(round(mp.x), 15, round(mp.z-0.5f)-0.5f);//subtract by 0.5 because that way it works 
			Instantiate(crate, mp, Quaternion.identity);
		}
	}

	int round(float f){
		if(f % 1 >= 0.5) return (int) f + 1;
		return (int) f;
	}

	void spawnEnemy(){
		GameObject spawnPoint = GameObject.FindGameObjectWithTag("Spawn Point");
		Instantiate(enemy, spawnPoint.transform.position, Quaternion.identity);
	}
}