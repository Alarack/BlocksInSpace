using UnityEngine;
using System.Collections;

public class MobSpinner : PowerUp {

    protected override void PowerUpEffect() {
        GameManager.SetUIText("Feeling Dizzy?");
        CameraController.ShakeCam(0.05f, 0.5f);
        EntityEnemy[] allMobs = FindObjectsOfType<EntityEnemy>();

        foreach (EntityEnemy mob in allMobs) {

            if(mob.gameObject.tag == "Enemy" && mob.GetComponent<Rigidbody2D>() != null && mob.GetComponent<ScreenWrap>() != null) {

                if (mob.GetComponent<ScreenWrap>().IsBeingRendered()) {
                    float spinVar = Random.Range(-500f, 500f);

                    mob.GetComponent<Rigidbody2D>().angularVelocity += spinVar;

                    if (mob.shootDown)
                        mob.shootDown = false;
                }
            }
        }
    }
}