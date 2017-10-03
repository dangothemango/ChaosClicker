using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle : MonoBehaviour {

    public float speed;

    House target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null) {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }
	}

    public void SetTarget(House h) {
        target = h;
        transform.up = target.transform.position - transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject == target.gameObject) {
            target.OnHit();
            Destroy(this.gameObject);
        }
    }
}
