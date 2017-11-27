using UnityEngine;
using System.Collections;

public class EventHorizon : MonoBehaviour {
    public int damage;
    public float damageInterval;

    private float damageTimer;

    void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Enemy") {
            damageTimer -= Time.deltaTime;

            if (damageTimer <= 0) {
                other.GetComponent<Health>().AdjustHealth(damage);

                damageTimer = damageInterval;

                if (other.transform.localScale.x > 0.1f) {
                    other.transform.localScale = new Vector3(other.transform.localScale.x - 0.1f, other.transform.localScale.y - 0.1f, other.transform.localScale.z - 0.1f);
                }
            }
        }

        if (other.gameObject.layer == 9) {
            Destroy(other.gameObject);
        }
    }
}