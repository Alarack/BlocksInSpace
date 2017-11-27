using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutoSeek : Projectile {

    private LookAtTarget lookScript;

    protected override void Start() {
        base.Start();

        lookScript = GetComponent<LookAtTarget>();
    }

    protected override void Update() {
        base.Update();

        if(Vector2.Distance(transform.position, lookScript.target) <= 1 && !Physics2D.OverlapCircle(transform.position, 0.2f, lookScript.whatIsTarget)){
            lookScript.AquireTarget(true);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other) {
        base.OnTriggerEnter2D(other);

        if (lookScript.alltargets.Contains(other.transform)) {
            lookScript.alltargets.Remove(other.transform);
            if (lookScript.alltargets.Count == 0) {
                CleanUp();
            }
            else {
                int rand = Random.Range(0, lookScript.alltargets.Count);
                if(lookScript.alltargets[rand] != null)
                    lookScript.target = lookScript.alltargets[rand].position;
            }
        }
    }
}