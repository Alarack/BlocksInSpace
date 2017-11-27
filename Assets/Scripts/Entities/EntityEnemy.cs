using UnityEngine;
using System.Collections;

public class EntityEnemy : Entity {

    [Header("Misc Options")]
    public bool shootDown = true;
    public bool shootColorModsDown = false;
    //public bool lockedAim = false;
    public float aimDegree = 180f;
    public bool autoFire = true;
    [Header("Sprite Stuff")]
    public Sprite[] sprites;

    protected override void Start() {
        base.Start();
        if (unitColor != EnumList.Colors.Grey)
            EnumList.ColorInit(unitColor, mySprite, sprites);
    }

    protected override void Update() {
        base.Update();

        if (shootDown)
            AimDown();

        if (autoFire)
            FireAllWeapons();
    }

    public virtual void FireAllWeapons() {
        foreach (Weapon weapon in fullWeaponList) {
            if (weapon.CanFire())
                weapon.Fire();
        }
    }

    public void AimDown() {
        foreach (Weapon weapon in fullWeaponList) {
            if (weapon != null) {
                if (primeWeaponMount != null) {
                    primeWeaponMount.transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, aimDegree));
                }
                else {
                    weapon.gameObject.transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, aimDegree));
                }

                if(weapon.GetComponentInParent<ColorModule>() != null && shootColorModsDown) {
                    weapon.gameObject.transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, aimDegree));
                }

                if (weapon.GetComponent<LookAtTarget>() != null && weapon.GetComponent<LookAtTarget>().activateLookAt == true) {
                    weapon.GetComponent<LookAtTarget>().activateLookAt = false;
                }

            }
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            float damage = GetComponent<Health>().curHealth / 5;

            if (damage > 25f)
                damage = 25f;

            other.gameObject.GetComponent<Health>().AdjustHealth(Mathf.Round(damage));
            GetComponent<Health>().KillEntity();
        }
        else if (other.gameObject.tag == "Drone") {
            float damage = other.gameObject.GetComponent<Health>().curHealth / 5;

            if (damage > 50f)
                damage = 50f;

            GetComponent<Health>().AdjustHealth(damage);
            other.gameObject.GetComponent<Health>().KillEntity();
        }
    }
}