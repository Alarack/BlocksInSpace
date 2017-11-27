using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutoSetPosition : MonoBehaviour {

    //public Transform myPosition;

    public string nodeName = "LootSpawn";
    public string multiNodeName = "FixedLocation";
    [Space(10)]
    public bool multiLoc;
    public bool setRotation;

    private GameObject[] allSpotsWithName;
    private List<Vector3> potentialLocations = new List<Vector3>();

    void Awake() {

        if (multiLoc) {
            allSpotsWithName = GameObject.FindGameObjectsWithTag(multiNodeName);

            foreach (GameObject g in allSpotsWithName) {
                potentialLocations.Add(g.transform.position);
            }

            int randLoc = Random.Range(0, potentialLocations.Count);

            transform.position = potentialLocations[randLoc];
        }
        else {
            transform.position = GameObject.FindGameObjectWithTag(nodeName).transform.position;
            if (setRotation) {
                transform.rotation = GameObject.FindGameObjectWithTag(nodeName).transform.rotation;
            }

        }
    }
}