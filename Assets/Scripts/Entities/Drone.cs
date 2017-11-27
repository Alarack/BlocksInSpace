using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Drone : EntityEnemy {

    private const int INFINITE_DURATION = -1;
    [Header("Drone Damage")]
    public float damage;
    public float weaponCooldown;

    [Header("Drone Lifetime")]
    public float maxDuration;

    [Header("Drone FireOptions")]
    public bool autoFireWhenTargetInRange = true;

    //[HideInInspector]
    public Entity owner;
    [HideInInspector]
    public Weapon parentWeapon;
    private float curDuration = 0f;
    private Health myHealth;
    private LookAtTarget lookScript;
    private CorralMovement myCorral;
    private Transform myLeashPoint;
    private List<Transform> dronePoints = new List<Transform>();

    protected override void Start() {
        base.Start();
        EnumList.ConfirmWeaponLayer(gameObject, activePrimary, activeSpecial);

        Transform dronebay = owner.transform.FindChild("DroneBay");

        if (dronebay == null)
            dronebay = owner.transform.parent.FindChild("DroneBay");


        if (owner != null) {
            Transform[] tmpdronePoints = dronebay.GetComponentsInChildren<Transform>();

            foreach (Transform t in tmpdronePoints) {
                if (t.gameObject.tag == "DronePoint") {
                    dronePoints.Add(t);
                }
            }

            int randomPoint = Random.Range(0, dronePoints.Count);
            myLeashPoint = dronePoints[randomPoint];
        }

        myHealth = GetComponent<Health>();
        if(activePrimary != null)
            lookScript = activePrimary.GetComponent<LookAtTarget>();
        myCorral = GetComponent<CorralMovement>();
    }

    protected override void Update() {
        base.Update();

        if (owner != null && owner.GetComponent<Health>().isDead)
            GetComponent<Health>().KillEntity();

        if (myHealth != null)
            GrowOld();

        if (autoFireWhenTargetInRange) {
            if (lookScript != null) {
                if (lookScript.target == Vector3.zero)
                    autoFire = false;
                else
                    autoFire = true;
            }
        }

        if (myCorral != null && owner != null) {
            myCorral.target = myLeashPoint.position;
        }
    }

    void GrowOld() {
        curDuration += Time.deltaTime;

        if(maxDuration != INFINITE_DURATION && curDuration >= maxDuration) {
            myHealth.KillEntity();
        }
    }
}