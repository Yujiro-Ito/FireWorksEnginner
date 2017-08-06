using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour {
	//--const--
	public const int CARD_NUM_MAX = 3; //カードが持つパワーの値の最大値(+1)
	public const int CARD_COLOR_MAX = 3;

	//--field--
	private Card[] _cards;
	public Card[] GetCards{ get{ return _cards; }}

	// Use this for initialization
	void Start () {
		//カードの取得と初期化
		_cards = GameObject.Find("Canvas").GetComponentsInChildren<Card>();
		for(int i = 0; i < _cards.Length; i++){
			ResetCard(i, true);
		}
		GameManager.Instance.AddStartAction("cardStart", GameStart);
		GameManager.Instance.AddFinishAction("cardFinish", FinishGame);
	}
	
	// Update is called once per frame
	void Update () {
		//カード全体の更新処理
		for(int i = 0 ; i < _cards.Length; i++){
			//更新処理
			_cards[i].StateUpdate();
			//死んでたら初期化
			if(_cards[i].isDead == true){
				ResetCard(i, false);
			}
		}
	}

	//カードの初期化関数
	void ResetCard(int suffix, bool first){
		cardColor color = (cardColor)Enum.ToObject(typeof(cardColor), Rand(0, CARD_COLOR_MAX));
		int cardNumber = Rand(1, CARD_NUM_MAX);
		if(first){
			_cards[suffix].FirstInitializeCard(color, cardNumber, this);
		} else {
			_cards[suffix].InitializeCard(color, cardNumber);
		}
	}

	//ランダム関数
	int Rand(int min, int max){
		int result = UnityEngine.Random.Range(min, max);
		return result;
	}

	//ゲームスタート時にカードをアクティブにする
	public void GameStart(){
		for(int i = 0; i < _cards.Length; i++){
			_cards[i].MoveStart();
		}
	}

	public void FinishGame(){
		for(int i = 0; i < _cards.Length; i++){
			_cards[i].FinishGame();
		}
	}
}
