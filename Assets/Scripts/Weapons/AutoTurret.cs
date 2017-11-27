using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//[RequireComponent(typeof(LookAtTarget))]
public class AutoTurret : Weapon {

    private LookAtTarget lookScript;

    protected override void Start() {
        base.Start();
        gameObject.tag = transform.parent.gameObject.tag;
        color = GetComponentInParent<Entity>().unitColor;
        gameObject.tag = "Weapon";
        gameObject.layer = transform.parent.gameObject.layer;
        lookScript = GetComponent<LookAtTarget>();
        lookScript.whatIsTarget = TargetUtils.SetMask(gameObject.layer, EnumList.MaskProperties.LookMask);
    }

    protected override void Update() {
        base.Update();

        if (TargetUtils.FindNearestTarget(transform.position, lookScript.alltargets) != Vector3.zero)
            lookScript.target = TargetUtils.FindNearestTarget(transform.position, lookScript.alltargets);
        else
            lookScript.target = Vector3.zero;
    }

    public override void Fire() {
        if(lookScript !=null)
            lookScript.error = Random.Range(-lookScript.inaccuracy, lookScript.inaccuracy);

        base.Fire();
    }
}