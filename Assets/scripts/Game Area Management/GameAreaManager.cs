using UnityEngine;
using System.Collections;

public class GameAreaManager : MonoBehaviour {

	void OnTriggerExit(Collider other){
		if(other.tag != "Enemy") Destroy(other.gameObject);
	}
}
