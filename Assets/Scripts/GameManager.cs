﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager INSTANCE;

    float chaos = 0;

    float Chaos {
        get {
            return chaos;
        }
        set {
            chaos = value;
        }
    }

	// Use this for initialization
	void Start () {
		if (INSTANCE != null) {
            this.enabled = false;
            return;
        }
        INSTANCE = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
