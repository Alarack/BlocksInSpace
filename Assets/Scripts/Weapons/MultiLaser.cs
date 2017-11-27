using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiLaser : Laser {

    [Header("MultiLaser Info")]
    public float rotateSpeed = 30f;
    public float maxZapDistance;
    public GameObject weaponPrefab;
    //public List<Transform> targets = new List<Transform>();
    //public List<GameObject> curWeapons = new List<GameObject>();

    public Dictionary<Transform, GameObject> targetLaserPair = new Dictionary<Transform, GameObject>();

    protected override void Start() {
        base.Start();
        mask = TargetUtils.SetMask(gameObject.layer, EnumList.MaskProperties.LookMask);
    }

    GameObject AddWeapon() {

        GameObject activeWeapon = Instantiate(weaponPrefab) as GameObject;
        EnumList.InitWeapon(parentEntity.gameObject, activeWeapon);
        activeWeapon.AddComponent<LookAtTarget>();
        LookAtTarget activeLook = activeWeapon.GetComponent<LookAtTarget>();

        activeLook.rotateSpeed = rotateSpeed;
        activeLook.targetRadius = maxZapDistance;
        activeLook.whatIsTarget = mask;
        activeLook.targetNearest = false;

        Laser laserScript = activeWeapon.GetComponent<Laser>();

        if (laserScript != null) {
            laserScript.damage = damage;
            laserScript.damageInterval = damageInterval;
            laserScript.autoFire = true;
        }
        return activeWeapon;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player") {
            targetLaserPair.Add(other.transform, AddWeapon());
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player") {
            Destroy(targetLaserPair[other.transform]);
            targetLaserPair.Remove(other.transform);
        }
    }

    protected override void Update() {
        base.Update();

        //for (int i = 0; i < targets.Count; i++) {
        //    if(targets[i] == null) {
        //        Destroy(curWeapons[i]);
        //    }

        //    if(targets[i] != null) {
        //        LookAtTarget activeLook = curWeapons[i].GetComponent<LookAtTarget>();
        //        activeLook.target = targets[i].position;
        //    }
        //}

        foreach(KeyValuePair<Transform, GameObject> entry in targetLaserPair) {

            if(entry.Key == null) {
                Destroy(entry.Value);
            }

            if(entry.Key != null) {
                LookAtTarget activeLook = entry.Value.GetComponent<LookAtTarget>();
                activeLook.target = entry.Key.position;
            }
        }
    }

    public override void Fire() {

    }

    //void OnTriggerStay2D(Collider2D other) {
    //    if (other.tag == "Player" || other.tag == "Enemy") {
    //        //Debug.DrawLine(transform.position, other.transform.position, Color.yellow);

    //        RaycastHit2D hit = Physics2D.Raycast(transform.position, (other.transform.position - transform.position), maxZapDistance, mask);
    //        //Debug.DrawRay(transform.position, (other.transform.position - transform.position), Color.green);

    //        if (hit.collider != null && hit.collider.GetComponent<Health>() != null) {


    //        }
    //    }
    //}
}