using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle : MonoBehaviour {

    public float speed;
    public GameObject explosion;

    Transform target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null) {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            transform.up = target.position - transform.position;
        } else {
            target = GameManager.INSTANCE.GetRandomPerson().transform;
        }
	}

    public void SetTarget(House h) {
        if (h == null) {
            target = GameManager.INSTANCE.GetRandomPerson().transform;
        }
        else {
            target = h.transform;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (target == null) return;
        if (collision.gameObject == target.gameObject) {
            Person p = target.GetComponent<Person>();
            if (p != null) {
                p.onMissileHit();
                Instantiate(explosion, target.position, new Quaternion(), GameManager.INSTANCE.gameObject.transform);
            }
            else {
                House h = target.GetComponent<House>();
                h.OnHit();
            }
            Destroy(this.gameObject);
        }
    }
}
