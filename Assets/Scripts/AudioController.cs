using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

	public AudioClip thunder_clap;
	public AudioClip missle_strike;
	public AudioClip door_shut;
	public AudioClip car_horn;
	public AudioClip[] screams;
	public float volume = 1f;

	AudioSource source;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void playThunder(){
		source.PlayOneShot (thunder_clap, volume);
	}

	public void playMissle(){
		source.PlayOneShot (missle_strike, volume);
	}

	public void playDoorClose(){
		source.PlayOneShot (door_shut, volume);
	}

	public void playCarHorn(){
		source.PlayOneShot (car_horn, volume);
	}

	public void playScream(){
		source.PlayOneShot (screams [(int)Random.Range (0, screams.Length)], volume);
	}
}
