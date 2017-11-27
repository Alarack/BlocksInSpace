using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Reticle : MonoBehaviour {

    public GameObject chevHolder;
    public float chevRotateSpeed;
    [Space(10)]
    public Color32 lockOnColor;
    public float lockDistance;
    public float unlockDistance;
    [Space(10)]
    public float lockSpeed;
    public List<GameObject> chevs = new List<GameObject>();
    public GameObject myTarget;
    [Space(10)]
    public bool lockedOn;
    //TODO: I probably broke the tutorial.
    //public bool unlock;
    //public bool targetAquired;

    private Color32[] myColors;
    private SpriteRenderer[] myChevSprites;
    private bool[] chevLocks;

    void Start() {
        myChevSprites = GetComponentsInChildren<SpriteRenderer>();
        myColors = new Color32[myChevSprites.Length];

        for (int i = 0; i < myChevSprites.Length; i++) {
            myColors[i] = myChevSprites[i].color;
        }
        chevLocks = new bool[chevs.Count];

        if(chevHolder.GetComponent<AutoSpin>() == null) {
            chevHolder.AddComponent<AutoSpin>();
            AutoSpin tmpspin = chevHolder.GetComponent<AutoSpin>();
            tmpspin.spinTransform = true;
        }
        else {
            chevHolder.GetComponent<AutoSpin>().rotateSpeed = chevRotateSpeed;
        }
    }

    void Update() {
        if (myTarget != null) {
            GetComponent<BasicMovement>().target = myTarget.transform.position;
        }
    }

    public void LockOnTarget() {
        foreach (Transform child in chevHolder.transform) {
            if (child.name == "Chev") {
                float distance = Vector2.Distance(child.position, chevHolder.transform.position);
                if (distance >= lockDistance) {
                    child.position = Vector3.MoveTowards(child.position, chevHolder.transform.position, lockSpeed * Time.deltaTime);
                }
                else {
                    child.GetComponent<SpriteRenderer>().color = lockOnColor;
                    lockedOn = true;
                }
            }
        }

        for (int i = 0; i < chevLocks.Length; i++) {
            if (chevs[i].GetComponent<SpriteRenderer>().color == lockOnColor) {
                chevLocks[i] = true;
            }
        }

        if (EnumList.CheckBoolArray(chevLocks)) {
            chevHolder.GetComponent<AutoSpin>().rotateSpeed = chevRotateSpeed * 2f;
        }
    }

    public void UnlockOnTarget() {
        if(chevHolder != null)
            chevHolder.GetComponent<AutoSpin>().rotateSpeed = chevRotateSpeed;

        foreach (Transform child in chevHolder.transform) {
            if (child.name == "Chev") {
                child.GetComponent<SpriteRenderer>().color = Color.white;

                float distance = Vector2.Distance(child.position, chevHolder.transform.position);
                if (distance < unlockDistance) {
                    child.position = Vector3.MoveTowards(child.position, chevHolder.transform.position, -lockSpeed * Time.deltaTime * 2f);
                }
                else if (distance >= unlockDistance) {
                    lockedOn = false;
                }
            }
        }

        for (int i = 0; i < chevLocks.Length; i++) {
            if (chevs[i].GetComponent<SpriteRenderer>().color == Color.white)
                chevLocks[i] = false;
        }

        if (!EnumList.CheckBoolArray(chevLocks)) {
            //chevHolder.GetComponent<Rigidbody2D>().angularVelocity = chevRotateSpeed / 3f;
            //unlock = false;
        }
    }
}