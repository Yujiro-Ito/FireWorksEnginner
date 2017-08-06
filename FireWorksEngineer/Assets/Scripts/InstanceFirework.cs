using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InstanceFirework : MonoBehaviour {
	GameObject _prefab;

	// Use this for initialization
	void Start () {
		_prefab = Resources.Load("Prefabs/FireWorks") as GameObject;
	}

	public void Instance(Vector3 pos, int cardPower, cardColor color){
		if(cardPower > 2){
			switch(GameManager.Instance.ComboCount){
				default:
				case 0:
					StartCoroutine(FireWorksSetup(pos, color, 0));
					break;
				case 1:
				case 2:
					StartCoroutine(FireWorksSetup(pos, color, 0));
					pos.x += 5;
					StartCoroutine(FireWorksSetup(pos, color, 1));
					break;
				case 3:
				case 4:
				case 5:
				case 6:
					float interval = 100f;
					for(int i = 0; i < 3; i++){
						StartCoroutine(FireWorksSetup(pos, color, 0));
						pos.x += interval;
					}
					pos.x -= interval / 2;
					for(int i = 0; i < 3; i++){
						StartCoroutine(FireWorksSetup(pos, color, 1));
						pos.x -= interval;
					}
					break;
			}
		} else {
			StartCoroutine(FireWorksSetup(pos, color, 0));
		}
	}

	public IEnumerator FireWorksSetup(Vector3 pos, cardColor col, float time){
		yield return new WaitForSeconds(time);
		//生成
		pos.x += Screen.width / 2;
		pos *= 0.1f;
		pos.z = 65;
		GameObject obj = (GameObject)Instantiate(_prefab, pos, Quaternion.Euler(0, -90, 0));
		//パーティクルを再生する
		ParticleSystem sys = obj.transform.Find("Start").GetComponent<ParticleSystem>();
		ParticleAction(()=>{
			sys.Simulate(2f, true, false);
			sys.Play();
		});
		//色を変更
		ColorSetup(sys.transform.Find("Exprosion/Trail").GetComponent<ParticleSystem>(), col);
	}

	public void ColorSetup(ParticleSystem sys, cardColor color){
		//色専用の変数
		Gradient grad = new Gradient();
		//カードの色によって分ける
		switch(color){
			case cardColor.red:
				grad.SetKeys(
					new GradientColorKey[]{
						new GradientColorKey(Color.gray, 0.0f),
						new GradientColorKey(Color.red, 0.5f),
						new GradientColorKey(Color.gray, 1.0f)
					},
					new GradientAlphaKey[]{
						new GradientAlphaKey(0.2f, 0.0f),
						new GradientAlphaKey(1.0f, 1f)
					}
				);
				break;
			case cardColor.green:
				grad.SetKeys(
					new GradientColorKey[]{
						new GradientColorKey(Color.gray, 0.0f),
						new GradientColorKey(Color.green, 0.5f),
						new GradientColorKey(Color.gray, 1.0f)
					},
					new GradientAlphaKey[]{
						new GradientAlphaKey(0.2f, 0.0f),
						new GradientAlphaKey(1.0f, 1f)
					}
				);
				break;
			case cardColor.yellow:
				grad.SetKeys(
					new GradientColorKey[]{
						new GradientColorKey(Color.gray, 0.0f),
						new GradientColorKey(Color.yellow, 0.5f),
						new GradientColorKey(Color.gray, 1.0f)
					},
					new GradientAlphaKey[]{
						new GradientAlphaKey(0.2f, 0.0f),
						new GradientAlphaKey(1.0f, 1f)
					}
				);
				break;
		}
		var col = sys.colorOverLifetime;
		col.color = grad;
	}

	public void ParticleAction(Action act){
		act();
	}
}
