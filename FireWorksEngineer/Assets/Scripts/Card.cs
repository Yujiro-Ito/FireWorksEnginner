using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {
	//--enum--
	private enum cardState{
		stop = 0,
		stay,
		move,
		hold
	}

	//--const--
	private const float MOVE_TIME = 1f;

	//--field--
	private CardManager _parent;
	private CardParam _cardParam;
	private cardState _cardState;
	private Vector3 _originPosition;
	private Vector3 _oldMousePosition;
	private Vector3 _newMousePosition;
	private bool _dead;
	private RectTransform _rectTransform;
	private Vector3 _mySize;
	private Text _myText;
	private Image _myImage;

	//--propaties--
	public bool isDead{ get{return _dead; }}

	//--method--
	//初期化に必要な処理を集めたメソッド
	public void FirstInitializeCard(cardColor color, int cardNumber, CardManager manager){
		//位置や引数の代入
		_parent = manager;
		_originPosition = transform.localPosition;
		transform.localPosition = _parent.transform.localPosition;
		MoveStart();
		InitializeCardParam(color, cardNumber);
		//変数の初期化
		_rectTransform = GetComponent<RectTransform>();
		_mySize = _rectTransform.sizeDelta;
		_dead = false;
		_myText = GetComponentInChildren<Text>();
		_myImage = GetComponent<Image>();
		//色と数値テキストの変更
		ChangeCardColor();
		_myText.text = _cardParam.StateNumber.ToString();
	}

	public void InitializeCard(cardColor color, int cardNumber){
		//位置や引数の代入
		transform.localPosition = _parent.transform.localPosition;
		MoveStart();
		InitializeCardParam(color, cardNumber);
		//変数の初期化
		_dead = false;
		//色と数値テキストの変更
		ChangeCardColor();
		_myText.text = _cardParam.StateNumber.ToString();
	}
	
	// Update is called once per frame
	public void StateUpdate () {
		//カードの状態に応じて処理
		switch(_cardState){
			case cardState.stop:
				Stop();
				break;
			case cardState.stay:
				Stay();
				break;
			case cardState.move:
				Move();
				break;
			case cardState.hold:
				Hold();
				break;
		}
	}

	//止まっている状態での処理
	private void Stop(){}

	//停滞状態での処理
	private void Stay(){}
	//押したときのイベントハンドラ
	public void Click(){
		HoldStart();
	}


	//移動状態での処理
	private void MoveStart(){
		_cardState = cardState.move;
		iTween.MoveTo(this.gameObject, iTween.Hash("position", _originPosition, "time", MOVE_TIME, "oncomplete", "MoveComplete", "oncompletetarget", this.gameObject, "isLocal", true));
	}

	private void Move(){
	}

	private void MoveComplete(){
		_cardState = cardState.stay;
	}

	//保持状態での処理
	private void HoldStart(){
		transform.SetAsLastSibling();  //ヒエラルキーの一番下にする
		_cardState = cardState.hold;
		_oldMousePosition = Input.mousePosition;
		_oldMousePosition.z = 10f;
	}
	private void Hold(){
		//オブジェクトの移動
		_newMousePosition = Input.mousePosition;
		_newMousePosition.z = 10f;

		transform.localPosition = (Input.mousePosition) * 2 - new Vector3(Screen.width / 2, Screen.height / 2, 0) - _mySize;
		//指を離したか確認
		if(Input.GetMouseButtonUp(0)){
			HoldEnd();
		}
	}
	private void HoldEnd(){
		//離した瞬間にほかのカードとあたり判定して消えるか判定する
		int rand = Random.Range(0, 2);
		if(rand == 0){
			MoveStart();
		} else {
			_dead = true;
		}
	}

	//カード情報の初期化
	public void InitializeCardParam(cardColor color, int num){
		_cardParam = new CardParam(color, num);
	}

	//カードの色を変更させる
	private void ChangeCardColor(){
		switch(_cardParam.CardColor){
			case cardColor.red:
				_myImage.color = Color.red;
				break;
			case cardColor.green:
				_myImage.color = Color.green;
				break;
			case cardColor.yellow:
				_myImage.color = Color.yellow;
				break;
		}
	}
}




//カードの情報を持つデータクラス
public class CardParam{
	//---field---
	private cardColor _cardColor;
	private int _num;

	//---propaties---
	public cardColor CardColor { get{ return _cardColor; }}
	public int StateNumber{ get{ return _num; }}

	//---constract---
	public CardParam(cardColor color, int num){
		_cardColor = color;
		_num = num;
	}

	//---method---
	public void StateUp(){
		_num++;
	}
}

//カードの色情報
public enum cardColor{
	red,
	green,
	yellow
}
