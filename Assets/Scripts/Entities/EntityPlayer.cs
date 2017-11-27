using UnityEngine;
using System.Collections;

public class EntityPlayer : Entity {

    protected override void Update() {
        base.Update();

        if (Input.GetButton("Jump") && !GetComponent<PlayerHealth>().isDead) {
            
            foreach (Weapon weapon in fullWeaponList) {
                if (weapon.CanFire() && weapon != null) {
                    weapon.Fire();
                    CameraController.ShakeCam(0.03f, 0.1f);
                }
            }

            foreach (ColorModule c in colorModules) {
                if(c.fullWeaponList.Count > 0)
                    c.fullWeaponList[0].autoFire = true;
            }
        }

        if (Input.GetButtonUp("Jump") && !GetComponent<PlayerHealth>().isDead) {
            foreach (ColorModule c in colorModules) {
                if (c.fullWeaponList.Count > 0)
                    c.fullWeaponList[0].autoFire = false;
            }

        }

        if (Input.GetButton("Fire3") && !GetComponent<PlayerHealth>().isDead && GameManager.missiles > 0) {
            if (specialWeapon != null && specialWeapon.CanFire()) {
                specialWeapon.Fire();
                GameManager.missiles -= 1;
            }
        }
    }//End of Update

    //void RespawnPlayer()
    //{
    //    deathTimer += Time.deltaTime;
    //    if (deathTimer >= deathDelay)
    //    {
    //        deathTimer = 0f;
    //        isDead = false;
    //        //transform.position = levelManager.currentCheckpoint.transform.position;
    //        this.GetComponent<Renderer>().enabled = true;
    //        FullHealth();
    //    }
    //}

}