using UnityEngine;
using System.Collections;

public class EquipNewWeapon : Upgrade {

    [Header("Weapon Swap Stats")]
    public GameObject newWeapon;
    public bool removeOldWeapon = true;

    private GameObject oldWeaponPrefab;
    private GameObject activeWeapon;

    protected override void Start() {
        base.Start();

        if (player != null && removeOldWeapon) {
            oldWeaponPrefab = player.activePrimary;
            Weapon oldWeapon = oldWeaponPrefab.GetComponent<Weapon>();

            player.fullWeaponList.Remove(oldWeapon);
            //player.weapons.Remove(oldWeapon);
            Destroy(oldWeaponPrefab);

            CreateWeapon();
        }

        if (player != null && !removeOldWeapon) {
            CreateWeapon();
        }
    }

    void CreateWeapon() {
        GameObject tmpWeapon = Instantiate(newWeapon, transform.position, transform.rotation) as GameObject;
        EnumList.InitWeapon(player.gameObject, tmpWeapon);

        activeWeapon = tmpWeapon;
        player.activePrimary = activeWeapon;

        foreach (Weapon w in activeWeapon.GetComponents<Weapon>()) {
            //player.weapons.Add(w);
            player.fullWeaponList.Add(w);
        }
    }

    protected override Upgrade GetTargetStatus<T>(T target) {
        return target.GetComponent<EquipNewWeapon>();
    }

    public override void StackEffect() {
        activeWeapon.GetComponent<Weapon>().damage *= 1.2f;
    }

    protected override void AddToTarget<T>(T target) {
        target.gameObject.AddComponent<EquipNewWeapon>();
        EquipNewWeapon activeNewWeapon = target.gameObject.GetComponent<EquipNewWeapon>();
        activeNewWeapon.newWeapon = newWeapon;
        activeNewWeapon.activeWeapon = activeWeapon;
        activeNewWeapon.removeOldWeapon = removeOldWeapon;
        activeNewWeapon.upgradeIcon = upgradeIcon;

        //TODO: Consider adding a Destroy(this) to the end of the add to target method. This would make sure they stacking is unneeded and it would always apply a new one.
    }
}