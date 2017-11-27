using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class TargetUtils {

    public static Vector3 FindNearestTarget(Vector3 myPos, List<Transform> targets) {
        Vector3 nearestTarget = new Vector3();

        foreach (Transform target in targets) {
            if(target != null) {
                if (FindDistance(target.position, myPos) == CalcDistances(targets, myPos).Min()) {
                    nearestTarget = target.position;
                    break;
                }
            }
        }

        if (nearestTarget != Vector3.zero)
            return nearestTarget;
        else
            return Vector3.zero;
    }

    public static List<Transform> FindAllTargets(Transform myTransform, float targetRadius, LayerMask mask) {
        Collider2D[] nearTargets = Physics2D.OverlapCircleAll(myTransform.position, targetRadius, mask);
        List<Transform> targetPositions = new List<Transform>();

        foreach (Collider2D target in nearTargets) {
            targetPositions.Add(target.transform);
        }

        return targetPositions;
    }

    public static List<float> CalcDistances(List<Transform> calcTargets, Vector3 myPos) {
        List<float> tempDistances = new List<float>();

        foreach (Transform target in calcTargets) {
            if(target != null) {
                float distance = FindDistance(target.position, myPos);
                tempDistances.Add(distance);
            }
        }
        return tempDistances;
    }

    public static float FindDistance(Vector3 targetPos, Vector3 myPos) {
        return Vector3.Distance(targetPos, myPos);
    }

    public static Quaternion SmoothRotation(Vector3 targetPos, Transform myTransform, float rotateSpeed, float error = 0f) {
        Quaternion newRotation = new Quaternion();

        Vector2 direction = (targetPos - myTransform.position);

        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90f + error;

        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        newRotation = Quaternion.Slerp(myTransform.rotation, q, Time.deltaTime * rotateSpeed);

        return newRotation;
    }

    public static LayerMask SetMask(int currentlayer, EnumList.MaskProperties whatAmI) {

        LayerMask newmask = new LayerMask();

        int playerShot = 8;
        int enemyShot = 9;
        int player = 10;
        int enemy = 11;
        int playerShield = 14;
        int enemyShield = 15;

        //int playerShotMask = 1 << playerShot;
        //int enemyShotMask = 1 << enemyShot;
        int playerMask = 1 << player;
        int enemyMask = 1 << enemy;
        int playerShieldMask = 1 << playerShield;
        int enemyShieldMask = 1 << enemyShield;

        int playerAndShield = playerMask | playerShieldMask;
        int enemyAndShiels = enemyMask | enemyShieldMask;


        if(currentlayer == player && whatAmI == EnumList.MaskProperties.LaserMask) {
            newmask = enemyAndShiels;
        }

        if(currentlayer == enemy && whatAmI == EnumList.MaskProperties.LaserMask) {
            newmask = playerAndShield;
        }

        if(currentlayer == enemyShot || currentlayer == enemy && whatAmI == EnumList.MaskProperties.LookMask) {
            newmask = playerMask;
        }

        if (currentlayer == playerShot || currentlayer == player && whatAmI == EnumList.MaskProperties.LookMask) {
            newmask = enemyMask;
        }

        if (currentlayer == playerShield && whatAmI == EnumList.MaskProperties.ShieldMask) {
            newmask = playerShieldMask;
        }

        if (currentlayer == enemyShield && whatAmI == EnumList.MaskProperties.ShieldMask) {
            newmask = enemyShieldMask;
        }

        //if(currentlayer == enemyShot && whatAmI == EnumList.MaskProperties.ShieldMask) {
        //    newmask = enemyShieldMask;
        //}

        return newmask;
    }

    public static int SetLayer(string tag, string type) {
        int layer = 0;

        if(tag == "Player" && type == "Shield") {
            layer = 14;
        }

        if (tag == "Player" && type == "Projectile") {
            layer = 8;
        }

        if (tag == "Enemy" && type == "Shield") {
            layer = 15;
        }

        if (tag == "Enemy" && type == "Projectile") {
            layer = 9;
        }

        return layer;
    }
}