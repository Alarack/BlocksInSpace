using UnityEngine;
using System.Collections;

public class OneShotWeapon : PowerUp {

    protected override void PowerUpEffect() {
        GameManager.missiles += 1;
        if (weapons.Length > 0) {
            foreach (Weapon w in weapons) {
                w.Fire();
            }
        }
    }
}