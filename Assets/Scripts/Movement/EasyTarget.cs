using UnityEngine;
using System.Collections;

public class EasyTarget : MonoBehaviour {

    public GameObject easyTarget;

    private BasicMovement myMoves;

    void Start() {
        myMoves = GetComponent<BasicMovement>();

    }

    void Update() {
        if (easyTarget != null)
            myMoves.target = easyTarget.transform.position;
    }
}