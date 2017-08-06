using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObj : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(DestroyObject());
	}

	private IEnumerator DestroyObject(){
		yield return new WaitForSeconds(5.5f);
		Destroy(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
