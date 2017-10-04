using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustSortingLayer : MonoBehaviour {

    Renderer r;

    // Use this for initialization
    void Start() {
        r = GetComponent<Renderer>();
        r.sortingLayerName = "Sentient";
    }

    // Update is called once per frame
    void Update() {

    }
}
