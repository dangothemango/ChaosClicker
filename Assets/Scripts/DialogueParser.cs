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

	public string[] Get_Dialogue_Options(string v){
        switch (v) {
            case "neutral":
                return dialogue.neutral;
            case "confusion":
                return dialogue.confusion;
            case "house_fire":
                return dialogue.house_fire;
            case "house_lightning":
                return dialogue.house_lightning;
            case "house_explosion":
                return dialogue.house_explosion;
            case "house_missile":
                return dialogue.house_missile;
            case "car_fire":
                return dialogue.car_fire;
            case "car_lightning":
                return dialogue.car_lightning;
            case "car_explosion":
                return dialogue.car_explosion;
            case "car_missile":
                return dialogue.car_missile;
            case "person_fire":
                return dialogue.person_fire;
            case "person_lightning":
                return dialogue.person_lightning;
            case "person_explosion":
                return dialogue.person_explosion;
            case "person_missile":
                return dialogue.person_missile;
            case "sentient":
                return dialogue.sentient;
            default:
                return new string[0];
        }
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
    
    public void DespawnBubble() {
        StartCoroutine(ScaleTooltip(-1));
    }

    public void SpawnBubble() {
        StartCoroutine(ScaleTooltip(1));
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

		interactTooltip.transform.localScale = destScale;
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
