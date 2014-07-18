using UnityEngine;
using System.Collections;

public class PulseManager : MonoBehaviour {

	public GameObject beam;
	public GameObject origin;
	private float distance;
	public float speed;
	void Start () {
		distance = 0;
		Destroy(this.gameObject, 4);
	}

	public void SetUp(GameObject b, GameObject o){
		beam = b;
		origin = o;
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = beam.transform.rotation;
		transform.position = origin.transform.position +  origin.transform.forward * distance;
		distance += Time.deltaTime * speed;
	}
}
