using UnityEngine;
using System.Collections;

public class ShotGun : Weapon {

    [Header("ShotGun Info")]
    public int numShots;
    public float minSpeedVar = 1.5f;
    public float maxSpeedVar = 2f;

    private float normalShotSpeed;

    protected override void Start() {
        base.Start();
        normalShotSpeed = shotSpeed;
    }

    public override void Fire() {
        for (int i = 0; i < numShots; i++) {
            shotSpeed = Random.Range(normalShotSpeed * minSpeedVar, normalShotSpeed / maxSpeedVar);
            CreateProjectile(null, error);
        }

        lastFire = Time.time + coolDown;
    }
}