using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Person : MonoBehaviour {

    public float speed = 1f;
    public float period = 1f;
    public float magnitude = 1f;
    public float scaleTime = 1f;
    public float launchSpeed = 10f;

	public GameObject interactTooltip;

    House target;
    bool seeking = false;
    float oRot;
    float t = 0;
    int direction = 1;
    Rigidbody2D rigidBody;
    bool sentient = false;
    public bool scaling = false;

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
	public Sprite[] bubbles;
	public GameObject ash;
	public string state = "neutral";
	public string[] chaos_state_options = { "confusion", "house_fire", "house_lightning", "house_explosion",
			"house_missile", "car_fire", "car_lightning", "car_explosion", 
		"car_missile", "person_fire",  "person_explosion", "person_missle" };

	private AudioSource source;   

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody2D>();
        StartCoroutine(ScaleUp(scaleTime));
        oRot = transform.localRotation.eulerAngles.z;
        GameManager.INSTANCE.AddPerson(this);
		source = GameObject.Find ("Audio_Main").GetComponent<AudioSource>();
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
                //print("should be done");
                //ScaleDown(.5f);
                seeking = false;

                Color c = Body.color;
                c.a -= Time.deltaTime;
                Body.color = c;
                if (c.a <= 0)
                {
                    //Spawn ash pile
                 	//   Instantiate(ash, transform.position, transform.localRotation, transform.parent);
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
		source.GetComponent<AudioController> ().playDoorClose ();		//Trigger sound effect

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
		source.GetComponent<AudioController> ().playScream ();
        //seeking = false;
        target = null;
        speed = 2f;
        //StartCoroutine(SusAndDie());
    }

    //returns true while the player alive and the fire should be updated
    private bool updateFire()
    {
        fireTimer += Time.deltaTime;
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
			state = "person_lightning";
        } else if (collision.gameObject.GetComponent<Explosion>() != null){
            seeking = false;
            rigidBody.velocity = (transform.position - collision.transform.position.normalized) * launchSpeed;
			state = "person_explosion";
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
        transform.localScale *= 1.5f;
        transform.localRotation = Quaternion.Euler(0,180,0);
        Floater.gameObject.SetActive(true);
        Floater.sortingLayerName = "Sentient";
        //Floater.sprite = questionMark;
        Body.sprite = confused[Random.Range(0, confused.Length)];
        StartCoroutine(MoveToButton());
        return this;
    }

    IEnumerator MoveToButton() {
        Debug.Log("Moving");
        while (Vector2.Distance(transform.position, GameManager.INSTANCE.chaosTarget.transform.position) > .1f) {
            transform.position=Vector2.MoveTowards(transform.position, GameManager.INSTANCE.chaosTarget.transform.position, 2*speed * Time.deltaTime);
            yield return null;
        }
        Floater.gameObject.SetActive(false);
        GameManager.INSTANCE.ScaleVirusUI();
        Destroy(this.gameObject);
    }

	public void SpawnBubbles(){
        Debug.Log(state);
        //Don't redisplay constantly
        if (Floater.gameObject.activeSelf) {
            return;
        }

		DialogueParser dialogue = GetComponent<DialogueParser>();
        
		dialogue.SpawnBubble();
		Floater.gameObject.SetActive(true);
		Floater.sprite = bubbles[Random.Range(0, bubbles.Length)];

		//Set the text component to some random neutral statement
		TextMesh editor = Floater.GetComponentInChildren<TextMesh> ();
		editor.fontSize = 20;

		int val = (int)Random.Range (0, dialogue.Get_Dialogue_Options (state).Length);

        //editor.text = dialogue.Get_Dialogue_Options(state)[val];

		string tmp_state = state;
		if (GameManager.INSTANCE.Chaos > .5) {
			tmp_state = chaos_state_options[ Random.Range (0, chaos_state_options.Length) ];
		}

        string text = dialogue.Get_Dialogue_Options( tmp_state )[val];
        char[] textCharArr = text.ToCharArray();

        int wrapLen = 12; //wrap the text after X characters
        int counter = 0;
        int lastSpaceIndex = 0;
        for (int i = 0; i < textCharArr.Length; i++)
        {
            if (textCharArr[i] == ' ')
            {
                lastSpaceIndex = i;
            }
            if (counter > wrapLen)
            {
                //text = text.Substring(0, lastSpaceIndex - 1) + "\n" + text.Substring(lastSpaceIndex + 1);
                textCharArr[lastSpaceIndex] = '\n';
                counter = 0;
            }
            counter++;
        }

        editor.text = new string(textCharArr);
    
        //Todo add new lines to text

        //Reach: Be aware of events that have happened nearby (or guess based on chaos), and choose a dialogue option
        //Reach: Add another portion to the json file saying which dialogue has what emotions

        StartCoroutine(WaitAndDespawnBubble());
	}

    IEnumerator WaitAndDespawnBubble() {
        yield return new WaitForSeconds(5f);
        GetComponent<DialogueParser>().DespawnBubble();
    }
}
