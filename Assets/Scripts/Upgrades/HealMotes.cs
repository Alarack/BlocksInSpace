using UnityEngine;
using System.Collections;

public class HealMotes : ModifyProjectile {

    [Header("Heal Mote Stats")]
    public int numMotes;
    public GameObject healMote;

    //void SpawnHealMotes(GameObject target) {
    //    GameObject activeHealMotes = Instantiate(healMote, target.transform.position, Quaternion.identity) as GameObject;
    //    activeHealMotes.GetComponent<DeathMissileController>().numMissiles = numMotes;
    //}

    protected override void ModifyKillShot(GameObject target) {
        for (int i = 0; i < numMotes; i++) {
            GameObject activeHealMote = Instantiate(healMote, target.transform.position, Quaternion.identity) as GameObject;

            float angle = Random.Range(0f, 360f);

            activeHealMote.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        }
    }

    protected override Upgrade GetTargetStatus<T>(T target) {
        return target.GetComponent<HealMotes>();
    }

    public override void StackEffect() {
        numMotes++;

    }

    protected override void AddToTarget<T>(T target) {
        target.gameObject.AddComponent<HealMotes>();
        HealMotes activeDetonation = target.gameObject.GetComponent<HealMotes>();
        activeDetonation.numMotes = numMotes;
        activeDetonation.healMote = healMote;
        activeDetonation.useOnKill = useOnKill;
        activeDetonation.upgradeIcon = upgradeIcon;
    }
}
