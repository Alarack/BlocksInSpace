using UnityEngine;
using System.Collections;

public class OscillateMovement : SinWaveMovement {

    //private Entity parent;

    public bool wonkyTurning;

    private Vector3 _startPostion;

    protected override void Start() {
        base.Start();
        //parent = GetComponentInParent<Entity>();

        _startPostion = transform.localPosition;
    }

    protected override void Update() {
        base.Update();
    }

    protected override void Move() {
        // base.Move();

        pos = transform.up * Time.deltaTime * speed;
        //axis = transform.up;

        if (wonkyTurning) {
            transform.localPosition = _startPostion + pos * Mathf.Sin(timer * frequency) * magnitude;
        }
        else {
            transform.localPosition = _startPostion * Mathf.Sin(timer * frequency) * magnitude;
        }
    }
}