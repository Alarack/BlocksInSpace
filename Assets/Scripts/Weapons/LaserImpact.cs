using UnityEngine;
using System.Collections;

public class LaserImpact : Projectile {

    public float damageInterval;

    private bool isTouching;
    private float intervalTimer;
    private GameObject target;

    protected override void Start() {
        SetLayer();
        mySprite = GetComponent<SpriteRenderer>();

        if (particleTrail) {
            if (particleTrailLocation == null)
                particleTrailLocation = transform;

            GameObject activeTrail = Instantiate(EnumList.ParticleTrailColor(parentColor.ToString(), projectileType.ToString()), particleTrailLocation.position, Quaternion.identity) as GameObject;
            activeTrail.transform.SetParent(transform, false);
            activeTrail.transform.rotation = particleTrailLocation.rotation;
        }

        if (autoSpriteColor) {
            ChangeColor(parentColor);
        }

    }

    //void OnTriggerStay2D(Collider2D other)
    //{
    //    Debug.Log(other.gameObject.name + " is in my Zone");
    //    if (other.tag == "Player" || other.tag == "Enemy")
    //    {
    //        other.GetComponent<Rigidbody2D>().WakeUp();
    //        if (other.GetComponent<Health>() != null)
    //        {
    //            if (!isTouching)
    //            {
    //                isTouching = true;

    //                Debug.Log("Starting Coroutine");
    //                StartCoroutine(DoT(other.GetComponent<Health>(), damage));
    //            }
    //        }
    //    }
    //}

    protected override void Update() {
        if (isTouching) {
            intervalTimer += Time.deltaTime;
            if (intervalTimer >= damageInterval && target != null) {
                TickDamage(target.GetComponent<Health>(), damage);
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player" || other.tag == "Enemy" || other.tag == "Reflector") {
            target = other.gameObject;
            isTouching = true;
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "Player" || other.tag == "Enemy" || other.tag == "Reflector") {
            target = other.gameObject;
            isTouching = true;
        }
    }


    void TickDamage(Health target, float dmg) {
        if (target.curHealth <= damage) {
            DeathBlow(target.GetComponent<Collider2D>(), target, dmg);
        }
        else {
            target.AdjustHealth(dmg);
        }
        intervalTimer = 0f;
    }

    void OnDisable() {
        target = null;
    }
}