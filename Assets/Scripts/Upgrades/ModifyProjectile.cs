using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ModifyProjectile : Upgrade {

    [Header("Modify Projectile Bools")]
    public bool useOnKill;
    public bool useOnShotFired;

    protected List<Weapon> upgradedWeapons = new List<Weapon>();

    protected override void Update() {
        CountAndApplyToWeapons();
    }

    protected virtual void CountAndApplyToWeapons() {
        if (player != null && upgradedWeapons.Count != player.fullWeaponList.Count) {
            List<Weapon> tempWeapons = player.fullWeaponList;

            foreach (Weapon playerWeapon in tempWeapons) {
                if (!upgradedWeapons.Contains(playerWeapon)) {
                    if (useOnShotFired)
                        playerWeapon.onShotFired += ModifyShotFired;

                    if (useOnKill)
                        playerWeapon.onKill += ModifyKillShot;

                    upgradedWeapons.Add(playerWeapon);
                }
            }
        }
    }

    protected virtual void ModifyShotFired(GameObject shot) {

    }

    protected virtual void ModifyKillShot(GameObject target) {

    }

    protected override Upgrade GetTargetStatus<T>(T target) {
        return target.GetComponent<ModifyProjectile>();
    }
}