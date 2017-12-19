using UnityEngine;
using System.Collections;

public delegate void OnKill(GameObject target);

public class Projectile : MonoBehaviour {

    private const int INFINITE_PIERCE = -1;

    public OnKill onKill;
    [Header("Sounds")]
    public string impactSound;
    public float soundVolume = 0.025f;

    public enum ProjectileType {
        Blaster,
        Rocket,
        Rail,
        Laser
    }

    public enum ProjectileTrailType {
        Rocket,
        MedThrust,
        ThinThrst,
        Backfire,
        RocketFlash,
        Power,
        Snowy,
        Rail
    }

    [Header("Basic Stats")]
    public ProjectileType projectileType;
    public float life;
    public float damage;
    [Space(10)]
    public GameObject explosionParticle;
    public EnumList.ExplosionType explosionType;

    [Header("Particle Trails")]
    public bool particleTrail;
    public ProjectileTrailType particleTrailType;
    public float particleTrailScaleMod = 1f;
    public Transform particleTrailLocation;
    [Header("Piercing Stats")]
    public bool piercing = false;
    public int maxPierce = INFINITE_PIERCE;
    [Header("Child Projectiles")]
    public bool spawnChildProjectile;
    public bool createParticleOnSapwn = true;
    public bool inheritParticleTrail;
    public float childProjectileShotScale = 1f;
    public GameObject childProjectile;
    [Header("Sprite Stuff")]
    public bool autoSpriteColor;
    public bool autoExplosionColor = true;
    public Sprite[] sprites;

    [HideInInspector]
    public bool playerShot;
    //[HideInInspector]
    public EnumList.Colors parentColor;
    [HideInInspector]
    public Weapon parentWeapon;

    protected int curPierce = 0;
    protected Rigidbody2D myBody;
    protected SpriteRenderer mySprite;

    protected virtual void Start() {
        if(life > 0f)
            Invoke("CleanUp", life);

        myBody = GetComponent<Rigidbody2D>();
        mySprite = GetComponent<SpriteRenderer>();

        if (particleTrail) {
            SetupParticleTrail();
            //if (particleTrailLocation == null)
            //    particleTrailLocation = transform;

            //GameObject activeTrail = Instantiate(EnumList.ParticleTrailColor(parentColor.ToString(), particleTrailType.ToString()), particleTrailLocation.position, particleTrailLocation.rotation) as GameObject;
            //EnumList.ScaleParticleEffect(activeTrail, particleTrailScaleMod * parentWeapon.weaponParticleTrailScaleFactor);

            //activeTrail.transform.parent = transform;
        }

        if (mySprite == null)
            mySprite = GetComponentInChildren<SpriteRenderer>();

        SetLayer();

        if (autoSpriteColor) {
            ChangeColor(parentColor);
        }

        //float speedVar = Random.Range(0.3f, 1f);
        //if (!playerShot && GetComponent<EntityMovement>() != null)
        //    GetComponent<EntityMovement>().speed *= speedVar;
    }

    public virtual void SetupParticleTrail() {
        if (particleTrailLocation == null)
            particleTrailLocation = transform;

        GameObject activeTrail = Instantiate(EnumList.ParticleTrailColor(parentColor.ToString(), particleTrailType.ToString()), particleTrailLocation.position, particleTrailLocation.rotation) as GameObject;
        EnumList.ScaleParticleEffect(activeTrail, particleTrailScaleMod * parentWeapon.weaponParticleTrailScaleFactor);

        activeTrail.transform.parent = transform;
    }



    protected virtual void SetLayer() {
        if (playerShot)
            gameObject.layer = 8;
        else
            gameObject.layer = 9;
    }

    protected virtual void FixedUpdate() {

    }

    protected virtual void Update() {

    }

    public virtual void CleanUp() {
        if (GetComponentInChildren<ParticleSystem>() != null) {
            foreach (Transform child in transform) {
                Destroy(child.gameObject, 1.5f);
            }
            transform.DetachChildren();
        }



        //if (EnumList.ExplosionColor(parentColor) != null && autoExplosionColor) {
        //    GameObject explosion = Instantiate(EnumList.ExplosionColor(parentColor), transform.position, transform.rotation) as GameObject;
        //    Destroy(explosion, 1f);
        //}
        //else if (explosionParticle != null) {
        //    GameObject explosion = Instantiate(explosionParticle, transform.position, transform.rotation) as GameObject;
        //    Destroy(explosion, 1f);
        //}

        if (EnumList.ExplosionColor(parentColor) != null && autoExplosionColor) {
            float size = 1f;

            if (gameObject.name == "TinyBeam3" + "(Clone)") {
                size = 0.5f;
            }
               
            GameObject explosion = Instantiate(EnumList.LoadExplosionByType(parentColor.ToString(), explosionType.ToString()), transform.position, transform.rotation) as GameObject;
            EnumList.ScaleParticleEffect(explosion, size);

            Destroy(explosion, 1f);
        }

        if (spawnChildProjectile)
            SpawnChildProjectile();

        Destroy(gameObject);
    }

    protected GameObject SpawnChildProjectile(Transform aimHelper = null, bool attachToParent = false) {
        if (createParticleOnSapwn) {
            GameObject explosion = Instantiate(EnumList.LoadExplosionByType(parentColor.ToString(), explosionType.ToString()), transform.position, transform.rotation) as GameObject;
            Destroy(explosion, 0.55f);
        }

        GameObject activeSubProjectile = Instantiate(childProjectile, transform.position, transform.rotation) as GameObject;
        Projectile subProjectileScript = activeSubProjectile.GetComponent<Projectile>();

        subProjectileScript.parentColor = parentColor;
        subProjectileScript.parentWeapon = this.parentWeapon;
        subProjectileScript.transform.localScale *= childProjectileShotScale;
        subProjectileScript.projectileType = projectileType;
        subProjectileScript.onKill = onKill;
        subProjectileScript.playerShot = playerShot;

        if(parentWeapon != null)
            subProjectileScript.damage *= parentWeapon.GetComponentInParent<Entity>().damageModifier;

        activeSubProjectile.gameObject.layer = gameObject.layer;
        activeSubProjectile.gameObject.tag = gameObject.tag;
        //Random Rotation
        Vector3 euler = activeSubProjectile.transform.eulerAngles;
        euler.z = Random.Range(0f, 360f);
        activeSubProjectile.transform.eulerAngles = euler;

        if (aimHelper != null)
            activeSubProjectile.transform.LookAt(transform.position + new Vector3(0, 0, 1), aimHelper.position - transform.position);

        if (inheritParticleTrail) {
            subProjectileScript.particleTrail = true;
            subProjectileScript.particleTrailType = particleTrailType;
        }

        if (attachToParent) {
            activeSubProjectile.GetComponent<Rigidbody2D>().isKinematic = true;
            //Destroy(activeSubProjectile.GetComponent<Rigidbody2D>());
            activeSubProjectile.transform.SetParent(transform, true);
            activeSubProjectile.name = "SubShot";
        }

        return activeSubProjectile;

    }

    protected virtual void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player" || other.tag == "Enemy" || other.tag == "Drone") {


            if (impactSound != "") {
                SoundManager.PlaySound(impactSound, soundVolume);
            }

            Health otherHealth = other.GetComponent<Health>();

            if (otherHealth != null) {
                if (otherHealth.curHealth <= damage) {
                    DeathBlow(other, otherHealth, damage);
                }
                else {
                    otherHealth.AdjustHealth(damage);
                }
            }
            //else {
            //    //other.GetComponentInParent<Health>().AdjustHealth(damage); //TODO: Not sure I need this
            //}

            if (piercing) {
                curPierce++;

                if (curPierce == maxPierce) {
                    CleanUp();
                }
            }
            else {
                CleanUp();
            }
        }
    }

    public virtual void DeathBlow(Collider2D other, Health targetHealth, float damage) {
        if (onKill != null)
            onKill(other.gameObject);

        targetHealth.AdjustHealth(damage);
    }

    public virtual void ChangeColor(EnumList.Colors color) {
        parentColor = color;
        if (mySprite != null && sprites.Length != 0)
            EnumList.ColorInit(color, mySprite, sprites);
    }

    //RaycastHit2D TestRay()
    //{
    //    RaycastHit2D hit = new RaycastHit2D();
    //    float dist = 0.8f;

    //    if (EnumList.CheckLayer(gameObject) == 9)
    //    {
    //        reflectorMask = 1 << 14;
    //        hit = Physics2D.Raycast(transform.position, myBody.velocity, dist, reflectorMask);
    //        if (hit.collider != null)
    //            Debug.Log("I hit:" + " " + hit.collider.gameObject.tag);
    //    } 

    //    if (EnumList.CheckLayer(gameObject) == 8)
    //    {
    //        reflectorMask = 1 << 15;
    //        hit = Physics2D.Raycast(transform.position, myBody.velocity, dist, reflectorMask);
    //        if (hit.collider != null)
    //            Debug.Log("I hit:" + " " + hit.collider.gameObject.tag);
    //    }

    //    return hit;
    //}

    //void Reflect(RaycastHit2D hit)
    //{

    //    Vector2 surfaceNormal = hit.normal;

    //    Vector2 reflectedVelocity = Vector2.Reflect(-myBody.velocity, surfaceNormal);

    //    Quaternion rotation = Quaternion.FromToRotation(myBody.velocity, reflectedVelocity);

    //    if(hit.collider != null)
    //        transform.rotation = rotation * hit.collider.transform.rotation;
    //}
}