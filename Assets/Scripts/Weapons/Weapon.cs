using UnityEngine;
using System.Collections;


public delegate void OnShotFired(GameObject shot);

public class Weapon : MonoBehaviour {

    public OnKill onKill;
    public OnShotFired onShotFired;

    public enum WeaponType {
        Blaster,
        Laser,
        Shotgun,
        Turret,
        Rocket,
        Misc
    }

    [Header("Basic Stats")]
    public WeaponType weaponType;
    public EnumList.ExplosionType explosionType;
    public float damage;
    public float shotScale = 1f;
    public float muzzleFlashScale = 1f;
    public bool addNewParticleTrail = false;
    public Projectile.ProjectileTrailType newProjectileTrailType;
    public float weaponParticleTrailScaleFactor = 1f;
    public float coolDown;
    public float shotSpeed;
    public float error;
    [Header("Projectile/Drone Info")]
    public GameObject weaponSpawn;
    public Transform shotOrigin;
    [Header("Burst Configuration")]
    public bool burstMode;
    public int numBurstShots;
    public float burstDelay;
    [Header("Misc Options")]
    public bool muzzleFlash = false;
    public bool compensateMuzzleFlashOffset = true;
    public bool useAimHelpers = false;
    public bool forcePlayerShot = false;
    public bool autoScaleShots = false;
    public bool autoFire = false;
    [Header("Recoil Options")]
    public float recoilSpeed = 2f;
    [Header("Sprite Stuff")]
    public bool autoSpriteColor = false;
    public Sprite[] sprites;
    [Header("Sounds")]
    public string fireSoundName;
    public float soundVolume = 0.025f;

    protected EnumList.Colors color;
    protected SpriteRenderer mySprite;
    protected Transform myShotOrigin;
    protected float lastFire;
    protected Entity parentEntity;
    protected float recoilTimer;
    protected Animator myAnim;

    protected virtual void Awake() {
        //if (transform.parent != null) {
        //    if (transform.parent.gameObject.tag == "Player")
        //        transform.gameObject.tag = transform.parent.gameObject.tag;
        //}

        if (shotOrigin == null) {
            shotOrigin = transform.FindChild("ShotOrigin");
        }

        myShotOrigin = shotOrigin;
    }

    protected virtual void Start() {
        parentEntity = GetComponentInParent<Entity>();
        mySprite = GetComponent<SpriteRenderer>();
        myAnim = GetComponent<Animator>();

        if (autoScaleShots && parentEntity != null) {
            shotScale *= parentEntity.weaponShotScaleMod;
            muzzleFlashScale *= parentEntity.weaponFlashScaleFactor;
            weaponParticleTrailScaleFactor *= parentEntity.weaponParticleTrailScaleFactor;
            coolDown *= parentEntity.attackSpeedModifier;
        }

        if (parentEntity != null) {
            color = parentEntity.GetColor();
        }
            
        if (autoSpriteColor && parentEntity != null)
            EnumList.ColorInit(parentEntity.GetColor(), mySprite, sprites);
    }

    protected virtual void Update() {
        if (autoFire && CanFire() && myShotOrigin != null)
            Fire();
    }

    public virtual bool CanFire() {
        return Time.time > lastFire;
    }

    public virtual void Fire() {
        if (burstMode) {
            if(burstDelay > 0) {
                StartCoroutine(BurstFire());
                lastFire = Time.time + coolDown;
            }
            else {
                MultiFire();
                lastFire = Time.time + coolDown;
            }
        }
        else {
            CreateProjectile(null, error);
            lastFire = Time.time + coolDown;
        }

        if(recoilSpeed > 0f) {
            WeaponRecoil();
        }

    }

    protected virtual void MultiFire() {
        for (int i = 0; i < numBurstShots; i++) {
            CreateProjectile(null, error);
        }
    }

    protected virtual IEnumerator BurstFire() {
        for (int i = 0; i < numBurstShots; i++) {
            CreateProjectile(null, error);

            if (recoilSpeed > 0f) {
                WeaponRecoil();
            }

            yield return new WaitForSeconds(burstDelay);
        }
    }

    protected virtual void CreateProjectile(Transform aimHelper = null, float error = 0f) {
        Transform tmpOrigin = myShotOrigin;


        if(fireSoundName != "") {
            SoundManager.PlaySound(fireSoundName, soundVolume);
        }

        //if(origin == null) {
        //    tmpOrigin = myShotOrigin;
        //}
        //else {
        //    tmpOrigin = origin;
        //}
        
        GameObject activeShot = Instantiate(weaponSpawn, myShotOrigin.transform.position, myShotOrigin.transform.rotation) as GameObject;
        activeShot.transform.localScale *= shotScale;

        if (muzzleFlash) {
            GameObject muzzleFlashParticle = Instantiate(EnumList.LoadExplosionByType(color.ToString(), explosionType.ToString()), tmpOrigin.position, transform.rotation) as GameObject;

            if(parentEntity != null)
                EnumList.ScaleParticleEffect(muzzleFlashParticle, (parentEntity.weaponFlashScaleFactor * muzzleFlashScale));
            else
                EnumList.ScaleParticleEffect(muzzleFlashParticle, (muzzleFlashScale));

            muzzleFlashParticle.transform.parent = myShotOrigin;
            if(compensateMuzzleFlashOffset)
                muzzleFlashParticle.transform.localPosition = new Vector3(0f, 0.4f, 0f);

            Destroy(muzzleFlashParticle, 1f);
        }

        Projectile shotScript = activeShot.GetComponent<Projectile>();
        shotScript.parentWeapon = this;

        if (shotSpeed != 0f && shotScript.GetComponent<EntityMovement>() != null)
            shotScript.GetComponent<EntityMovement>().speed = shotSpeed;

        if (parentEntity != null) {
            shotScript.parentColor = parentEntity.GetColor();
            shotScript.damage = damage * parentEntity.damageModifier;
        }
        else {
            shotScript.damage = damage;
        }

        if (aimHelper != null && useAimHelpers)
            activeShot.transform.LookAt(transform.position + new Vector3(0, 0, 1), aimHelper.position - transform.position);

        if (error != 0f) {
            float e = Random.Range(-error, error);
            activeShot.transform.rotation = tmpOrigin.rotation * Quaternion.Euler(tmpOrigin.rotation.x, tmpOrigin.rotation.y, e);
        }
        //Delegate Stuff
        if (onKill != null)
            shotScript.onKill += onKill;

        if (onShotFired != null)
            onShotFired(activeShot);

        //Add new particle trail
        if (addNewParticleTrail)
            AddNewProjectileTrail(shotScript);

        DetectPlayerShot(activeShot);
    }

    public void AddNewProjectileTrail(Projectile shot) {

        if (!shot.particleTrail)
            shot.particleTrail = true;

        shot.particleTrailType = newProjectileTrailType;

        //shot.SetupParticleTrail();
    }


    protected virtual void DetectPlayerShot(GameObject shot) {
        if (gameObject.tag == "Player" || forcePlayerShot || gameObject.layer == 10) {
            shot.GetComponent<Projectile>().playerShot = true;
        }
        else if (gameObject.tag != "Player") {
            shot.GetComponent<Projectile>().playerShot = false;
        }
    }

    public virtual void WeaponRecoil() {
        if (myAnim != null) {
            myAnim.speed = recoilSpeed;
            myAnim.SetTrigger("Recoil");
        }
    }
}