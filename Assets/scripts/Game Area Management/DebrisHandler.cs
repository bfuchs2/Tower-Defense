using UnityEngine;
using System.Collections;

public class DebrisHandler : MonoBehaviour {

	public float flashTime;
	void Start () {
		rigidbody.AddForce(new Vector3(Random.value, Random.Range(1, 3), Random.value) * 100);
		rigidbody.AddTorque(new Vector3(Random.value+1, Random.value, Random.value+1) * 100);
		Destroy(this.gameObject, 7);
		StartCoroutine(flash());
	}
	

	IEnumerator flash(){
		MeshRenderer mr = this.GetComponent<MeshRenderer>();
		yield return new WaitForSeconds(5);
		while(true){
			yield return new WaitForSeconds(flashTime);
			mr.enabled = !mr.enabled;
			flashTime -= 0.1f;
		}
	}
}
