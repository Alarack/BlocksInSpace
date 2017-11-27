using UnityEngine;
using System.Collections;

public class SpecialWeaponController : MonoBehaviour {

    public enum SpecialWeaponCondition {
        AtWill,
        WhenWounded,
        WhileHealthy
    }

    public SpecialWeaponCondition condition;
    public float healthThreshold;

    private Weapon specialWeapon;
    private Health myHealth;
    private EntityEnemy npcSelf;

	void Start () {
        if(GetComponent<Entity>().activeSpecial != null)
            specialWeapon = GetComponent<Entity>().activeSpecial.GetComponent<Weapon>();
        myHealth = GetComponent<Health>();
        npcSelf = GetComponent<EntityEnemy>();
	}

	void Update () {

        switch (condition) {

            case SpecialWeaponCondition.AtWill:
                if (npcSelf != null && specialWeapon.CanFire() && npcSelf.autoFire)
                    specialWeapon.Fire();

                break;

            case SpecialWeaponCondition.WhenWounded:
                if (myHealth.curHealth / myHealth.maxHealth <= healthThreshold && specialWeapon.CanFire())
                    specialWeapon.Fire();


                break;

            case SpecialWeaponCondition.WhileHealthy:
                if (myHealth.curHealth / myHealth.maxHealth >= healthThreshold && specialWeapon.CanFire())
                    specialWeapon.Fire();

                break;
        }
	}
}