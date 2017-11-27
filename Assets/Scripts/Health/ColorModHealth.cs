using UnityEngine;
using System.Collections;

public class ColorModHealth : Health {

    private ColorModule self;

    void Awake() {
        self = GetComponent<ColorModule>();
    }

    protected override void EntityDead() {
        if (self.myPrimaryWeapon != null)
            self.mainShip.fullWeaponList.Remove(self.myPrimaryWeapon);

        self.mainShip.colorModules.Remove(self);
        transform.parent.GetComponent<Health>().AdjustHealth(Mathf.Round(maxHealth / 3));

        foreach (ColorModule mod in self.neighbors) {
            if (mod.neighbors.Contains(self)) {
                mod.RemoveNeighbor(self);
            }
        }

        DeathFlair();
        Destroy(gameObject);
    }

    //protected override void DeathFlair() {
    //    GameObject activeExplosion = Instantiate(EnumList.ExplosionColor(self.unitColor), transform.position, transform.rotation) as GameObject;
    //    Destroy(activeExplosion, 1f);
    //}
}