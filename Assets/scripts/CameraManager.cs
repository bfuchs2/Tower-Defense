using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {
	
	public float camSpeed;
	public float zoomSpeed;

	//game boundaries
	private const float xMin = -0.5f, zMax = 0.5f, zMin = -15.5f, xMax = 16.5f;

	void Start(){
		turret = null;
		transform.position = new Vector3(
			(xMin + xMax)/2,
			5,
			(zMin + zMax)/2
		);
	}


	Vector3 camMove;
	float m;
	void Update(){
		if(turret == null){
			camMove = Time.deltaTime * new Vector3(
				Input.GetAxisRaw("Horizontal")*m, 
				-Input.GetAxisRaw("Mouse ScrollWheel")*zoomSpeed, 
				Input.GetAxisRaw("Vertical")*m
			);
			transform.position += camMove;
			m = transform.position.y * camSpeed;
		}else{
			transform.rotation = turret.transform.GetChild(1).transform.rotation;
			float angle;
			Vector3 axis;
			transform.rotation.ToAngleAxis(out angle, out axis);
			transform.position = turret.transform.GetChild(1).transform.position + (Vector3.up * 0.7f);
			transform.Rotate(new Vector3(20, 0, 0));
		}
	}

	private Vector3 oldPosn;
	private GameObject turret;
	public void SwitchToTurretView(GameObject t){
		if(turret == null) oldPosn = transform.position;
		turret = t;
		if(t == null){
			transform.position = oldPosn;
			transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
		}
	}

}
