using UnityEngine;
using System.Collections;

public class MissileSwarmer : Weapon {

    [Header("Multi-Shot Info")]
    public Transform[] aimHelpers;
    public float numMissiles;

    public bool ordredFire = true;

    private float burstTimer;

    public override void Fire() {
        if(burstDelay > 0)
            StartCoroutine(BurstDelay());
        else
            MultiFire();

        lastFire = Time.time + coolDown;
    }

    protected virtual IEnumerator BurstDelay() {
        for (int i = 0; i < numMissiles; i++) {
            if (ordredFire) {
                if (aimHelpers.Length > 0)
                    CreateProjectile(aimHelpers[i]);
                else
                    CreateProjectile();
            }
            else {
                int mixed = Random.Range(0, aimHelpers.Length);
                CreateProjectile(aimHelpers[mixed]);
            }
            yield return new WaitForSeconds(burstDelay);
        }
    }

    protected override void MultiFire() {
        for (int i = 0; i < numMissiles; i++) {
            if (ordredFire && aimHelpers.Length > 0) {
                CreateProjectile(aimHelpers[i]);
            }
            else {
                int mixed = Random.Range(0, aimHelpers.Length);
                CreateProjectile(aimHelpers[mixed]);
            }
        }
    }
}