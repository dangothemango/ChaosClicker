using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour {

    public float speed = 1f;
    public float period = 1f;
    public float magnitude = 1f;
    public float scaleTime = 1f;
    public float launchSpeed = 10f;

    House target;
    bool seeking = false;
    float oRot;
    float t = 0;
    int direction = 1;
    Rigidbody2D rigidBody;
    bool sentient = false;
    bool scaling = false;

    bool isOnFire = false;
    float fireTimer = 0;

    public SpriteRenderer Body;
    public SpriteRenderer Floater;

    [Header("Sprites")]
    public Sprite surprised;
    public Sprite[] confused;
    public Sprite questionMark;
    public Sprite exclamationMark;
    public Sprite fire;

   

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody2D>();
        StartCoroutine(ScaleUp(scaleTime));
        oRot = transform.localRotation.eulerAngles.z;
        GameManager.INSTANCE.AddPerson(this);
    }
	
	// Update is called once per frame
	void Update () {
        t += Time.deltaTime;
        if (seeking) {
            if (target != null && target.state == 0) {
                if (target.state == 0) {
                    transform.localPosition = Vector3.MoveTowards(transform.localPosition, target.transform.localPosition, speed * Time.deltaTime);
                    Vector3 rot = transform.localRotation.eulerAngles;
                    rot.z = (oRot + Mathf.Sin(period * t) / magnitude);
                    transform.localRotation = Quaternion.Euler(rot);
                    if (Vector2.Distance(transform.position, target.transform.position) < .1f) {
                        seeking = false;
                        StartCoroutine(ScaleDown(scaleTime));
                    }
                }
            }
            else {
                if (Random.Range(0, 1.0f) > .99845f) {
                    direction *= -1;
                    transform.localRotation = Quaternion.Euler(0, direction > 0 ? 180 : 0, 0);
                }
                transform.localPosition += direction * Vector3.right * speed * 2 * Time.deltaTime;
                Vector3 rot = transform.localRotation.eulerAngles;
                rot.z = (oRot + Mathf.Sin(period * 2 * t) / magnitude);
                transform.localRotation = Quaternion.Euler(rot);
            }
        }

        if (isOnFire)
        {
            if (Random.Range(0, 1.0f) > .9f)
            {
                direction *= -1;
            }
            //if the fire should no longer be updated
            if (!updateFire())
            {
                print("should be done");
                //ScaleDown(.5f);
                seeking = false;

                Color c = Body.color;
                c.a -= Time.deltaTime;
                Body.color = c;
                print(c.a);
                if (c.a <= 0)
                {
                    print("destroyed?");
                    Destroy(this.gameObject);
                }
            }
        }
    }

    public bool SetTarget(House h) {
        if (h == null) {
            target = null;
            seeking = true;
            direction = transform.localPosition.x < 0 ? 1 : -1;
            transform.localRotation = Quaternion.Euler(0, direction > 0 ? 180 : 0, 0);
            return true;
        }
        if (h.transform.position == transform.position) {
            return false;
        } else {
            target = h;
            direction = target.transform.position.x - transform.position.x > 0 ? 1 : -1;
            transform.localRotation = Quaternion.Euler(0, direction > 0 ? 180 : 0, 0);
            return true;
        }
    }

    IEnumerator ScaleUp(float scaleTime) {
        scaling = true;
        Vector2 oScale = transform.localScale;
        float sT = 0;
        while (sT < scaleTime) {
            sT += Time.deltaTime;
            transform.localScale = Vector2.Lerp(Vector2.zero, oScale, sT / scaleTime);
            yield return null;
        }
        seeking = true;
        scaling = false;
    }

    IEnumerator ScaleDown(float scaleTime) {
        if (sentient) yield break;
        scaling = true;
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


    public void onMissileHit()
    {
        if (sentient) return;
        seeking = false;
        StartCoroutine(SusAndDie());
    }

    public void OnHit() {
        if (sentient) return;
        seeking = false;
        StartCoroutine(SusAndDie());
    }

    public void OnLightningHit()
    {
        if (sentient) return;
        isOnFire = true;
        //seeking = false;
        target = null;
        speed = 2f;
        //StartCoroutine(SusAndDie());
    }

    //returns true while the player alive and the fire should be updated
    private bool updateFire()
    {
        fireTimer += Time.deltaTime;
        print("fireTimer: " + fireTimer);

        return fireTimer < 5f;
    }

    IEnumerator SusAndDie() {
        if (sentient) yield break;
        Body.sprite = surprised;
        yield return ScaleDown(.5f);
    }

    IEnumerator LaunchAndDie(float t) {
        if (sentient) yield break;
        float eT = 0;
        while (eT < t) {
            eT += Time.deltaTime;
            transform.Rotate(0, 0, 180 * Time.deltaTime);
            yield return null;
        }
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (sentient) return;
        if (collision.gameObject.GetComponentInParent<Cloud>() != null) {
            Body.sprite = confused[Random.Range(0,confused.Length)];
        } else if (collision.gameObject.GetComponent<Explosion>() != null){
            seeking = false;
            rigidBody.velocity = (transform.position - collision.transform.position.normalized) * launchSpeed;
            StartCoroutine(LaunchAndDie(2f));
        }


        if (isOnFire)
        {
            Body.sprite = fire;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (sentient) return;
        direction *= -1;
        transform.localRotation = Quaternion.Euler(0, direction > 0 ? 180 : 0, 0);
    }

    public Person BecomeSentient() {
        if (scaling) return null;
        seeking = false;
        target = null;
        sentient = true;
        Body.sortingLayerName = "Sentient";
        transform.localRotation = Quaternion.Euler(0,180,0);
        Floater.gameObject.SetActive(true);
        Floater.sortingLayerName = "Sentient";
        Floater.sprite = questionMark;
        Body.sprite = confused[Random.Range(0, confused.Length)];
        StartCoroutine(MoveToButton());
        return this;
    }

    IEnumerator MoveToButton() {
        Debug.Log("Moving");
        while (Vector2.Distance(transform.position, GameManager.INSTANCE.chaosButton.transform.position) > .1f) {
            transform.position=Vector2.MoveTowards(transform.position, GameManager.INSTANCE.chaosButton.transform.position, speed * Time.deltaTime);
            yield return null;
        }
        Floater.gameObject.SetActive(false);
    }
}
