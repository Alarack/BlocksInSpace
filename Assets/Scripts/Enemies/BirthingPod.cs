using UnityEngine;
using System.Collections;

public class BirthingPod : MonoBehaviour {

    private const int INFINITE_SPAWN = -1;

    [Header("Spawing Info")]
    public GameObject spawn;
    public float gestationPerioid;
    [Header("Multi-Spawn Stuff")]
    public bool repeatSpawn;
    public int maxSpawn = INFINITE_SPAWN;
    

    private int curSpawn = 0;
    private float gestationTimer;
    private bool isSpawning;

    void Update() {

        if (gestationTimer < gestationPerioid)
            Gestate();

        if (gestationTimer >= gestationPerioid && !isSpawning)
            Spawn();
    }

    void Spawn() {
        isSpawning = true;

        if (repeatSpawn && curSpawn == maxSpawn) {
            GetComponent<Health>().KillEntity();
        }
        else if (repeatSpawn && curSpawn < maxSpawn) {
            SpawnRandomColorUnit();
            gestationTimer = 0;
            isSpawning = false;
            curSpawn++;
        }
        else {
            SpawnRandomColorUnit();
            GetComponent<Health>().KillEntity();
        }
    }

    void Gestate() {
        gestationTimer += Time.deltaTime;
    }

    void SpawnRandomColorUnit() {
        GameObject activeSpawn = Instantiate(spawn, transform.position, transform.rotation) as GameObject;
        if (activeSpawn.GetComponent<Entity>() != null && activeSpawn.GetComponent<Entity>().unitColor != EnumList.Colors.Grey) {
            activeSpawn.GetComponent<Entity>().unitColor = EnumList.RandomColor();
        }
    }
}