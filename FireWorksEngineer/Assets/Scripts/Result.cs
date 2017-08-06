using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour {
	private Text _score;

	void Start(){
		transform.Find("Score").GetComponent<Text>().text = "Score:" + GameManager.Instance.GameScore;
	}
	
	public void GoTitle(){
		SceneManager.LoadScene("Title");
	}

	public void OneMore(){
		GameManager.Instance.InitializeGame();
		SceneManager.LoadScene("Game");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
