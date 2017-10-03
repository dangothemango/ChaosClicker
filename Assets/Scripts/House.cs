using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour {

    public Sprite explosion;
    public int state = 0;

    SpriteRenderer ss;

	// Use this for initialization
	void Start () {
        ss = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnHit() {
        state = 1;
        ss.sprite = explosion;
        Road r = GetComponentInParent<Road>();
        r.RemoveHouse(this);
    }
}
