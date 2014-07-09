using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {
	
	public float camSpeed;
	public float zoomSpeed;

	//game boundaries
	private const float xMin = -0.5f, zMax = 0.5f, zMin = -15.5f, xMax = 16.5f;
	private float maxHeight;

	void Start(){
		camPosn = new Vector3(
			(xMin + xMax)/2,
			0,
			(zMin + zMax)/2
		);
		float fact = 10;
		while(fact > 0.01f){
			if(Physics.Raycast(camera.ViewportPointToRay(Vector3.zero)) 
					&& Physics.Raycast(camera.ViewportPointToRay(new Vector3(1, 1, 0)))){
				camPosn.y ++;
				transform.position = camPosn;
			}else{
				fact /= 2;
			}
		}
		maxHeight = transform.position.y;
	}


	Vector3 camMove, camPosn, bottomLeft, topRight;
	float m;
	private Ray zero, one;
	private bool check = true;
	void Update(){
		camMove = Time.deltaTime * new Vector3(
			Input.GetAxisRaw("Horizontal")*m, 
			-Input.GetAxisRaw("Mouse ScrollWheel")*zoomSpeed, 
			Input.GetAxisRaw("Vertical")*m
		);
		if(camMove.magnitude != 0 || check){//the following computation is only necesarry if the camera will move
			camPosn = transform.position + camMove;
			//camPosn = new Vector3(camPosn.x, camPosn.y, camPosn.z);//prevents deep copying
			//makes sure that the camera is within certain bounds such that the skybox is not visible
			zero = camera.ViewportPointToRay(Vector3.zero);
			one = camera.ViewportPointToRay(Vector3.one);
			//the boundaries of the camera at a given y, as two vector3s
			//this was before I knew about raycasting, I should probably update this or something
			bottomLeft = transform.position.y*zero.direction/zero.direction.y + new Vector3(xMin, 0, zMin);
			topRight = transform.position.y*one.direction/one.direction.y + new Vector3(xMax, 0, zMax);
			transform.position = new Vector3(
				Mathf.Clamp(camPosn.x, bottomLeft.x, topRight.x),
				Mathf.Clamp(camPosn.y, 2, maxHeight),
				Mathf.Clamp(camPosn.z, bottomLeft.z, topRight.z)
				);
			m = camPosn.y * camSpeed;
			check = !check;
		}
	}

}
