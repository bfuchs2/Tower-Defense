using UnityEngine;
using System.Collections;

public class RangeAnimatorScript : MonoBehaviour {

	public GameObject maxRange;
	public GameObject dynamicRange;
	bool on;
	public float rotationSpeed;
	public float growSpeed;


	void Start(){
		Disable();
	}

	public void Disable(){
		maxRange.GetComponent<MeshRenderer>().enabled = false;
		dynamicRange.GetComponent<MeshRenderer>().enabled = false;
		on = false;
	}

	public void SetPositionAndReset(float x, float y, float range){
		transform.position = new Vector3(x, 0.1f, y);
		transform.localScale = Vector3.one * range;
		transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
		maxRange.GetComponent<MeshRenderer>().enabled = true;
		dynamicRange.GetComponent<MeshRenderer>().enabled = true;
		dynamicRange.transform.localScale = new Vector3(0, 0, 1);
		on = true;
	}
	public void SetPositionAndReset(Vector3 posn, float range){
		SetPositionAndReset(posn.x, posn.z, range * 2);
	}

	void Update(){
		if(on){
			transform.Rotate(Vector3.forward * Time.deltaTime * rotationSpeed);
			if(dynamicRange.transform.localScale.x >= 1){
				dynamicRange.transform.localScale = new Vector3(0, 0, 1);
			}else{
				dynamicRange.transform.localScale += new Vector3(1, 1, 0) * Time.deltaTime * growSpeed;
			}
		}
	}
}
