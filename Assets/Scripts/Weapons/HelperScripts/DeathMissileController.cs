using UnityEngine;
using System.Collections;

public class DeathMissileController : MonoBehaviour {

    public int numMissiles;

    private MissileSwarmer myWeapon;

    void Start() {
        myWeapon = GetComponent<MissileSwarmer>();

        myWeapon.numMissiles = numMissiles;
        StartCoroutine(FireMissiles());
    }

    IEnumerator FireMissiles() {
        yield return new WaitForSeconds(0.1f);
        myWeapon.Fire();
        yield return new WaitForSeconds(myWeapon.burstDelay * numMissiles + 0.1f);
        Destroy(gameObject);
    }
}