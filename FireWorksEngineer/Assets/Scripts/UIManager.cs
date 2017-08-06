using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	private Text _scoreText;
	private Text _timeText;

	// Use this for initialization
	void Start () {
		_scoreText = gameObject.transform.Find("Score/Text").GetComponent<Text>();
		_timeText = gameObject.transform.Find("Time/Text").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		_scoreText.text  = "Score:" + GameManager.Instance.GameScore;
		_timeText.text = "Time:" + (int)GameManager.Instance.GameTime;
	}
}
