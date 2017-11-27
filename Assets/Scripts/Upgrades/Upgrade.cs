using UnityEngine;
using System.Collections;

public abstract class Upgrade : MonoBehaviour {

    [Header("Icon Stuff")]
    public GameObject upgradeIcon;
    public bool iconCreated;

    [Header("Timer")]
    public float effectInterval;

    protected float intervalCounter;
    protected EntityPlayer player;

    protected virtual void Start() {
        player = GetComponent<EntityPlayer>();
    }

    protected virtual void Update() {
        if (player != null) {
            UpdateTime();

            if (IsIntervalTime()) {
                ApplyIntervalEffect();
                ResetIntervalTime();
            }
        }
    }

    protected virtual void UpdateTime() {
        intervalCounter -= Time.deltaTime;
    }

    protected virtual void ResetIntervalTime() {
        intervalCounter = effectInterval;
    }

    protected virtual bool IsIntervalTime() {
        return intervalCounter <= 0;
    }

    public virtual void Apply<T>(T target) where T : MonoBehaviour {
        Upgrade upgrade = GetTargetStatus(target);
        if (upgrade != null) {
            upgrade.StackEffect();
        }
        else {
            AddToTarget(target);
        }
    }

    protected virtual void ApplyIntervalEffect() {

    }

    protected abstract Upgrade GetTargetStatus<T>(T target) where T : MonoBehaviour;
    protected abstract void AddToTarget<T>(T target) where T : MonoBehaviour;
    public abstract void StackEffect();
}