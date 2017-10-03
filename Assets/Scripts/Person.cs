using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour {

    public float speed = 1f;
    public float period = 1f;
    public float magnitude = 1f;
    public float scaleTime = 1f;

    House target;
    bool seeking = false;
    float oRot;
    float t = 0;

    public SpriteRenderer Body;

    [Header("Sprites")]
    public Sprite surprised;
    public Sprite[] confused;

	// Use this for initialization
	void Start () {
        StartCoroutine(ScaleUp(scaleTime));
        oRot = transform.localRotation.eulerAngles.z;
        GameManager.INSTANCE.AddPerson(this);
    }
	
	// Update is called once per frame
	void Update () {
		if (target != null && seeking) {
            if (target.state == 1) {
                //TODO: Go Nuts
                return;
            }
            t += Time.deltaTime;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition,target.transform.localPosition,speed*Time.deltaTime);
            Vector3 rot = transform.localRotation.eulerAngles;
            rot.z = (oRot + Mathf.Sin(period * t) / magnitude);
            transform.localRotation = Quaternion.Euler(rot);
            if (Vector2.Distance(transform.position,target.transform.position) < .1f) {
                seeking = false;
                StartCoroutine(ScaleDown(scaleTime));
            }
        }
	}

    public bool SetTarget(House h) {
        if (h.transform.position == transform.position) {
            return false;
        } else {
            target = h;
            transform.localRotation = Quaternion.Euler(0, target.transform.position.x-transform.position.x > 0 ? 180 : 0, 0);
            return true;
        }
    }

    IEnumerator ScaleUp(float scaleTime) {
        Vector2 oScale = transform.localScale;
        float sT = 0;
        while (sT < scaleTime) {
            sT += Time.deltaTime;
            transform.localScale = Vector2.Lerp(Vector2.zero, oScale, sT / scaleTime);
            yield return null;
        }
        seeking = true;
    }

    IEnumerator ScaleDown(float scaleTime) {
        Vector2 oScale = transform.localScale;
        float sT = 0;
        while (sT < scaleTime) {
            sT += Time.deltaTime;
            transform.localScale = Vector2.Lerp(oScale, Vector3.zero, sT / scaleTime);
            yield return null;
        }
        Destroy(this.gameObject);
    }

    private void OnDestroy() {
        GameManager.INSTANCE.RemovePerson(this);
    }

    public void OnHit() {
        seeking = false;
        StartCoroutine(SusAndDie());
    }

    IEnumerator SusAndDie() {
        Body.sprite = surprised;
        yield return ScaleDown(.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponentInParent<Cloud>() != null) {
            Body.sprite = confused[Random.Range(0,confused.Length)];
        }
    }
}
