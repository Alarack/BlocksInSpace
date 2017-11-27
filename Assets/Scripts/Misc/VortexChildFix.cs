using UnityEngine;
using System.Collections;

public class VortexChildFix : MonoBehaviour {

	void Update () {

        if (transform.parent == null)
            CleanUp();
    }

    void CleanUp() {
        if (GetComponentInChildren<ParticleSystem>() != null) {
            foreach (Transform child in transform) {
                child.GetComponentInChildren<ParticleSystem>().loop = false;
                Destroy(child.GetComponentInChildren<OscillateMovement>());
                Destroy(child.gameObject, 4f);

                //child.parent = child;
            }
            transform.DetachChildren();
        }
    }
}