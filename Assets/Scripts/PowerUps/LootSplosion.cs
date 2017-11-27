using UnityEngine;
using System.Collections;

public class LootSplosion : PowerUp {

    [Header("LootSplosion Info")]
    public Transform spawnPoint;
    public Loot[] drops;
    public int numDrops;
    public float spawnDelay;

    void Awake() {
        drops = GetComponents<Loot>();
        spawnPoint = GameObject.FindGameObjectWithTag("LootSpawn").transform;
    }

    protected override void PowerUpEffect() {
        GameManager.SetUIText("Lootsplosion!!");

        StartCoroutine(SpawnLoot());
    }

    IEnumerator SpawnLoot() {
        for (int i = 0; i < numDrops; i++) {
            int dropIndex = Random.Range(0, drops.Length);
            drops[dropIndex].DropLoot(spawnPoint.position, spawnPoint.rotation);

            yield return new WaitForSeconds(spawnDelay);
        }
    }
}