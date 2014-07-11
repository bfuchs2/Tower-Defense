using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {
	
	public float camSpeed;
	public float zoomSpeed;

	//game boundaries
	private const float xMin = -0.5f, zMax = 0.5f, zMin = -15.5f, xMax = 16.5f;

	void Start(){
		transform.position = new Vector3(
			(xMin + xMax)/2,
			5,
			(zMin + zMax)/2
		);
	}


	Vector3 camMove;
	float m;
	private bool check = true;
	void Update(){
		camMove = Time.deltaTime * new Vector3(
			Input.GetAxisRaw("Horizontal")*m, 
			-Input.GetAxisRaw("Mouse ScrollWheel")*zoomSpeed, 
			Input.GetAxisRaw("Vertical")*m
		);
		transform.position += camMove;
		m = transform.position.y * camSpeed;
		check = !check;
	}

}
