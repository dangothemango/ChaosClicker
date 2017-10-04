using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperSpinner : MonoBehaviour {

    public int threshold = 1;
    float x, y, z;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        x = y = z = 0;
		if (GameManager.INSTANCE.SuperChaos >= threshold) {
            z = Random.Range(0f, 20f);
        }
        if (GameManager.INSTANCE.SuperChaos >= threshold+2) {
            x = Random.Range(0f, 20f);
            y = Random.Range(0f, 20f);
        }
        transform.Rotate(x, y, z);
    }
}
