using UnityEngine;
using System.Collections;

public class GlobalDOT : MonoBehaviour {

    public float damage;
    public float damageInterval;

    private float intervalTimer;
    private GameObject[] enemies;
    private Projectile myProjectile;

    void Start() {
        myProjectile = GetComponent<Projectile>();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void Update() {
        intervalTimer += Time.deltaTime;
        if (intervalTimer >= damageInterval) {
            foreach (GameObject enemy in enemies) {
                if (enemy != null && enemy.GetComponent<Collider2D>().enabled) {
                    Health enemyhealth = enemy.GetComponent<Health>();

                    TickDamage(enemyhealth, damage);
                }
            }
        }
    }

    void TickDamage(Health target, float dmg) {
        if (target.curHealth <= damage) {
            myProjectile.DeathBlow(target.GetComponent<Collider2D>(), target, dmg);
        }
        else {
            target.AdjustHealth(dmg);
        }
        intervalTimer = 0f;
    }
}