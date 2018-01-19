using UnityEngine;
using System.Collections;

public class Laser : Weapon {

    [Header("Laser Damage Cooldown")]
    public float damageInterval;
    [Header("Laser pieces")]
    public GameObject laserStart;
    public GameObject laserMiddle;
    public GameObject laserEnd;
    public LayerMask mask;

    [HideInInspector]
    public Vector3 laserDir;
    protected GameObject start;
    protected GameObject middle;
    protected GameObject end;
    protected float capSpriteHeight;
    protected LaserImpact laserPoint;
    protected LookAtTarget lookScript;
    protected TargetingWeapon reticleTargetScript;
    protected TargetingWeapon parentReticleTargetScript;

    protected override void Start() {
        base.Start();
        //Get a bunch of scripts for targeting checks
        lookScript = GetComponent<LookAtTarget>();
        reticleTargetScript = GetComponent<TargetingWeapon>();
        parentReticleTargetScript = parentEntity.GetComponent<TargetingWeapon>();
        mask = TargetUtils.SetMask(gameObject.layer, EnumList.MaskProperties.LaserMask);
        CreateLaser();
    }

    void CreateLaser() {
        //End Piece
        if (parentEntity != null) {
            end = Instantiate(laserEnd) as GameObject;
            end.GetComponent<Projectile>().parentColor = parentEntity.GetColor();
            end.transform.SetParent(transform, true);
            end.transform.localPosition = Vector2.zero;
            end.transform.rotation = transform.rotation;
            capSpriteHeight = laserEnd.GetComponent<SpriteRenderer>().bounds.size.y * (1f / Mathf.Abs(transform.parent.localScale.y));
            laserPoint = end.GetComponent<LaserImpact>();
            //LingeringProjectile laserPoint = end.GetComponent<LingeringProjectile>();
            DetectPlayerShot(end);
            end.SetActive(false);
        }
        //Mid Piece
        middle = Instantiate(laserMiddle, transform.position, transform.rotation) as GameObject;
        middle.transform.parent = transform;
        middle.transform.localPosition = Vector2.zero;
        middle.SetActive(false);
        //Start Piece
        start = Instantiate(laserStart, transform.position, transform.rotation) as GameObject;
        start.transform.parent = transform;
        start.transform.localPosition = Vector2.zero;
        start.SetActive(false);
    }

    protected override void Update() {
        base.Update();

        if (Input.GetButtonUp("Jump")) {
            CleanUpLaser();
        }

        if (lookScript != null && lookScript.target == Vector3.zero) {
            CleanUpLaser();
        }

        if (!autoFire && (reticleTargetScript != null || parentReticleTargetScript != null)) {
            CleanUpLaser();
        }

        if (laserMiddle.activeSelf) {
            if (!SoundManager.IsLoopedSoundPlaying()) {
                if(fireSound != null) {
                    fireSound.PlaySound();
                }
            }
        }else if (SoundManager.IsLoopedSoundPlaying()) {
            SoundManager.StopLoopedSound();
        }
    }

    void CleanUpLaser() {
        if (start != null)
            start.SetActive(false);

        if (middle != null)
            middle.SetActive(false);

        if (end != null)
            end.SetActive(false);
    }

    public override void Fire() {
        // Activate the start piece
        if (!start.activeSelf) {
            start.SetActive(true);
        }

        // Activate the Middle bit
        if (!middle.activeSelf) {
            middle.SetActive(true);
        }

        // Define an "infinite" size, not too big but enough to go off screen
        float maxLaserSize = 60f;
        float currentLaserSize = maxLaserSize;

        //Raycast Upward
        Vector2 laserDirection = this.transform.up;
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, laserDirection, maxLaserSize, mask);

        if (hit.collider != null) {
            // We touched something!
            // -- Get the laser length
            currentLaserSize = Vector2.Distance(hit.point, this.transform.position) * (1f / Mathf.Abs(transform.parent.localScale.y));
            
            // -- Activated the End Piece
            if (!end.activeSelf) {
                end.SetActive(true);

                if (parentEntity != null) {
                    laserPoint.damage = damage * parentEntity.damageModifier;
                }
                else {
                    laserPoint.damage = damage;
                }

                laserPoint.damageInterval = damageInterval;

                //Delegate Stuff
                if (onKill != null)
                    laserPoint.onKill += onKill;
            }
        }
        else {
            // Nothing hit
            if (end != null)
                end.SetActive(false);
        }

        // Place things

        // -- the middle is after start and, as it has a center pivot, have a size of half the laser (minus start and end)
        middle.transform.localScale = new Vector3(middle.transform.localScale.x, currentLaserSize - (capSpriteHeight * 2f), middle.transform.localScale.z);
        middle.transform.localPosition = new Vector2(0f, (currentLaserSize / 2f));
        
        // End?
        if (end != null) {
            end.transform.localPosition = new Vector2(0f, currentLaserSize);
        }
    }
}