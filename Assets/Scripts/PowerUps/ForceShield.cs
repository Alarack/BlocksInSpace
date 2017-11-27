using UnityEngine;
using System.Collections;

public class ForceShield : PowerUp {

    [Header("Shield PowerUp Info")]
    public GameObject shield;
    public float shieldMaxHealth;

    private bool isDeployed;

    protected override void PowerUpEffect() {
        GameManager.SetUIText("Shields Up!");

        GameObject activeShield = Instantiate(shield, transform.position, transform.rotation) as GameObject;
        activeShield.GetComponent<Health>().maxHealth = shieldMaxHealth;
        activeShield.GetComponent<Health>().curHealth = shieldMaxHealth;
        activeShield.transform.parent = transform.parent;
        isDeployed = true;
    }

    void LateUpdate() {
        if (transform.childCount == 0 && isDeployed) {
            Destroy(gameObject);
        }
    }
}