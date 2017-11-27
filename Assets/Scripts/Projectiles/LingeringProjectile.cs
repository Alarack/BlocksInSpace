using UnityEngine;
using System.Collections;

public class LingeringProjectile : Projectile {

    [Header("Damage Over Time")]
    public float damageInterval;
    [Header("Misc Options")]
    public bool jiggle = true;
    public bool destroyEnemyProjectiles;

    protected float damageTimer = 0f;

    protected override void Start() {
        base.Start();
        piercing = true;

        if (transform.parent != null)
            if (transform.parent.gameObject.tag == "Player")
                playerShot = true;
                //playerShot = transform.parent.GetComponent<Projectile>().playerShot;

        SetLayer();
    }

    protected override void Update() {
        base.Update();
        if(jiggle)
            Jiggle();
    }

    public override void CleanUp() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject, 1.5f);
        }
        transform.DetachChildren();

        Destroy(gameObject);
    }

    protected virtual void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "Player" || other.tag == "Enemy") {
            damageTimer -= Time.deltaTime;

            if (damageTimer <= 0) {
                Health otherHealth = other.GetComponent<Health>();

                if (otherHealth != null) {
                    if (otherHealth.curHealth <= damage) {
                        DeathBlow(other, otherHealth, damage);
                    }
                    else {
                        otherHealth.AdjustHealth(damage);
                    }
                }
                damageTimer = damageInterval;
            }
        }

        if(destroyEnemyProjectiles && (other.gameObject.layer == 8 || other.gameObject.layer == 9) ){
            other.GetComponent<Projectile>().CleanUp();
        }

    }

    void Jiggle() {
        Vector2 dir = Random.insideUnitCircle * 0.01f;
        Vector3 dir3 = new Vector3(dir.x, dir.y, 0f);

        transform.position += dir3;
    }
}