using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DroneLauncher : Weapon {

    [Header("DroneLauncher Info")]
    public int maxDrones;
    //public float thrusterScaleMod = 1.0f;
    [Header("Color Stuff")]
    public bool inheritColorFromParent = true;
    public EnumList.Colors droneColor;

    private List<GameObject> currentDrones = new List<GameObject>();

    public override void Fire() {
        if (burstMode) {
            StartCoroutine(BurstFire());
            lastFire = Time.time + coolDown;
        }
        else {
            CreateDrone();
            lastFire = Time.time + coolDown;
        }
    }

    protected override IEnumerator BurstFire() {
        for (int i = 0; i < numBurstShots; i++) {
            CreateDrone();
            yield return new WaitForSeconds(burstDelay);
        }
    }

    public override bool CanFire() {
        return Time.time > lastFire && CheckCurrentDrones();
    }

    //TODO: this is the same as in the DroneBay
    public bool CheckCurrentDrones() {
        for (int i = currentDrones.Count - 1; i > -1; i--) {
            if (currentDrones[i] == null)
                currentDrones.RemoveAt(i);
        }
        if (currentDrones.Count < maxDrones) {
            return true;
        }
        else {
            return false;
        }
    }

    void CreateDrone() {
        GameObject activeDrone = Instantiate(weaponSpawn, myShotOrigin.transform.position, myShotOrigin.transform.rotation) as GameObject;

        if (fireSound != null) {
            fireSound.PlaySound();
        }

        if (parentEntity != null)
            activeDrone.transform.localScale *= (shotScale * parentEntity.weaponShotScaleMod);
        else
            activeDrone.transform.localScale *= (shotScale);

        Drone droneScript = activeDrone.GetComponent<Drone>();
        droneScript.parentWeapon = this;

        if (parentEntity != null)
            droneScript.thrusterScaleMod *= parentEntity.weaponShotScaleMod;

        droneScript.owner = GetComponentInParent<Entity>();

        if (shotSpeed != 0f && droneScript.GetComponent<EntityMovement>() != null)
            droneScript.GetComponent<EntityMovement>().speed = shotSpeed;

        if (parentEntity != null) {

            if (inheritColorFromParent) {
                droneScript.unitColor = parentEntity.GetColor();
            }
            else {
                droneScript.unitColor = droneColor;
            }
            activeDrone.gameObject.tag = "Drone";
            activeDrone.gameObject.layer = parentEntity.gameObject.layer;
            //droneScript.damage = damage * parentEntity.damageModifier;
        }
        currentDrones.Add(activeDrone);
    }
}