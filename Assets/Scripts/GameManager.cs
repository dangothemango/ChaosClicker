using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager INSTANCE;

    public GameObject thunderCloud;

    List<Person> people;
    List<Cloud> clouds;
    List<Car> cars;
    float chaos = 0;

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

    public void IncChaos() {
        chaos += .01f;
    }
}
