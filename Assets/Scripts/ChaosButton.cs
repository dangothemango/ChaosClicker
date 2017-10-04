using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChaosButton : MonoBehaviour {

	public Button button;
	public Sprite[] low_chaos_buttons;
	public Sprite[] med_chaos_buttons;
	public Sprite[] high_chaos_buttons;

	public float x_default_move_speed = 10f;		//How fast does the button move
	public float y_default_move_speed = 10f;		//How fast does the button move
	public float change_move_speed = 20f;			//How much to increase the speed by each click at high chaos

	float x_max_move_speed = 100f;			//How fast *can* the button move
	float x_min_move_speed = 40f;
	float y_max_move_speed = 100f;			//How fast *can* the button move
	float y_min_move_speed = 40f;

	public float wiggle_bounds = 30f;	//Maximum rotation allowed
	public float wiggle_delta = 10f;	//Initial Speed to rotate
	public float wiggle_increase_rate = 30f;	//How much faster to rotate at more chaotic levels
	bool clockwise = true;

	private Image button_sprite;
	private Vector3 button_initial;

	public Camera local_camera;


	// Use this for initialization
	void Start () {
		button_sprite = button.GetComponent<Image> ();
		button_initial = button.transform.position;
		local_camera = local_camera.GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		WiggleButton ();
		RepositionButton ();
	}

	/// <summary>
	/// Triggered on button click -> decides if the button sprite for change, and if so chooses randomly from those available to the current chaos
	/// </summary>
	public void RollToSwapSprite(){
		//Roll to swap the sprites
		if (Random.Range (0, 1) < .25) {	// 1 in 4 chance of changing
			if (GameManager.INSTANCE.Chaos >= .7)				//Pick randomly from high chaos options
				button_sprite.sprite = high_chaos_buttons[(int)(Random.Range(0, high_chaos_buttons.Length))];
			else if (GameManager.INSTANCE.Chaos >= .4)			//Pick randomly from medium chaos options
				button_sprite.sprite = med_chaos_buttons[ (int)(Random.Range(0, med_chaos_buttons.Length))];
			else 												//Pick randomly from low chaos option
				button_sprite.sprite = low_chaos_buttons[(int)(Random.Range(0, low_chaos_buttons.Length))];
		}
	}

	//Moves the button to a new location
	public void RepositionButton(){
		//Max Chaos: Reset the button
		if (GameManager.INSTANCE.Chaos >= 1) {
			ResetButton ();
			return;
		} 

		//At medium-high Chaos, changes the speed of the button on click.

		//Medium Chaos: Move the button around the screen faster & move left & right
		if (GameManager.INSTANCE.Chaos >= .3) {
			// Check if moving in the current direction will render the button offscreen
			if (button.transform.position.x + button.image.rectTransform.rect.width/2 > Screen.width || 
				button.transform.position.x - button.image.rectTransform.rect.width <= 0) {
				x_default_move_speed *= -1f;	//<- Causes things to go wrong.
			}

			//Perform the translation
			button.transform.Translate( new Vector3(x_default_move_speed, 0, 0));
		}

		// Medium-Low Chaos: Move the button up & down on side of screen
		if (GameManager.INSTANCE.Chaos >= .2) {
			// Check if moving in the current direction will render the button offscreen
			if (button.transform.position.y + button.image.rectTransform.rect.height/2 > Screen.height || 
				button.transform.position.y -  button.image.rectTransform.rect.height  <= 0)
				y_default_move_speed *= -1;

			//Perform the translation
			button.transform.Translate( new Vector3(0, y_default_move_speed, 0));
		}

		//Don't move at lower chaos.
	}

	//Wiggle stuff nicely
	public void WiggleButton(){

		//Should we not wiggle at max speed?
		if (GameManager.INSTANCE.Chaos >= 1)
			return;

		float chaotice_wiggle_delta = wiggle_delta + (GameManager.INSTANCE.Chaos * 30);

		//Perform the rotation
		if (clockwise) {
			button.transform.Rotate (0, 0, chaotice_wiggle_delta);
		} else {
			button.transform.Rotate (0, 0, -1 * chaotice_wiggle_delta);
		}
			
		//Reset direction on bounds
		if ( (button.transform.rotation.z * 100 ) >= wiggle_bounds) {
			clockwise = !clockwise;
		} 
		else if ((button.transform.rotation.z * 100) <= wiggle_bounds * -1){
			clockwise = !clockwise;
		}
	}

	//On higher chaos, change the speed of the button on click, and increase the lowest speed at each click
	public void ChangeButtonSpeed(){

		if (GameManager.INSTANCE.Chaos < .6)
			return;

		//Re-roll the current speed
		float new_x_speed = Random.Range (x_min_move_speed, x_max_move_speed);
		float new_y_speed = Random.Range (y_min_move_speed, y_max_move_speed);

		//Ensure the direction doesn't change
		x_default_move_speed = (x_default_move_speed > 0 ) ? new_x_speed : -new_x_speed;
		y_default_move_speed = (y_default_move_speed > 0 ) ? new_y_speed : -new_y_speed;

		//Change the minimum speed to be faster
		x_max_move_speed += change_move_speed;
		x_min_move_speed += change_move_speed/2;
		y_max_move_speed += change_move_speed;
		y_min_move_speed += change_move_speed/2;
	}

	//Resets button to its original position
	public void ResetButton(){
		button.transform.position = button_initial;
	}
}
