using UnityEngine;
using System.Collections;

public class BeamManager : MonoBehaviour {

	public float lifeSpan;
	public float damage;
	public GameObject pulse;
	//public GameObject[] pulses;
	public GameObject origin;
	public float wait;
	

	//I don't think this is the right C# way to do this...
	private bool firing;
	public bool IsFiring(){
		return firing;
	}

	/*void Start(){
		pulses = new GameObject[transform.childCount -1];
		for(int i = 0; i < pulses.Length; i++){
			pulses[i] = transform.GetChild(i).gameObject;
			pulses[i].transform.position = origin.transform.position + (pulses[i].transform.forward * i * beamLength / pulses.Length);
		}
	}*/

	void OnTriggerStay(Collider other){
		if(other.tag == "Enemy"){
			other.gameObject.GetComponent<EnemyBehaviourScript>().damage(damage * Time.deltaTime);
		}
		if(!firing && other.tag == "Pulse"){
			Destroy(other.gameObject);
		}
	}

	
	public void On(){
		audio.Play();
		transform.position -= Vector3.down * 10;
		firing = true;
		StartCoroutine(SpawnPulse());
	}

	public void Off(){
		audio.Stop();
		transform.position += Vector3.down * 10;
		firing = false;
	}

	void Update(){
		/*foreach(GameObject p in pulses){
			p.transform.position += p.transform.forward * Time.deltaTime;
			if((origin.transform.position - p.transform.position).magnitude > beamLength) p.transform.position = origin.transform.position;
		}*/	
	}
	
	IEnumerator SpawnPulse(){
		while(firing){
			GameObject go = Instantiate(pulse, origin.transform.position, origin.transform.rotation) as GameObject;
			go.GetComponent<PulseManager>().SetUp(this.gameObject, origin);
			yield return new WaitForSeconds(wait);
		}
	}

	void OnTriggerExit(Collider c){
		if(c.tag == "Pulse"){
			Destroy(c.gameObject);
		}
	}
}
