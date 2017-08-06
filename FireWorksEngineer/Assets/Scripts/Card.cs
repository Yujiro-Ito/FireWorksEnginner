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
	private const float MOVE_TIME = 0.8f;

	//--field--
	private CardManager _parent;
	private CardParam _cardParam;
	private cardState _cardState;
	private Vector3 _originPosition;
	private Vector3 _oldMousePosition;
	private Vector3 _newMousePosition;
	private bool _dead;
	private RectTransform _rectTransform;
	private Vector3 _cardSize;
	private Text _cardText;
	private Image _cardImage;
	private Rect _cardRect;
	public GameObject ShotArea;
	private Rect _shotAreaRect;
	private InstanceFirework _insFireworks;

	//--propaties--
	public bool isDead{ get{return _dead; }}
	public CardParam GetCardParam{ get{ return _cardParam; }}
	public Vector3 OriginPosition{ get{ return _originPosition; } set{ _originPosition = value; }}

	//--method--
	//初期化に必要な処理を集めたメソッド
	public void FirstInitializeCard(cardColor color, int cardNumber, CardManager manager){
		//位置や引数の代入
		_parent = manager;
		_originPosition = transform.localPosition;
		transform.localPosition = _parent.transform.localPosition;
		_cardState = cardState.stop;
		InitializeCardParam(color, cardNumber);
		//変数の初期化
		_rectTransform = GetComponent<RectTransform>();
		_cardSize = _rectTransform.sizeDelta;
		_dead = false;
		_cardText = GetComponentInChildren<Text>();
		_cardImage = GetComponent<Image>();
		_cardRect = new Rect(0, 0, 0, 0);
		_insFireworks = FindObjectOfType<InstanceFirework>();
		//色と数値テキストの変更
		ChangeCardColor();
		_cardText.text = _cardParam.StateNumber.ToString();
		//ショットエリアの矩形エリア情報を取得
		RectTransform rt = ShotArea.GetComponent<RectTransform>();
		float x = ShotArea.transform.localPosition.x - rt.sizeDelta.x / 2;
		float y = ShotArea.transform.localPosition.y - rt.sizeDelta.y / 2;
		_shotAreaRect = new Rect(x, y, rt.sizeDelta.x, rt.sizeDelta.y);
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
		_cardText.text = _cardParam.StateNumber.ToString();
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
		if(_cardState == cardState.stay){
			HoldStart();
		}
	}


	//移動状態での処理
	public void MoveStart(){
		_cardState = cardState.move;
		iTween.MoveTo(this.gameObject, iTween.Hash("position", _originPosition, "time", MOVE_TIME, "oncomplete", "MoveComplete", "oncompletetarget", this.gameObject, "isLocal", true));
	}

	public void FinishGame(){
		_cardState = cardState.stop;
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

		Vector3 newPostion = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2);
		transform.localPosition = newPostion;
		//指を離したか確認
		if(Input.GetMouseButtonUp(0)){
			HoldEnd();
		}
	}
	private void HoldEnd(){
		//離した瞬間にほかのカードとあたり判定して消えるか判定する
		Vector3 targetPos;
		Vector3 touchPos = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2);
		for(int i = 0; i < _parent.GetCards.Length; i++){
			//カードの位置を取得して矩形オブジェクトをセット
			targetPos = _parent.GetCards[i].transform.localPosition;
			_cardRect.Set(targetPos.x - _cardSize.x / 2, targetPos.y - _cardSize.y / 2, _cardSize.x, _cardSize.y);
			//自カードは除外
			if(Vector3.Magnitude(targetPos - transform.localPosition) != 0){
				//他カードと当たっていれば他カードとの条件を比較
				if(_cardRect.Contains(touchPos) == true){
					//色と数値が一致しているか
					if(_parent.GetCards[i].GetCardParam.CardColor == _cardParam.CardColor && 
					_parent.GetCards[i].GetCardParam.StateNumber == _cardParam.StateNumber){
						//どちらも一致していれば相手カードを一段階強化して自壊する。
						_parent.GetCards[i].PowerUp();
						_dead = true;	
					} else {
						//色と数値が一致してなければ位置交換
						ChangePosition(_parent.GetCards[i]);
					}
				}
			}
		}

		//どのカードにも当たらなければショットエリアとのあたり判定
		if(_dead == false && _shotAreaRect.Contains(touchPos)){
			GameManager.Instance.ScoreUp(_cardParam.StateNumber);
			_insFireworks.Instance(touchPos, _cardParam.StateNumber, _cardParam.CardColor);
			_dead = true;
		}

		//死んでない（どのカードとも条件が合わなかった）場合、元に戻る
		if(_dead == false) MoveStart();
	}

	//カード情報の初期化
	public void InitializeCardParam(cardColor color, int num){
		_cardParam = new CardParam(color, num);
	}

	//カードの色を変更させる
	private void ChangeCardColor(){
		switch(_cardParam.CardColor){
			case cardColor.red:
				_cardImage.color = Color.red;
				break;
			case cardColor.green:
				_cardImage.color = Color.green;
				break;
			case cardColor.yellow:
				_cardImage.color = Color.yellow;
				break;
		}
	}

	//カードの値を一段階強化する
	private void PowerUp(){
		_cardParam.StateUp();
		_cardText.text = _cardParam.StateNumber.ToString();
	}

	//カードの位置を交換する
	public void ChangePosition(Card changePartner){
		//位置の交換
		Vector3 tmp = changePartner.OriginPosition;
		changePartner.OriginPosition = _originPosition;
		_originPosition = tmp;
		//相手との位置交換開始
		changePartner.MoveStart();
		MoveStart();
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
