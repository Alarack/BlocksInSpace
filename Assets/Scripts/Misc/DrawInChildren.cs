using UnityEngine;
using System.Collections;

public class DrawInChildren : MonoBehaviour {

    public float speed;
    public GameObject detonationProjectile;
    public int numFragments;

    private Projectile parent;

    void Start() {
        parent = GetComponentInParent<Projectile>();
    }

    void Update() {
        foreach (Transform child in transform) {
            if (child.transform.position != transform.position)
                child.transform.position = Vector2.MoveTowards(child.transform.position, transform.position, speed * Time.deltaTime);
            else {
                Detonate(numFragments);
                Destroy(child.gameObject);
            }
        }
    }

    void Detonate(int numShots) {
        for (int i = 0; i < numShots; i++) {
            GameObject activeShot = Instantiate(detonationProjectile, transform.position, transform.rotation) as GameObject;
            Projectile shotScript = activeShot.GetComponent<Projectile>();
            shotScript.parentColor = parent.parentColor;
            shotScript.playerShot = parent.playerShot;
            Vector2 randomDir = Random.insideUnitCircle;

            activeShot.transform.rotation = Quaternion.FromToRotation(activeShot.transform.up, randomDir);
        }
    }
}
