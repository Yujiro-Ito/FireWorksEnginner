using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour {
	//--const--
	private const float START_TIME = 4;

	//--field--
	private Text _start;
	private Text _finish;
	private float _startCount;

	// Use this for initialization
	void Start () {
		_start = transform.Find("Start").GetComponent<Text>();
		_finish = transform.Find("Finish").GetComponent<Text>();
		_startCount = START_TIME;
		GameManager.Instance.AddFinishAction("finish", Finish);
		StartCoroutine(StartCount());
	}

	private void Finish(){
		_finish.enabled = true;
		StartCoroutine(SceneSwitch());
	}

	private IEnumerator SceneSwitch(){
		yield return new WaitForSeconds(3);
		SceneManager.LoadScene("Result");
	}

	private IEnumerator StartCount(){
		while(true){
			_startCount -= Time.deltaTime;
			_start.text = (int)_startCount + "";
			if(_startCount <= 1){
				GameManager.Instance.GameStart();
				_start.enabled = false;
				yield break;
			}
			yield return new WaitForEndOfFrame();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
