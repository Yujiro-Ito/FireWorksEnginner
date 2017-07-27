using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	//--singleton--
	private static GameObject _singletonObject;
	private static GameManager _singleton;
	public static GameManager Instance{
		get{
			if(_singleton == null){
				_singletonObject = new GameObject("GameManager");
				_singletonObject.AddComponent<GameManager>();
				_singleton = _singletonObject.GetComponent<GameManager>();
			}
			return _singleton;
		}
	}

	//----const---
	private const int GAME_TIME = 60;
	private const int COMBO_TIME = 5;

	//---field---
	private float _time;
	private int _score;
	private bool _finish;
	//コンボに使う変数
	private int _combo;
	private float _comboTime;
	
	//---propaties---
	public float GameTime{ get{ return _time; }}
	public int GameScore{ get{ return _score; }}
	public bool GameFinish { get{ return _finish; }}

	// Use this for initialization
	void Awake () {
		//変数の初期化
		_time = 0;
		_score = 0;
		_finish = false;
		_comboTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
		//時間計算
		_time += Time.deltaTime;
		if(_time > GAME_TIME){
			_finish = true;
		}
	}

	//スコアアップ
	public void ScoreUp(int number){
		if(_finish == false){
			_score++;
			_combo++;
		}
	}

	//コンボの経過時間を計算する
	public void ComboTimeElapsed(){
		if(_comboTime > 0 && _finish == false){
			_comboTime -= Time.deltaTime;
		} else {
			_combo = 0;
		}
	}
}
