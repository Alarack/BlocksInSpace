using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

    public LayerMask mask;

    protected Vector2 surfaceNormal;

    protected virtual void Start() {
        if(transform.parent.GetComponent<Projectile>() != null && transform.parent.gameObject.layer == 8) {
            gameObject.layer = 14;
        } else if(transform.parent.GetComponent<Projectile>() != null && transform.parent.gameObject.layer == 9) {
            gameObject.layer = 15;
        }
        else {
            gameObject.layer = TargetUtils.SetLayer(transform.parent.gameObject.tag, "Shield");
        }
        mask = TargetUtils.SetMask(gameObject.layer, EnumList.MaskProperties.ShieldMask);
    }

    protected virtual void Update() {

        if (GetComponent<Health>().curHealth <= 0) {
            Destroy(gameObject);
        }

        ShieldOrientation();
    }

    void ShieldOrientation() {
        if(transform.parent.gameObject.tag == "Enemy")
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, 180f));
    }

    void OnTriggerEnter2D(Collider2D other) {
        //This relfection only works properly when the shield is straight up or down. The method above is kind of a hacky fix.
        //RaycastHit2D inRayHit = Physics2D.Raycast(other.transform.position, other.transform.up, Mathf.Infinity, mask);
        RaycastHit2D inRayHit = Physics2D.Raycast(transform.position, other.transform.position - transform.position, Mathf.Infinity, mask);

        if (inRayHit.collider != null) {
            surfaceNormal = inRayHit.normal;
        }
        ShieldEffect(other);
    }

    protected virtual bool CheckTag() {
        if (transform.parent.gameObject.tag == "Player" || transform.parent.gameObject.layer == 8) 
            return true;
        else
            return false;
    }

    //protected virtual void SetLayer() {
    //    if (transform.parent.gameObject.tag == "Player")
    //        gameObject.layer = 14;
    //    else
    //        gameObject.layer = 15;
    //}

    protected virtual void ProjectileCollisionEffect(Collider2D other) {
        Projectile shotScript = other.GetComponent<Projectile>();
        GetComponent<Health>().AdjustHealth(shotScript.damage);
        if (shotScript.projectileType != Projectile.ProjectileType.Laser)
            shotScript.CleanUp();
    }

    protected virtual void ShieldEffect(Collider2D other) {
        if (other.gameObject.layer == 9 && CheckTag() || other.gameObject.layer == 8 && !CheckTag()) {
            ProjectileCollisionEffect(other);
            return;
        }

        if (other.gameObject.tag == "Enemy" && CheckTag()) {
            Health otherHealth = other.GetComponent<Health>();

            if (otherHealth != null) {
                GetComponent<Health>().AdjustHealth(Mathf.Round(otherHealth.curHealth / 5));
                otherHealth.KillEntity();
            }
        }
        else if (other.gameObject.tag == "Player" && !CheckTag()) {
            Health otherHealth = other.GetComponent<Health>();

            if (otherHealth != null) {
                otherHealth.AdjustHealth(Mathf.Round(GetComponent<Health>().curHealth / 5));
                GetComponent<Health>().KillEntity();
            }
        }
    }
}