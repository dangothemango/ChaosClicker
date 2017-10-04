using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager INSTANCE;

    public GameObject camera;
    public GameObject chaosTarget;
    public Button chaosButton;
    public GameObject thunderCloud;
    public Transform[] missleSpawns;

    Person virus;

    public GameObject virusUI;

    public Person Virus {
        get {
            return virus;
        }
    }

    List<Person> people;
    List<Cloud> clouds;
    List<Car> cars;
    float chaos = 0;
    int superChaos = 0;

    public int SuperChaos {
        get {
            return superChaos;
        }
        set {
            superChaos = value;
            if (superChaos == 1) {
                StartCoroutine(ZoomInAndBackToZero());
            } else if (superChaos == 3) {
                StartCoroutine(ZoomInAndPastZero());
            } else if (superChaos == 5) {
                //TODO start spinning the camera
            }
        }
    }

    public float Chaos {
        get {
            return chaos;
        }
        set {
            chaos = value;
        }
    }

	// Use this for initialization
	void Awake () {
		if (INSTANCE != null) {
            this.enabled = false;
            return;
        }
        INSTANCE = this;
        people = new List<Person>();
        clouds = new List<Cloud>();
        cars = new List<Car>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q)) {
            Chaos = 0.99f;
        }
        if (Random.Range(0, 1.0f) > .994f) {
            SpawnCloud();
        }
    }

    public void AddPerson(Person p) {
        people.Add(p);
    }

    public void RemovePerson(Person p) {
        people.Remove(p);
    }

    public void AddCar(Car c) {
        cars.Add(c);
    }

    public void RemoveCar(Car c) {
        cars.Remove(c);
    }

    public void AddCloud(Cloud c) {
        clouds.Add(c);
    }

    public void RemoveCloud(Cloud c) {
        clouds.Remove(c);
    }

    public Person GetRandomPerson() {
        if (people.Count == 0) {
            return null;
        }
        return people[Random.Range(0, people.Count)];
    }

    void SpawnCloud() {
        float x = Random.Range(-10.0f, 10.0f);
        float y = Mathf.Sqrt(100.0f - (x * x))*(Random.Range(0, 1.0f) > .5f ? -1 : 1);
        Instantiate(thunderCloud, new Vector3(x,y,0), Quaternion.Euler(0, 0, 0), transform);
    }

    public Transform GetRandomMissleSpawn() {
        return missleSpawns[Random.Range(0, missleSpawns.Length)];
    }

    public void IncChaos() {
        //TODO change this to 1f
        if (Chaos < 1f) {
            Chaos += .01f;
        }
        else if (virus == null && virusUI.transform.localScale==Vector3.zero) {
            Debug.Log("Sentience");
            virus = GetRandomPerson().BecomeSentient();
            people.Remove(virus);
        }
        Debug.Log(Chaos);
    }

    public void ScaleVirusUI() {
        StartCoroutine(ScaleVirusUi());
    }

    IEnumerator ScaleVirusUi() {
        float t = 0;
        while (t <= 3f) {
            t += Time.deltaTime;
            virusUI.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t / 3f);
            yield return null;
        }
        StartCoroutine(GeneratePlayerChaos());
    }

    IEnumerator GeneratePlayerChaos() {
        float t = 0;
        while (t < 1f) {
            t += Time.deltaTime;
            virusUI.transform.localPosition = new Vector3(0, Mathf.Sin(t * Mathf.PI)*30,0);
            yield return null;
        }
        chaosButton.onClick.Invoke();
        SuperChaos++;
        yield return new WaitForSeconds(6f);
        StartCoroutine(GeneratePlayerChaos());
    }

    IEnumerator ZoomInAndBackToZero() {
        float t = 0;
        while (SuperChaos != 3) {
            float prev = t;
            t += Time.deltaTime;
            float cur = t;
            camera.transform.position += transform.forward * (Mathf.Abs(Mathf.Sin(cur)) - Mathf.Abs(Mathf.Sin(prev)))*5;
            yield return null;
        }
    }

    IEnumerator ZoomInAndPastZero() {
        float t = 0;
        while (SuperChaos != 5) {
            float prev = t;
            t += Time.deltaTime;
            float cur = t;
            camera.transform.position += transform.forward * (Mathf.Sin(cur) - Mathf.Sin(prev)) * 5;
            yield return null;
        }
    }
}
