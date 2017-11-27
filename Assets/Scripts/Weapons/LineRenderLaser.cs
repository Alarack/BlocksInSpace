using UnityEngine;
using System.Collections;

public class LineRenderLaser : MonoBehaviour {

    public Transform laserHit;
    public LayerMask mask;

    private LineRenderer myLineRenderer;

    void Start() {
        myLineRenderer = GetComponent<LineRenderer>();
        myLineRenderer.enabled = true;
        myLineRenderer.useWorldSpace = true;
    }

    void Update() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 25f, mask);
        laserHit.position = hit.point;
        myLineRenderer.SetPosition(0, transform.position);
        myLineRenderer.SetPosition(1, laserHit.position);

    }
}
