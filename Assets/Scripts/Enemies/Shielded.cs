using UnityEngine;
using System.Collections;

public class Shielded : MonoBehaviour {

    public GameObject shield;
    public float shieldScaleMod = 1f;
    public float shieldAmount;
    // private GameObject myShield;

    void Start() {
        GameObject activeShield = Instantiate(shield, transform.position, transform.rotation) as GameObject;
        activeShield.transform.parent = transform;
        activeShield.transform.localScale *= shieldScaleMod;
        activeShield.GetComponent<Health>().maxHealth = shieldAmount;
        activeShield.GetComponent<Health>().curHealth = shieldAmount;
        //myShield = activeShield;
    }

    void Update() {
        //myShield.transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, 180f));
    }
}