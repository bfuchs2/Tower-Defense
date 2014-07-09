using UnityEngine;
using System.Collections;

public class PathAutoTiling : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//sets the tiling so that it's all made of squares
		gameObject.renderer.material.SetTextureScale("_MainTex", new Vector2(transform.localScale.x, transform.localScale.y));
	}
}
