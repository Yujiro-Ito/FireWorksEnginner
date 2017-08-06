using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour {
	private bool _once = false;
	public Light mainLight;
	public void Click(){
		if(_once == false){
			Hashtable hash = new Hashtable();
			hash.Add("from", 50);
			hash.Add("to", -11);
			hash.Add("time", 2.5f);
			hash.Add("onupdate", "RotCamera");
			hash.Add("onupdatetarget", this.gameObject);
			hash.Add("oncomplete", "SceneSwitch");
			hash.Add("oncompletetarget", this.gameObject);
			iTween.ValueTo(this.gameObject, hash);
			_once = true;
		}
	}

	private void RotCamera(float num){
		mainLight.transform.rotation = Quaternion.Euler(num, 0, 0);
	}

	private void SceneSwitch(){
		SceneManager.LoadScene("Game");
		GameManager.Instance.InitializeGame();
	}
}
