using UnityEngine;
using System.Collections;

public class Missile : Projectile {

    private float startAngle;
    private bool tracked = false;

    protected override void Start() {
        base.Start();

        int leftOrRight = Random.Range(0, 2);

        if (leftOrRight == 0)
            startAngle = 90f;
        else
            startAngle = -90f;

        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, startAngle);
        transform.position = new Vector2(transform.position.x, transform.position.y - .6f);
        StartCoroutine(MissileTracking());

        if (!playerShot)
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y * -1, transform.localScale.z);
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();

        if (tracked) {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.x, transform.rotation.y, -startAngle * 0.5f), 0.2f);
            //speed += 25f;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player" || other.tag == "Enemy") {
            if (other.GetComponent<Rigidbody2D>() != null)
                other.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-500f, 500f);
        }
        base.OnTriggerEnter2D(other);
    }

    IEnumerator MissileTracking() {
        yield return new WaitForSeconds(2f);
        tracked = true;
        myBody.velocity = Vector2.zero;
        GetComponent<EntityMovement>().speed *= 3;
    }
}
