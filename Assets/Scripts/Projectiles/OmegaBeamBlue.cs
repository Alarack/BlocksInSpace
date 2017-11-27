using UnityEngine;
using System.Collections;

public class OmegaBeamBlue : LingeringProjectile {

    //public float duration;

    private Animator myAnim;
    private bool isShrinking;
    private float timer;

    protected override void Start() {
        base.Start();
        //transform.SetParent(parentWeapon.transform, false);
        //transform.localPosition = Vector2.zero;
        transform.parent = parentWeapon.transform;
        transform.localScale =(parentWeapon.transform.parent.localScale.normalized);

        //if (playerShot)
        //    transform.parent = FindObjectOfType<EntityPlayer>().transform;
        //else
        //    Debug.Log("Find my weapon");

        myAnim = GetComponent<Animator>();
        myAnim.SetTrigger("Grow");

        //CameraController.ShakeCam(.05f, duration);
    }

    protected override void Update() {
        base.Update();
        CameraController.ShakeCam(.05f, 0.5f);

        timer += Time.deltaTime;

        if(timer >= life - 2f && !isShrinking) {
            isShrinking = true;
            ShrinkBeam();
        }
    }

    void ShrinkBeam() {
        myAnim.SetTrigger("Shrink");
    }

    //protected override void FixedUpdate() {
    //    base.FixedUpdate();
    //    duration -= Time.deltaTime;
    //    if (duration <= 0)
    //        myAnim.SetTrigger("Shrink");
    //}

    //void OnTriggerStay2D(Collider2D other) {
    //    if (other.tag == "Player" || other.tag == "Enemy") {
    //        if (other.GetComponent<Health>() != null) {
    //            StartCoroutine(DoT(other.GetComponent<Health>(), damage));
    //        }
    //    }
    //}

    //IEnumerator DoT(Health target, float dmg) {
    //    target.GetComponent<Health>().AdjustHealth(dmg);
    //    yield return new WaitForSeconds(damageInterval);
    //}

    protected override void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == 9) {
            Destroy(other.gameObject);
        }
    }
}