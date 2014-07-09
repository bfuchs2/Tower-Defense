using UnityEngine;
using System.Collections;

public class ExplotionManager : MonoBehaviour {

	public float scale;
	public float lifeSpan;
	private float startTime;
	// Use this for initialization
	void Start () {
		startTime = Time.time;
		transform.position = new Vector3((int) transform.position.x, 0, (int) transform.position.z);
		transform.localScale = Vector3.one * scale;
		Destroy(this.gameObject, lifeSpan);
	}

	void Update(){
		transform.localScale = Vector3.one * scale * Mathf.Log(1 + lifeSpan + startTime - Time.time);
		light.range = transform.localScale.x * 2;
	}

}
