using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnerShot : Projectile {

    [Header("SpawnerShot Info")]
    public float spawnInterval;

    public bool beginSpawning = true;
    public bool delaySpawning;
    public float spawnDelay;
    public bool limitedSpawns;
    public int numSpawns;
    [Space(10)]
    public bool useAimHelpers;
    public Transform[] aimHelpers;
    public bool parentChildShots;

    private int spawnCounter;
    private float spawnTimer;
    private float lastSpawn;

    private List<GameObject> subShots = new List<GameObject>();

    protected override void Update() {
        base.Update();

        if (CanSpawn())
            Spawn();


        if (delaySpawning) {
            beginSpawning = false;
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnDelay) {
                beginSpawning = true;
                delaySpawning = false;
            }                

        }
    }

    bool CanSpawn() {
        return Time.time > lastSpawn && beginSpawning;
    }

    void Spawn() {

        if (useAimHelpers) {
            for (int i = 0; i < aimHelpers.Length; i++) {
                if (parentChildShots) {
                    subShots.Add(SpawnChildProjectile(aimHelpers[i], true));
                }
                else {
                    subShots.Add(SpawnChildProjectile(aimHelpers[i], false));
                }

                subShots[i].GetComponent<BasicMovement>().target = aimHelpers[i].position;
            }
        }
        else {
            if (limitedSpawns && spawnCounter < numSpawns) {
                SpawnChildProjectile();
                spawnCounter++;
            }
            else if (!limitedSpawns) {
                SpawnChildProjectile();
            }
        }


        lastSpawn = Time.time + spawnInterval;
    }
}