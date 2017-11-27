using UnityEngine;
using System.Collections;

public class HealthUp : MonoBehaviour {

    //TODO: This is temporary. Make this the new basis for powerups at some point.
    public float healAmount;

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player") {
            //other.GetComponent<Health>().AdjustHealth(-healAmount);
            FindObjectOfType<PlayerHealth>().AdjustHealth(-healAmount);
            Destroy(gameObject);
        }

    }
}