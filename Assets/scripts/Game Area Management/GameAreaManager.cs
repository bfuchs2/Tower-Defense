using UnityEngine;
using System.Collections;

public class GameAreaManager : MonoBehaviour {

	void OnTriggerExit(Collider other){
		Destroy(other.gameObject);
	}
}
