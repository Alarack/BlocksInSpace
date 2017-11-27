using UnityEngine;
using System.Collections;

public class TriShot : Upgrade {

    [Header("TriShot Info")]
    public GameObject newWeaponHolder;

    private GameObject baseWeapon;
    private Transform[] newWeaponLocations;

    protected override void Start() {
        base.Start();

        if (player != null) {
            baseWeapon = player.activePrimary;
            CreateNewWeapons();
        }
    }
    //TODO: Perhaps instead of creating two new weapons, I could add aim-helpers to the current weapon, then make it use those aimhelpers and turn on burst mode.
    void CreateNewWeapons() {
        GameObject activeWeaponHolder = Instantiate(newWeaponHolder) as GameObject;
        activeWeaponHolder.transform.parent = transform;
        activeWeaponHolder.transform.localPosition = Vector2.zero;

        newWeaponLocations = activeWeaponHolder.GetComponentsInChildren<Transform>();

        foreach (Transform t in newWeaponLocations) {
            GameObject activeNewWeapon = Instantiate(baseWeapon) as GameObject;
            EnumList.InitWeapon(player.gameObject, activeNewWeapon);
            //activeNewWeapon.name = "Weapon";
            //activeNewWeapon.transform.parent = transform;
            //activeNewWeapon.transform.localPosition = Vector2.zero;
            activeNewWeapon.transform.rotation = t.rotation;
            player.fullWeaponList.Add(activeNewWeapon.GetComponent<Weapon>());
            player.activePrimary = activeNewWeapon; //This isn't so good. It replaces the weapon a bunch of unneeded times.
        }

        player.fullWeaponList.Remove(baseWeapon.GetComponent<Weapon>());
        Destroy(baseWeapon);
        Destroy(activeWeaponHolder);
    }

    protected override Upgrade GetTargetStatus<T>(T target) {
        return target.GetComponent<TriShot>();
    }

    public override void StackEffect() {
        //TODO: Add a stack effect to TriShot
    }

    protected override void AddToTarget<T>(T target) {
        target.gameObject.AddComponent<TriShot>();
        TriShot activeTriShotScript = target.gameObject.GetComponent<TriShot>();
        activeTriShotScript.baseWeapon = baseWeapon;
        activeTriShotScript.newWeaponHolder = newWeaponHolder;
        activeTriShotScript.newWeaponLocations = newWeaponLocations;
        activeTriShotScript.upgradeIcon = upgradeIcon;
    }
}