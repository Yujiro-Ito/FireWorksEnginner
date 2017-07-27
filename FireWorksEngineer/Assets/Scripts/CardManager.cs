using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour {
	//--const--
	private const int CARD_MAX = 4;
	//--field--
	private Card[] _cards;

	// Use this for initialization
	void Start () {
		//カードの取得と初期化
		_cards = GameObject.Find("Canvas").GetComponentsInChildren<Card>();
		for(int i = 0; i < _cards.Length; i++){
			ResetCard(i, true);
		}
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
		cardColor color = (cardColor)Enum.ToObject(typeof(cardColor), Rand(0, 3));
		int cardNumber = Rand(1, 4);
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
}
