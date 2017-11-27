using UnityEngine;
using System.Collections;

public class SinWaveChildPulse : MonoBehaviour {

    public float magnitude;
    public float frequency;
    public float speed;
    [Space(10)]
    public bool spinChildren;
    public float childRotationSpeed = 150f;
    [Space]
    public bool wonkyTurning;
    [Header("Misc Switches")]
    public bool getColorModsOnly = true;

    public Transform[] allChildren;


    void Start() {
        allChildren = GetComponentsInChildren<Transform>();
        AssignChildMovement2();
    }

    //void AssignChildrenMovement() {
    //    foreach (ColorModule c in myChildren) {
    //        OscillateChild(c.gameObject);

    //        if (spinChildren) {
    //            //c.gameObject.AddComponent<AutoSpin>();
    //            //AutoSpin childSpin = c.GetComponent<AutoSpin>();
    //            //childSpin.rotateSpeed = childRotationSpeed;
    //            SpinChild(c.gameObject);
    //        }
    //    }
    //}

    void AssignChildMovement2() {
        foreach (Transform t in allChildren) {
            if (getColorModsOnly) {
                if (t.GetComponent<ColorModule>() != null && t != this.transform) {
                    OscillateChild(t.gameObject);
                    if(spinChildren)
                        SpinChild(t.gameObject);
                }
            }
            else {
                if (t != this.transform) {
                    OscillateChild(t.gameObject);
                    if (spinChildren)
                        SpinChild(t.gameObject);
                }
            }
        }
    }

    void SpinChild(GameObject child) {
        child.AddComponent<AutoSpin>();
        AutoSpin childSpin = child.GetComponent<AutoSpin>();
        childSpin.spinTransform = true;
        childSpin.rotateSpeed = childRotationSpeed;
    }

    void OscillateChild(GameObject child) {
        child.AddComponent<OscillateMovement>();
        child.AddComponent<Rigidbody2D>();

        Rigidbody2D childBody = child.GetComponent<Rigidbody2D>();
        childBody.gravityScale = 0f;
        childBody.isKinematic = true;

        OscillateMovement childMove = child.GetComponent<OscillateMovement>();

        childMove.magnitude = magnitude;
        childMove.frequency = frequency;
        childMove.speed = speed;

        if (wonkyTurning) {
            //childMove.magnitude *= 7.5f;
            childMove.wonkyTurning = true;
        }
    }
}