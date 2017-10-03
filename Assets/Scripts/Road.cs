using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour {

    public GameObject car;
    public GameObject person;
    public GameObject missle;

    [Header("Car Spawns")]
    public GameObject leftSpawn;
    public GameObject rightSpawn;


    List<House> houses;

	// Use this for initialization
	void Start () {
        houses = new List<House>(GetComponentsInChildren<House>());
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.M) || Random.Range(0, 1.0f) <= (GameManager.INSTANCE.Chaos-.2f) / 19) {
            Missle m = Instantiate(missle, GameManager.INSTANCE.GetRandomMissleSpawn().position, Quaternion.Euler(0, 0, 0), transform).GetComponent<Missle>();
            if (houses.Count > 0) {
                m.SetTarget(houses[Random.Range(0, houses.Count)]);
            } else {
                m.SetTarget(null);
            }
        }

        if (Random.Range(0, 1.0f) > .9964f-Mathf.Sin(Mathf.Max(GameManager.INSTANCE.Chaos*Mathf.PI*3,Mathf.PI))) {
            Instantiate(car, (Random.Range(0, 1.0f) > .5f ? leftSpawn : rightSpawn).transform.position,Quaternion.Euler(0,0,0),transform);
        }
        if (Random.Range(0, 1.0f) > .9964f-GameManager.INSTANCE.Chaos/10) {
            if (houses.Count != 0) {
                Person p = Instantiate(person, houses[Random.Range(0, houses.Count)].transform.position, Quaternion.Euler(0, 0, 0), transform).GetComponent<Person>();
                while (houses.Count > 1 && !p.SetTarget(houses[Random.Range(0, houses.Count)])) ;
                if (houses.Count <= 1) {
                    p.SetTarget(null);
                }
            } else {
                Person p = Instantiate(person,(Random.Range(0, 1.0f) > .5f ? leftSpawn : rightSpawn).transform.position, Quaternion.Euler(0, 0, 0), transform).GetComponent<Person>();
                p.SetTarget(null);
            }
        }
	}

    public void RemoveHouse(House h) {
        houses.Remove(h);
        Debug.Log(houses.Count);
    }
}
