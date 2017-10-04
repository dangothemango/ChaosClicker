using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {

    public float speed = 5f;
    public float period = 10f;
    public float magnitude = 6f;
    public float launchSpeed = 4f;

    int direction = 0;
    float oY;
    float t = 0;
    Rigidbody2D rigidBody;
    bool dying;

    private void Awake() {
        direction = transform.localPosition.x < 0 ? 1 : -1;
        transform.localRotation = Quaternion.Euler(0, direction > 0 ? 180 : 0, 0);
        oY = transform.localPosition.y;
        rigidBody = GetComponent<Rigidbody2D>();

    }

    // Use this for initialization
    void Start() {
        GameManager.INSTANCE.AddCar(this);
    }

    // Update is called once per frame
    void Update() {
        if (dying) return;
        t += Time.deltaTime;
        Vector3 newP = transform.localPosition;
        newP.x += direction * speed * Time.deltaTime;
        newP.y = oY + Mathf.Sin(period * t) / magnitude;
        transform.localPosition = newP;
    }

    public void OnBecameInvisible() {
        Destroy(this.gameObject);
    }

    private void OnDestroy() {
        GameManager.INSTANCE.RemoveCar(this);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<Explosion>() != null) {
            rigidBody.velocity = (transform.position - collision.transform.position.normalized) * launchSpeed;
            StartCoroutine(LaunchAndDie(2f));
        }
    }

    IEnumerator LaunchAndDie(float t) {
        dying = true;
        float eT = 0;
        while (eT < t) {
            eT += Time.deltaTime;
            transform.Rotate(0, 0, 180 * Time.deltaTime);
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
