using UnityEngine;
using System.Collections;

public class ShieldGenerator : MonoBehaviour {

    public GameObject shield;
    public float shieldMaxHealth;
    public float coolDown;

    private float lastShield;
    private bool isShielded;
    private GameObject activeShield;

    void Awake() {
        CreateShield();
    }

    void Update() {

        if (activeShield == null && isShielded) {
            lastShield = ShieldsDownTime();
            isShielded = false;
        }

        if (CanShield()) {
            CreateShield();
        }
    }

    public virtual bool CanShield() {
        if (Time.time > lastShield && !isShielded)
            return true;
        else
            return false;
    }

    void CreateShield() {
        GameObject tmpShield = Instantiate(shield, transform.position, transform.rotation) as GameObject;
        tmpShield.GetComponent<Health>().maxHealth = shieldMaxHealth;
        tmpShield.GetComponent<Health>().curHealth = shieldMaxHealth;
        tmpShield.transform.parent = transform;
        tmpShield.transform.localPosition = Vector2.zero;
        isShielded = true;

        activeShield = tmpShield;
    }

    float ShieldsDownTime() {
        return Time.time + coolDown;
    }
}