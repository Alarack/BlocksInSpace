using UnityEngine;
using System.Collections;

public class MultiShot : Projectile {

    public float speed;

    public GameObject subProjectile;
    [Space(10)]
    public Transform[] horizontalShots;
    public Transform[] verticalShots;

    private bool horizontalShot;
    //private Projectile[] subShots;

    protected override void Start() {
        base.Start();

        for (int i = 0; i < horizontalShots.Length; i++) {
            GameObject activeShot = Instantiate(subProjectile, horizontalShots[i].position, horizontalShots[i].rotation) as GameObject;
            Projectile shotScript = activeShot.GetComponent<Projectile>();

            activeShot.transform.parent = transform;

            if (shotScript.sprites.Length > 0)
                shotScript.autoSpriteColor = true;

            shotScript.playerShot = playerShot;
            shotScript.damage = damage;
            shotScript.gameObject.tag = gameObject.tag;
            shotScript.gameObject.layer = gameObject.layer;
            shotScript.parentColor = parentColor;
            shotScript.GetComponent<BasicMovement>().speed = speed;
        }
    }

    protected override void Update() {
        base.Update();
        if (transform.childCount <= 3)
            Destroy(gameObject);
    }
}