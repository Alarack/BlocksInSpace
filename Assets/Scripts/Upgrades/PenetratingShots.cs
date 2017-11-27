using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PenetratingShots : ModifyProjectile {

    protected override void ModifyShotFired(GameObject shot) {
        shot.GetComponent<Projectile>().piercing = true;
        shot.GetComponent<Projectile>().maxPierce = 2;
    }

    protected override Upgrade GetTargetStatus<T>(T target) {
        return target.GetComponent<PenetratingShots>();
    }

    public override void StackEffect() {
        foreach (Weapon w in player.fullWeaponList) {
            w.onShotFired += ModifyPiercing;
        }
    }

    void ModifyPiercing(GameObject shot) {
        if (shot.GetComponent<Projectile>().piercing)
            shot.GetComponent<Projectile>().maxPierce += 1;
    }

    protected override void AddToTarget<T>(T target) {
        target.gameObject.AddComponent<PenetratingShots>();
        PenetratingShots activePenetratingShot = target.gameObject.GetComponent<PenetratingShots>();
        activePenetratingShot.useOnShotFired = useOnShotFired;
        activePenetratingShot.upgradeIcon = upgradeIcon;
    }
}