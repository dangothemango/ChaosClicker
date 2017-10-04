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
	AudioSource source;
	float honk_timer = 10f;
	float current_honk_timer;

    private void Awake() {
        direction = transform.localPosition.x < 0 ? 1 : -1;
        transform.localRotation = Quaternion.Euler(0, direction > 0 ? 180 : 0, 0);
        oY = transform.localPosition.y;
        rigidBody = GetComponent<Rigidbody2D>();

		//Initialize audio source
		source = GameObject.Find ("Audio_Main").GetComponent<AudioSource>();

		//Setup timers for checking if a car honks
		honk_timer += Random.Range (-5, 5) + GameManager.INSTANCE.Chaos;
		current_honk_timer = honk_timer;
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

		//Reset timer & honk on cooldown
		if (current_honk_timer <= 0) {
			CheckHornHonk ();
			current_honk_timer = honk_timer;
		}

		//Decrement timer
		current_honk_timer -= Time.fixedDeltaTime;
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

	public void CheckHornHonk(){
		if (Random.Range (0, 1.5f) < .5f + GameManager.INSTANCE.Chaos)
			return;
		
		source.GetComponent<AudioController> ().playCarHorn ();
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
