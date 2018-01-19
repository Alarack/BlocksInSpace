using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EggLauncher : Weapon {

    [Header("Egg Launcher Info")]
    public List<GameObject> potentialSpawns = new List<GameObject>();
    public GameObject baseEgg;

    protected void CreateEgg(Transform aimHelper = null) {

        if (fireSound != null) {
            fireSound.PlaySound();
        }

        int randomEgg = Random.Range(0, potentialSpawns.Count);

        GameObject activeEgg = Instantiate(baseEgg, myShotOrigin.transform.position, myShotOrigin.transform.rotation) as GameObject;
        activeEgg.transform.localScale *= shotScale;

        BirthingPod eggScript = activeEgg.GetComponent<BirthingPod>();

        eggScript.spawn = potentialSpawns[randomEgg];

        if (shotSpeed != 0f && activeEgg.GetComponent<EntityMovement>() == null) {
            activeEgg.AddComponent<BasicMovement>();
            BasicMovement eggMoves = activeEgg.GetComponent<BasicMovement>();

            eggMoves.moveType = BasicMovement.MoveType.SetVelocity;
            eggMoves.direction = BasicMovement.Direction.Up;
            eggMoves.speed = shotSpeed;
        }
        else {
            activeEgg.GetComponent<EntityMovement>().speed = shotSpeed;
        }
            
        if (aimHelper != null && useAimHelpers) {
            activeEgg.transform.LookAt(transform.position + new Vector3(0, 0, 1), aimHelper.position - transform.position);
        }
        else {
            Vector2 randomDir = Random.insideUnitCircle;
            activeEgg.transform.rotation = Quaternion.FromToRotation(activeEgg.transform.position, randomDir);
        }
    }

    protected override void CreateProjectile(Transform aimHelper = null, float error = 0) {
        CreateEgg(aimHelper);
    }
}