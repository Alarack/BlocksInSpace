using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DetonateEnemy : ModifyProjectile {

    [Header("Detonate Enemy Stats")]
    public int numRockets;
    public GameObject deathRocket;

    void LaunchDeathRockets(GameObject target) {
        GameObject missileBarrage = Instantiate(deathRocket, target.transform.position, Quaternion.identity) as GameObject;
        missileBarrage.GetComponent<DeathMissileController>().numMissiles = numRockets;
    }

    protected override void ModifyKillShot(GameObject target) {
        GameObject missileBarrage = Instantiate(deathRocket, target.transform.position, Quaternion.identity) as GameObject;
        missileBarrage.GetComponent<DeathMissileController>().numMissiles = numRockets;
    }

    protected override Upgrade GetTargetStatus<T>(T target) {
        return target.GetComponent<DetonateEnemy>();
    }

    public override void StackEffect() {
        if (numRockets < 8)
            numRockets += 1;
    }

    protected override void AddToTarget<T>(T target) {
        target.gameObject.AddComponent<DetonateEnemy>();
        DetonateEnemy activeDetonation = target.gameObject.GetComponent<DetonateEnemy>();
        activeDetonation.numRockets = numRockets;
        activeDetonation.deathRocket = deathRocket;
        activeDetonation.useOnKill = useOnKill;
        activeDetonation.upgradeIcon = upgradeIcon;
    }
}