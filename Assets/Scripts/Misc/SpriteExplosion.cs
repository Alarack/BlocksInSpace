using UnityEngine;
using System.Collections;

public class SpriteExplosion : MonoBehaviour {

    private Animator myAnim;

    void Start() {
        myAnim = GetComponent<Animator>();
    }

    void Update() {

        if (myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f && !myAnim.IsInTransition(0)) {
            Destroy(gameObject);
        }
    }
}