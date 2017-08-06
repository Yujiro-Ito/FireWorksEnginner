using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour {
	//--singleton--
	private static GameObject _singletonObject;
	private static GameManager _singleton;
	public static GameManager Instance{
		get{
			if(_singleton == null){
				_singletonObject = new GameObject("GameManager");
				DontDestroyOnLoad(_singletonObject);
				_singletonObject.AddComponent<GameManager>();
				_singleton = _singletonObject.GetComponent<GameManager>();
			}
			return _singleton;
		}
	}

	//----const---
	private const int GAME_TIME = 10;
	private const int COMBO_TIME = 3;
	private int[] _scoreList = {100, 150, 300, 600, 1400, 3000, 9000, 18000, 10000000};
	private float[,] _comboList = {{0, 1f}, {1, 1.01f}, {2, 1.02f}, {3, 1.04f}, {4, 1.07f}, {5, 1.09f}, {6, 1.11f}, {7, 1.2f}, {8, 1.4f}, {15, 1.6f}, {30, 2f}};
	private Dictionary<string, Action> _startAction;
	private Dictionary<string, Action> _finishAction;

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
	public int ComboCount{ get{ return _combo; }}
	public float ComboTime{ get{ return _comboTime; }}

	// Use this for initialization
	void Awake () {
		_finish = true;
		InitializeGame();
		_startAction = new Dictionary<string, Action>();
		_finishAction = new Dictionary<string, Action>();
	}

	public void InitializeGame(){
		//変数の初期化
		_time = GAME_TIME;
		_score = 0;
		_comboTime = 0;
	}

	public void GameStart(){
		_finish = false;
		foreach(KeyValuePair<string, Action> act in _startAction){
			act.Value();
		}
	}
	
	// Update is called once per frame
	void Update () {
		//時間計算
		if(_finish == false){
			_time -= Time.deltaTime;
			if(_time < 0){
				_finish = true;
				//アクションを実行
				foreach(KeyValuePair<string, Action> act in _finishAction){
					act.Value();
				}
			}
			ComboTimeElapsed();
		}
	}

	//スコアアップ
	public void ScoreUp(int number){
		if(_finish == false){
			//スコア計算
			int score = _scoreList[number - 1];
			for(int i = 0; i < _comboList.Length; i++){
				//コンボによる加算
				if(_comboList[i, 0] >= _combo){
					score = (int)(score *  _comboList[i, 1]);
					break;
				}
			}
			score += UnityEngine.Random.Range(-score / 10, score / 10);
			_score += score;
			//コンボ計算
			if(number >= 3){
				_comboTime = COMBO_TIME;
				_combo++;
			}
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

	//始まった時のデリゲートを登録する
	public void AddStartAction(string key, Action value){
		//無ければ追加する
		if(_startAction.ContainsKey(key) == false){
			_startAction.Add(key, value);
		} else {
			//あれば更新
			_startAction[key] = value;
		}
	}

	//終わった時のデリゲートを登録する
	public void AddFinishAction(string key, Action value){
		//無ければ追加する
		if(_finishAction.ContainsKey(key) == false){
			_finishAction.Add(key, value);
		} else {
			//あれば更新
			_finishAction[key] = value;
		}
	}
}
