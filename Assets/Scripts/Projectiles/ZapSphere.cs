using UnityEngine;
using System.Collections;

public class ZapSphere : LingeringProjectile {

    public float zapDamage;
    public LayerMask mask;

    private float maxZapDistance;

    protected override void Start()
    {
        base.Start();

        maxZapDistance = GetComponent<CircleCollider2D>().radius;
    }

    protected override void Update() {
        base.Update();

    }

    protected override void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            //damageTimer -= Time.deltaTime;
            damageTimer = 0f;
            if (damageTimer <= 0)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, (other.transform.position - transform.position), maxZapDistance, mask);


                if (hit.collider != null && hit.collider.GetComponent<Health>() != null)
                {


                    Debug.DrawLine(transform.position, hit.collider.transform.position, Color.red);

                    Health otherHealth = hit.transform.GetComponent<Health>();

                    if (otherHealth.curHealth <= zapDamage)
                    {
                        DeathBlow(other, otherHealth, zapDamage);
                    }
                    else
                    {
                        otherHealth.AdjustHealth(zapDamage);
                    }
                }

                damageTimer = damageInterval;
            }
        }
    }
}