using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

    [Header("Basic health Stats")]
    public float maxHealth;
    public float curHealth;
    public float armor;
    [Space(10)]
    public bool isDead = false;
    [Header("Optional Death Particle")]
    public GameObject deathExplosion;

    protected float healthDifference;
    protected Color32 myColor;
    protected SpriteRenderer mySprite;

    protected virtual void Start() {
        mySprite = GetComponent<SpriteRenderer>();
        myColor = mySprite.color;
        FullHealth();
    }

    protected virtual void Update() {
        HealthCheck();
    }

    public virtual void FullHealth() {
        curHealth = maxHealth;
    }

    public virtual void AdjustHealth(float healthAdj, Entity source = null) {
        if (healthAdj > 0f) {
            if (mySprite != null)
                StartCoroutine(DamageFlash(healthAdj));

            if (armor > 0f)
                healthAdj %= armor;

            if (healthAdj < 1f)
                healthAdj = 1f;
        }

        if(healthAdj < 0f) {
            if (mySprite != null)
                StartCoroutine(DamageFlash(healthAdj));
        }


        curHealth -= healthAdj;

        if (curHealth <= 0) {
            curHealth = 0;
        }
        if (curHealth >= maxHealth) {
            curHealth = maxHealth;
        }

        healthDifference = curHealth / maxHealth;
    }

    protected virtual IEnumerator DamageFlash(float color) {

        Color32 flashColor = new Color32();

        if (color > 0f)
            flashColor = new Color32(255, 0, 0, 255);
        else
            flashColor = new Color32(0, 255, 0, 255);

        mySprite.color = flashColor;

        yield return new WaitForSeconds(0.05f);

        mySprite.color = myColor;
    }

    protected virtual void HealthCheck() {
        if (curHealth <= 0) {
            if (!isDead) {
                EntityDying();
            }
            else {
                EntityDead();
            }
        }
    }

    protected virtual void EntityDying() {
        isDead = true;
    }

    protected virtual void EntityDead() {
        if (GetComponent<Entity>() != null) {
            DeathFlair();
        }

        if(GetComponent<Entity>().colorModules.Count > 0) {
            foreach (ColorModule c in GetComponent<Entity>().colorModules) {
                c.GetComponent<Health>().DeathFlair();
            }
        }

        //GameObject deathExplosion = Instantiate(EnumList.ExplosionColor(GetComponent<Entity>().unitColor), transform.position, transform.rotation) as GameObject;

        Destroy(gameObject);
    }

    protected virtual void DeathFlair() {
        CameraController.ShakeCam(0.1f, 0.1f);
        deathExplosion = EnumList.ExplosionColor(GetComponent<Entity>().unitColor);
        GameObject activeBoom = Instantiate(deathExplosion, transform.position, transform.rotation) as GameObject;

        int rand = Random.Range(0, 100);
        if(rand > 50) {
            //activeBoom.transform.localScale *= 3;
            EnumList.ScaleParticleEffect(activeBoom, 2.5f);
        }

        if(gameObject.tag == "Enemy" && rand > 75) {
            int numFrags = Random.Range(1, 4);
            for (int i = 0; i < numFrags; i++) {
                GameObject activeFragment = Instantiate(EnumList.FragmentColor(GetComponent<Entity>().unitColor), transform.position, Quaternion.identity) as GameObject;
                activeFragment.GetComponent<Fragment>().color = GetComponent<Entity>().GetColor();
            }

        }

        Destroy(activeBoom, 1f);
    }

    public virtual void KillEntity() {
        curHealth = 0;
    }
}