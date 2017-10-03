using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {

    public float speed = 2f;

    Transform target;

    Transform Target{
        get {
        return target;
        } set {
            target = value;
        }
    }

    Vector2 direction;

	// Use this for initialization
	void Start () {
        float xV = Random.Range(.2f, .8f);
        float yV = 1 - xV;
        direction = new Vector2(transform.position.x < 0 ? xV : -1 * xV, transform.position.y < 0 ? yV : -1 * yV).normalized;
        GameManager.INSTANCE.AddCloud(this);
	}
	
	// Update is called once per frame
	void Update () {
        if (target != null) {

        } else if (direction != null) {
            transform.position += (Vector3)(direction * Time.deltaTime * speed);
        }
	}

    private void OnBecameInvisible() {
        Destroy(this.gameObject);
    }

    private void OnDestroy() {
        GameManager.INSTANCE.RemoveCloud(this);
    }
}
