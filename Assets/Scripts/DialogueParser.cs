using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueParser : MonoBehaviour {

	public TextAsset dialogFile = null;
	public GameObject interactTooltip;
	public Canvas UI;

	Vector3 tooltipOScale;
	private Dialogue dialogue;
	Coroutine interactionCoroutine;


	void Awake() {
		if (dialogFile != null) {
			dialogue = JsonUtility.FromJson<Dialogue>(dialogFile.text);
		}

		tooltipOScale = interactTooltip.transform.localScale;
		interactTooltip.transform.localScale = Vector3.zero;
	}

	public Dialogue Get_Dialogue_Options(){
		return dialogue;
	}

	public void SpawnBubbles(Person parent){
		print ("Trying to spawn bubbles");

		if (interactionCoroutine != null) {
			StopCoroutine (interactionCoroutine);
		}

		interactionCoroutine = StartCoroutine(ScaleTooltip(1));

		//Instantiate (interactTooltip, parent.transform);
		interactTooltip.SetActive (true);
		interactTooltip.transform.position = parent.transform.position;
	}

	IEnumerator ScaleTooltip(int dir) {
		float t = 0;
		Vector3 startScale = interactTooltip.transform.localScale;
		Vector3 destScale = dir == 1 ? tooltipOScale : Vector3.zero;
		while (t <= .25f) {
			interactTooltip.transform.localScale = Vector3.Lerp(startScale, destScale, t / .25f);
			t+= Time.deltaTime;
			yield return null;
		}
	}

	[System.Serializable]
	public class Dialogue{
		public string[] neutral;
		public string[] confusion;
		public string[] house_fire;
		public string[] house_lightning;
		public string[] house_explosion;
		public string[] house_missile;
		public string[] car_fire;
		public string[] car_lightning;
		public string[] car_explosion;
		public string[] car_missile;
		public string[] person_fire;
		public string[] person_lightning;
		public string[] person_explosion;
		public string[] person_missile;
		public string[] sentient;
	}
}
