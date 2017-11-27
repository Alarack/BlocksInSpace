using UnityEngine;
using System.Collections;

public class MultiSpriteExplosion : MonoBehaviour {

    public GameObject[] explosions;
    public int numExplosions;
    public float delay;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            StartCoroutine(BeginExploding());
            //Destroy(gameObject, numExplosions * delay);
        }
    }

    public IEnumerator BeginExploding() {
        for (int i = 0; i < numExplosions; i++) {
            float randX = Random.Range(-1f, 1f);
            float randY = Random.Range(-1f, 1f);
            float randomDeg = Random.Range(0, 360f);

            int boomIndex = Random.Range(0, explosions.Length);
            Instantiate(explosions[boomIndex], new Vector2(transform.position.x + randX, transform.position.y + randY), Quaternion.Euler(transform.rotation.x, transform.rotation.y, randomDeg));
            yield return new WaitForSeconds(delay);
        }
    }
}