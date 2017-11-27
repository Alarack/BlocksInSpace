using UnityEngine;
using System.Collections;

public class Vortex : Projectile {

    public float spinRate;

    public Projectile[] myChildShots;

    void Awake() {

    }

    protected override void Start() {
        if (life > 0f)
            Invoke("CleanUp", life);

        //if (parentWeapon != null)
        //    transform.parent = parentWeapon.transform;

        myChildShots = GetComponentsInChildren<SpawnerShot>();

        foreach (SpawnerShot s in myChildShots) {
            s.playerShot = playerShot;

        }
        //TODO: Make the vortex generate the sub-projectiles instead of having them pre-exist.

    }
}