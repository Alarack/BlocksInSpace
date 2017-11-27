using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LookAtTarget : MonoBehaviour {

    public bool activateLookAt = true;
    public bool targetNearest = true;
    public bool autoMask = true;
    public LayerMask whatIsTarget;
    public float targetRadius;
    public float rotateSpeed;
    public float inaccuracy;
    public bool updateError = false;
    [HideInInspector]
    public Vector3 target;
    [HideInInspector]
    public float error;
    //[HideInInspector]
    public List<Transform> alltargets = new List<Transform>();

    private Transform myWeaponMount;


    void Start() {
        if(autoMask)
            whatIsTarget = TargetUtils.SetMask(gameObject.layer, EnumList.MaskProperties.LookMask);

        AquireTarget();
        error = Random.Range(-inaccuracy, inaccuracy);

        myWeaponMount = transform.parent;

        if (myWeaponMount != null && myWeaponMount.gameObject.name != "PrimeWeaponMount") { //TODO: Rename this or add AuxWeaponMount
            myWeaponMount = null;
        }
        if(GetComponent<Projectile>() == null)
            transform.localRotation = Quaternion.identity;
    }

    void Update() {
        if (activateLookAt) {
            if (updateError) {
                error = Random.Range(-inaccuracy, inaccuracy);
            }

            if (targetNearest) {
                AquireTarget();

                if (target != Vector3.zero) {
                    Aim();
                }
                else {
                    AquireTarget();
                }
            }
            else {
                if (target != Vector3.zero) {
                    Aim();
                }
            }
        }
    }//End of Update

    void Aim() {
        if(myWeaponMount != null) {
            myWeaponMount.rotation = TargetUtils.SmoothRotation(target, myWeaponMount, rotateSpeed, error);
        }
        else {
            transform.rotation = TargetUtils.SmoothRotation(target, transform, rotateSpeed, error);
        }
    }

    public void AquireTarget(bool onlyUpdateNear = false) {
        if(!onlyUpdateNear)
            alltargets = TargetUtils.FindAllTargets(transform, targetRadius, whatIsTarget);

        target = TargetUtils.FindNearestTarget(transform.position, alltargets);
    }
}