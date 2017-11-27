using UnityEngine;
using System.Collections;

public class ReflectorShield : Shield {
    private EnumList.Colors color;

    protected override void Start() {
        base.Start();

        if (GetComponentInParent<Entity>() != null) {
            color = GetComponentInParent<Entity>().GetColor();
        }
    }

    protected override void ProjectileCollisionEffect(Collider2D other) {
        if (other.GetComponent<Projectile>().projectileType == Projectile.ProjectileType.Laser || other.GetComponent<AutoSeek>() != null) {
            base.ProjectileCollisionEffect(other);
        }
        else {
            Projectile shotScript = other.GetComponent<Projectile>();
            Rigidbody2D shotBody = other.GetComponent<Rigidbody2D>();

            if (shotScript.piercing) {
                shotScript.piercing = false;
            }
            //if(other.GetComponent<LookAtTarget>() != null) {
            //    Destroy(other.GetComponent<LookAtTarget>());
            //}



            if (shotBody != null) {
                Vector2 reflectedVelocity = Vector2.Reflect(-shotBody.velocity, surfaceNormal);

                Quaternion rotation = Quaternion.FromToRotation(shotBody.velocity, reflectedVelocity);
                other.transform.rotation = rotation * transform.rotation;

                shotScript.ChangeColor(color);
            }

            if (other.gameObject.layer == 8 && shotScript.playerShot) {
                other.gameObject.layer = 9;
                shotScript.playerShot = false;
            }

            else if (other.gameObject.layer == 9 && !shotScript.playerShot) {
                other.gameObject.layer = 8;
                shotScript.playerShot = true;
            }

            GetComponent<Health>().AdjustHealth(shotScript.damage);
        }
    }
}