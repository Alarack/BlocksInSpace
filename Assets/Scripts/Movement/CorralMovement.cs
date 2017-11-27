using UnityEngine;
using System.Collections;

public class CorralMovement : BasicMovement {

    [Header("Corral Info")]
    public float followDistance;
    public float turnSpeed = 1f;
    public bool correctChildWeaponRotation = true;

    private Entity self;
    private AutoSpin spinScript;
    private Entity owner;

    protected override void Start() {
        base.Start();
        self = GetComponent<Entity>();
        owner = GetComponent<Drone>().owner;
        spinScript = GetComponent<AutoSpin>();

        ApplyAutoSpin();
    }

    void Update() {
        ToggleSpin();
    }

    //void DirectedTowardOwner() {
    //    if (TargetUtils.FindDistance(target, transform.position) > followDistance) {
    //        direction = Direction.Directed;
    //    }
    //    else {
    //        direction = Direction.None;
    //    }
    //}

    void TurnAtEdge() {
        if (TargetUtils.FindDistance(target, transform.position) > followDistance) {
            Quaternion storedRotation = transform.rotation;
            transform.rotation = TargetUtils.SmoothRotation(target, transform, turnSpeed);

            //Rotation Correction for Child Weapon
            if (correctChildWeaponRotation && self != null && self.activePrimary != null) {
                Quaternion diff = Quaternion.Inverse(transform.rotation) * storedRotation;
                self.activePrimary.transform.rotation *= diff;
            }
        }
        else {
            direction = Direction.Up;
        }
    }

    void ToggleSpin() {
        if (TargetUtils.FindDistance(target, transform.position) > followDistance && !SeekOwner()) {
            spinScript.enabled = true;
        }
        else {
            int rand = Random.Range(0, 2);
            if (rand == 0) {
                spinScript.rotateSpeed *= -1;
            }
            spinScript.enabled = false;
        }
    }

    void ApplyAutoSpin() {
        if (spinScript == null) {
            gameObject.AddComponent<AutoSpin>();
            spinScript = GetComponent<AutoSpin>();
            spinScript.spinTransform = true;
            spinScript.rotateSpeed = turnSpeed;
            spinScript.correctChildWeaponRotation = true;
        }
    }

    bool SeekOwner() {
        RaycastHit2D hit = Physics2D.Raycast(new Vector3(transform.position.x + 2f, transform.position.y, transform.position.z), transform.up, Mathf.Infinity);
        if (hit.collider != null && (WhatDidIHit<Entity>(hit) == owner || owner.colorModules.Contains(WhatDidIHit<ColorModule>(hit)))) {
            return true;
        }
        else {
            return false;
        }
    }

    T WhatDidIHit<T>(RaycastHit2D hit) where T : Entity {
        return hit.collider.GetComponent<T>();
    }

}