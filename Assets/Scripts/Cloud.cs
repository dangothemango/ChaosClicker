using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {

    public float speed = 2f;
    public SpriteRenderer lightning;
    int age = 1;

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
        if (target == null && (Input.GetKeyDown(KeyCode.L) || Random.Range(0,1.0f) <= GameManager.INSTANCE.Chaos/Mathf.Pow(19,age))) {
            target = GameManager.INSTANCE.GetRandomPerson().transform;
            
        }
        if (target != null) {
            if (Vector2.Distance(transform.position, target.position + new Vector3(.397004f, 1.521376f, 0)) < .1f) {
                StartCoroutine(LightningStrike());
                target.GetComponent<Person>().OnHit();
                age++;
                target = null;
            }
            else {
                transform.position = Vector2.MoveTowards(transform.position, target.position + new Vector3(.397004f, 1.521376f, 0), speed * Time.deltaTime * 5);
            }
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

    IEnumerator LightningStrike() {
        lightning.gameObject.SetActive(true);
        yield return new WaitForSeconds(.2f);
        lightning.gameObject.SetActive(false);
    }
}
