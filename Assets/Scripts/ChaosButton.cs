using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChaosButton : MonoBehaviour {

	public Button button;
	public Sprite[] low_chaos_buttons;
	public Sprite[] med_chaos_buttons;
	public Sprite[] high_chaos_buttons;

	public float wiggle_bounds = 90f;
	public float wiggle_delta = 10f;
	bool clockwise = true;

	private Image button_sprite;
	private Vector3 button_initial;


	// Use this for initialization
	void Start () {
		button_sprite = button.GetComponent<Image> ();
		button_initial = button.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		WiggleButton ();
	}

	public void RollToSwapSprite(){
		float roll = Random.Range (1, 100);

		//Roll to swap the sprites
		if (Random.Range (1, 100) < 50) {

			if (GameManager.INSTANCE.Chaos >= .7) {
				button_sprite.sprite = high_chaos_buttons[(int)(Random.Range(0, high_chaos_buttons.Length))];
				print ("High chaos choice");
			} else if (GameManager.INSTANCE.Chaos >= .4) {
				button_sprite.sprite = med_chaos_buttons[ (int)(Random.Range(0, med_chaos_buttons.Length))];
				print ("Med chaos choice");

			} else {
				button_sprite.sprite = low_chaos_buttons[(int)(Random.Range(0, low_chaos_buttons.Length))];
				print ("Low chaos choice");
			}
		}
	}

	//Moves the button to a new location
	public void RepositionButton(){
		if (Random.Range (0, 5) > 4) {

			//Swap position of button again
			if (GameManager.INSTANCE.Chaos >= .7) {
					
			} else if (GameManager.INSTANCE.Chaos >= .4) {
				
			} else {
				
			}

		}
	}

	//Wiggle stuff nicely
	public void WiggleButton(){

		//Perform the rotation
		if (clockwise) {
			button.transform.Rotate (0, 0, wiggle_delta * Time.deltaTime);
		} else {
			button.transform.Rotate (0, 0, -1 * wiggle_delta * Time.deltaTime);
		}
			
		//Reset direction on bounds
		if ( (button.transform.rotation.z * 100 ) >= wiggle_bounds) {
			clockwise = !clockwise;
		} 
		else if ((button.transform.rotation.z * 100) <= wiggle_bounds * -1){
			clockwise = !clockwise;
		}


	}

	//Resets button to its original position
	public void ResetButton(){
		button.transform.position = button_initial;
	}
}
