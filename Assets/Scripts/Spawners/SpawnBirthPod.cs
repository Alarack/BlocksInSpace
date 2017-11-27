using UnityEngine;
using System.Collections;

public class SpawnBirthPod : MonoBehaviour {

    public GameObject birthPod;

    [Header("Pheonix Stuff")]
    public bool isPhenoix;
    public float birthDuration;

    private Health myHealth;
    private bool isPodSpawing;

    private GameObject pod;

    void Start() {
        myHealth = GetComponent<Health>();
    }

    void Update() {
        if (myHealth.isDead && !isPodSpawing) {
            isPodSpawing = true;
            SpawnPod();
        }
    }

    void SpawnPod() {
        if (isPhenoix) {
            GameObject activeEgg = Instantiate(birthPod, transform.position, transform.rotation) as GameObject;
            string rebirthName = gameObject.name.Replace("(Clone)", "");
            GameObject nextRebirth = Resources.Load("Mobs/" + rebirthName) as GameObject;

            BirthingPod podScript = activeEgg.GetComponent<BirthingPod>();

            if (podScript == null) {
                activeEgg.AddComponent<BirthingPod>();
                BirthingPod newPodScript = activeEgg.GetComponent<BirthingPod>();
                newPodScript.gestationPerioid = birthDuration;
                newPodScript.spawn = nextRebirth;
            }
            else {
                activeEgg.GetComponent<BirthingPod>().spawn = nextRebirth;
            }
        }
        else {
            Instantiate(birthPod, transform.position, transform.rotation);
        }
    }
}