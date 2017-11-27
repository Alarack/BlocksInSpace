using UnityEngine;
using System.Collections;

public class AmmoRegen : Upgrade {

    [Header("Ammo Regen Stats")]
    public int amount;

    protected float initialInterval;

    protected override void Start() {
        base.Start();

        initialInterval = effectInterval;
    }

    protected override void ApplyIntervalEffect() {
        GameManager.missiles += amount;
        //TODO: add some visual here.
    }

    protected override Upgrade GetTargetStatus<T>(T target) {
        return target.GetComponent<AmmoRegen>();
    }

    public override void StackEffect() {
        if (effectInterval > 3f)
            effectInterval -= 1f;
    }

    protected override void AddToTarget<T>(T target) {
        target.gameObject.AddComponent<AmmoRegen>();
        AmmoRegen activeAmmo = target.gameObject.GetComponent<AmmoRegen>();
        activeAmmo.effectInterval = effectInterval;
        activeAmmo.amount = amount;
        activeAmmo.upgradeIcon = upgradeIcon;
    }
}