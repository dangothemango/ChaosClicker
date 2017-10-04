using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChaosButton : MonoBehaviour {

	public Button button;
	public Sprite[] low_chaos_buttons;
	public Sprite[] med_chaos_buttons;
	public Sprite[] high_chaos_buttons;

	private Image button_sprite;

	// Use this for initialization
	void Start () {
		button_sprite = button.GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RollToSwapSprite(){
		float roll = Random.Range (1, 100);

		//Roll to swap the sprites
		if (Random.Range (1, 100) < 50) {

			if (GameManager.INSTANCE.Chaos >= .7) {
				button_sprite.sprite = high_chaos_buttons[(int)(Random.Range(0, high_chaos_buttons.Length))];
				print ("High chaos choice");
			} else if (GameManager.INSTANCE.Chaos >= .5) {
				button_sprite.sprite = med_chaos_buttons[ (int)(Random.Range(0, med_chaos_buttons.Length))];
				print ("Med chaos choice");

			} else {
				button_sprite.sprite = low_chaos_buttons[(int)(Random.Range(0, low_chaos_buttons.Length))];
				print ("Low chaos choice");

			}

		}
	}
}
