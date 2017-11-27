using UnityEngine;
using System.Collections;

public class EnergyDisk : Upgrade {

    [Header("Energy Disk Info")]
    public GameObject energyDisk;
    public float distance;
    public float damage;
    public float damageInterval;

    private GameObject activeDisk;


    protected override void Start() {
        base.Start();

        if (player != null)
            CreateEnergyDisk();
    }

    void CreateEnergyDisk() {
        GameObject disk = Instantiate(energyDisk) as GameObject;

        disk.transform.SetParent(player.transform.FindChild("OrbitBay").transform, false);
        disk.transform.localPosition = new Vector2(0f, distance);

        LingeringProjectile diskScript = disk.GetComponent<LingeringProjectile>();
        diskScript.damage = damage;
        diskScript.damageInterval = damageInterval;

        activeDisk = disk;
    }

    protected override Upgrade GetTargetStatus<T>(T target) {
        return target.GetComponent<EnergyDisk>();
    }

    public override void StackEffect() {
        activeDisk.GetComponent<LingeringProjectile>().damage += 3f;
    }

    protected override void AddToTarget<T>(T target) {
        target.gameObject.AddComponent<EnergyDisk>();
        EnergyDisk activeDiskUpgrade = target.gameObject.GetComponent<EnergyDisk>();
        activeDiskUpgrade.energyDisk = energyDisk;
        activeDiskUpgrade.damage = damage;
        activeDiskUpgrade.damageInterval = damageInterval;
        activeDiskUpgrade.distance = distance;
        activeDiskUpgrade.activeDisk = activeDisk;
        activeDiskUpgrade.upgradeIcon = upgradeIcon;
    }
}