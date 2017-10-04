using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueParser : MonoBehaviour {

	public TextAsset dialogFile = null;
//	public GameObject interactTooltip;

	Vector3 tooltipOScale;
	private Dialogue dialogue;

	void Awake() {
		if (dialogFile != null) {
			dialogue = JsonUtility.FromJson<Dialogue>(dialogFile.text);
			print (dialogue);
		}

//		tooltipOScale = interactTooltip.transform.localScale;
//		interactTooltip.transform.localScale = Vector3.zero;
	}

	public Dialogue Get_Dialogue_Options(){
		return dialogue;
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
