using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {

    public float speed = 5f;
    public float period = 10f;
    public float magnitude = 6f;

    int direction = 0;
    float oY;
    float t=0;

    private void Awake() {
        direction = transform.localPosition.x < 0 ? 1 : -1;
        transform.localRotation = Quaternion.Euler(0, direction > 0 ? 180 : 0, 0);
        oY = transform.localPosition.y;

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        t += Time.deltaTime;
        Vector3 newP = transform.localPosition;
        newP.x += direction * speed * Time.deltaTime;
        newP.y = oY + Mathf.Sin(period*t)/magnitude;
        transform.localPosition = newP;
	}
}
