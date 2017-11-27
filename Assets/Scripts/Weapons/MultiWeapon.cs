using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiWeapon : Weapon {

    [Header("MultiWeapon Stuff")]
    public GameObject[] childWeapons;
    public Transform[] weaponLocations;
    public bool overrideCooldowns;
    public float[] childWeaponCooldownOverrides;
    public float[] childScaleMods;
    public float childWeaponScaleMod = 1f;
    public float weaponFireDelay;


    private List<Weapon> activeWeapons = new List<Weapon>();

    protected override void Start () {
        base.Start();

        foreach (Transform point in weaponLocations) {
            point.gameObject.layer = gameObject.layer;

        }
        //TODO: Thos is very similar to InitWeapon
        for (int i = 0; i < childWeapons.Length; i++) {
            GameObject tmpWpn = Instantiate(childWeapons[i]) as GameObject;

            tmpWpn.gameObject.tag = "Weapon";
            tmpWpn.gameObject.layer = gameObject.layer;

            if(childScaleMods.Length == 0)
                tmpWpn.transform.localScale *= childWeaponScaleMod;

            if (tmpWpn.GetComponent<LookAtTarget>() != null)
                Destroy(tmpWpn.GetComponent<LookAtTarget>());

            tmpWpn.transform.SetParent(weaponLocations[i], false);
            tmpWpn.transform.localPosition = Vector2.zero;


            Weapon wpnScript = tmpWpn.GetComponent<Weapon>();

            wpnScript.damage += damage;
            wpnScript.error += error;

            activeWeapons.Add(wpnScript);


            if(childScaleMods.Length > 0) {
                activeWeapons[i].transform.localScale *= childScaleMods[i];
            }

            if (overrideCooldowns) {
                activeWeapons[i].coolDown = childWeaponCooldownOverrides[i];
            }

            //parentEntity.fullWeaponList.Add(wpnScript);

            

        }

	}

    public override void Fire() {
        StartCoroutine(FireSubWeapons(activeWeapons));
        lastFire = Time.time + coolDown;
    }

    IEnumerator FireSubWeapons(List<Weapon> weapons) {

        foreach(Weapon w in weapons) {
            if (w.CanFire()) {
                w.Fire();
            }
            yield return new WaitForSeconds(weaponFireDelay);
        }
    }
}