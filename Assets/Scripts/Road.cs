using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour {

    public GameObject car;
    public GameObject person;

    [Header("Car Spawns")]
    public GameObject leftSpawn;
    public GameObject rightSpawn;


    House[] houses;

	// Use this for initialization
	void Start () {
        houses = GetComponentsInChildren<House>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Random.Range(0, 1.0f) > .9964f) {
            Instantiate(car, (Random.Range(0, 1.0f) > .5f ? leftSpawn : rightSpawn).transform.position,Quaternion.Euler(0,0,0),transform);
        }
        if (Random.Range(0, 1.0f) > .99164f) {
            Person p = Instantiate(person, houses[Random.Range(0, houses.Length)].transform.position, Quaternion.Euler(0, 0, 0), transform).GetComponent<Person>();
            GameManager.INSTANCE.AddPerson(p);
            while (!p.SetTarget(houses[Random.Range(0, houses.Length)]));
        }
	}
}
