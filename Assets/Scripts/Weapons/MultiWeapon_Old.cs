using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiWeapon_Old : MissileSwarmer {

    public List<Vector2> weaponPoints = new List<Vector2>();
    //public float numWeaponPoints;
    public float weaponPointDistance;
    public GameObject newWeaponPoint;

    private List<Transform> finalWeaponPoints = new List<Transform>();

    protected override void Start() {
        base.Start();
        for (int i = 0; i < numMissiles; i++) {
            Vector2 rightPoint = new Vector2(shotOrigin.position.x + weaponPointDistance, shotOrigin.position.y);

            Vector2 leftPoint = new Vector2(shotOrigin.position.x - weaponPointDistance, shotOrigin.position.y);

            weaponPoints.Add(rightPoint);
            weaponPoints.Add(leftPoint);

            weaponPointDistance *= 2;

            GameObject activeWeaponPoint = Instantiate(newWeaponPoint, weaponPoints[i], Quaternion.identity) as GameObject;
            activeWeaponPoint.transform.parent = gameObject.transform;

            finalWeaponPoints.Add(activeWeaponPoint.transform);
        }
    }

    protected override IEnumerator BurstDelay() {
        for (int i = 0; i < numMissiles; i++) {
            myShotOrigin = finalWeaponPoints[i];
            CreateProjectile(finalWeaponPoints[i]);

            yield return new WaitForSeconds(burstDelay);
        }
    }
}