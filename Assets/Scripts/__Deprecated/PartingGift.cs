using UnityEngine;
using System.Collections;

public class PartingGift : MonoBehaviour {

    public GameObject[] gifts;
    public float delay;
    public bool isShotSpawn = false;
    public bool repeatSpawn = false;
    public float repeatSpawnDelay;
    public int numSpawns;
    public bool spawnInfinite;

    private Entity owner;
    private Projectile myProjectile;

    void Start() {

        owner = GetComponent<Entity>();
        myProjectile = GetComponent<Projectile>();

        if (owner == null)
            isShotSpawn = true;

        if (repeatSpawn)
            delay = myProjectile.life;
    }

    void Update() {

        if (!isShotSpawn && owner.GetComponent<Health>().isDead) {
            SpawnFromEntity();
        }

        if (isShotSpawn && !repeatSpawn) {
            delay -= Time.deltaTime;

            if (delay <= 0) {
                SpawnFromShot();
                Destroy(gameObject);
            }
        }

        if (isShotSpawn && repeatSpawn) {
            repeatSpawn = false;
            StartCoroutine(RepeatSpawn());
        }
    }

    void SpawnFromEntity() {
        for (int i = 0; i < gifts.Length; i++) {
            //float xVar = Random.Range(-5f, 5f);
            //float yVar = Random.Range(-5f, 5f);
            GameObject activeGift = Instantiate(gifts[i], new Vector2(transform.position.x, transform.position.y), transform.rotation) as GameObject;
            Rigidbody2D giftBody = activeGift.GetComponent<Rigidbody2D>();
            giftBody.angularVelocity = Random.Range(-200f, 200f);
        }
    }

    public void SpawnFromShot() {
        for (int i = 0; i < gifts.Length; i++) {
            float xVar = Random.Range(-180f, 180f);
            //float yVar = Random.Range(-5f, 5f);
            GameObject activeGift = Instantiate(gifts[i], transform.position, transform.rotation) as GameObject;
            //Rigidbody2D giftBody = activeGift.GetComponent<Rigidbody2D>();
            //giftBody.angularVelocity = Random.Range(-200f, 200f);
            activeGift.transform.rotation = Quaternion.Euler(0f, 0f, xVar);

            SetGiftLayer(activeGift);
        }
    }

    IEnumerator RepeatSpawn() {
        for (int i = 0; i < numSpawns; i++) {
            int spawnIndex = Random.Range(0, gifts.Length);
            GameObject activeGift = Instantiate(gifts[spawnIndex], transform.position, transform.rotation) as GameObject;
            Projectile projectileScript = activeGift.GetComponent<Projectile>();
            if (projectileScript != null)
                projectileScript.damage = myProjectile.damage;

            SetGiftLayer(activeGift);

            if (spawnInfinite)
                numSpawns++;

            if (isShotSpawn)
                projectileScript.parentColor = GetComponent<Projectile>().parentColor;

            yield return new WaitForSeconds(repeatSpawnDelay);
        }
    }

    void SetGiftLayer(GameObject gift) {
        if (gameObject.layer == 8) {
            gift.GetComponent<Projectile>().playerShot = true;
        }
        else if (gameObject.layer != 8) {
            gift.GetComponent<Projectile>().playerShot = false;
        }
    }
}