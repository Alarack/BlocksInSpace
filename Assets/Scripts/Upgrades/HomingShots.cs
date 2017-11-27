using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HomingShots : ModifyProjectile {

    [Header("Homing Shot Info")]
    public LayerMask whatIsTarget;
    public float targetRadius;
    public float rotateSpeed;
    public float inaccuracy;

    protected override void ModifyShotFired(GameObject shot) {
        shot.AddComponent<LookAtTarget>();
        LookAtTarget lookScript = shot.GetComponent<LookAtTarget>();
        lookScript.whatIsTarget = whatIsTarget;
        lookScript.targetRadius = targetRadius;
        lookScript.rotateSpeed = rotateSpeed;
        lookScript.inaccuracy = inaccuracy;
    }

    protected override Upgrade GetTargetStatus<T>(T target) {
        return target.GetComponent<HomingShots>();
    }

    public override void StackEffect() {
        foreach (Weapon w in player.fullWeaponList) {
            w.onShotFired += ModifyLookScript;
        }
    }

    void ModifyLookScript(GameObject shot) {
        LookAtTarget lookScript = shot.GetComponent<LookAtTarget>();
        if (lookScript != null) {
            if (lookScript.rotateSpeed < 5f) {
                lookScript.rotateSpeed += 0.5f;
            }
        }
    }

    protected override void AddToTarget<T>(T target) {
        target.gameObject.AddComponent<HomingShots>();
        HomingShots activeHomingShot = target.gameObject.GetComponent<HomingShots>();
        activeHomingShot.whatIsTarget = whatIsTarget;
        activeHomingShot.targetRadius = targetRadius;
        activeHomingShot.rotateSpeed = rotateSpeed;
        activeHomingShot.inaccuracy = inaccuracy;
        activeHomingShot.useOnShotFired = useOnShotFired;
        activeHomingShot.upgradeIcon = upgradeIcon;
    }
}