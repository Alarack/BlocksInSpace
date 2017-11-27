using UnityEngine;
using System.Collections;

public class AmmoUp : PowerUp {

    [Header("Ammo Up Info")]
    public int ammoCount;

    protected override void PowerUpEffect() {
        GameManager.missiles += ammoCount;
    }
}