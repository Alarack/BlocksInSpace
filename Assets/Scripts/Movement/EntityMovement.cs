using UnityEngine;
using System.Collections;

public abstract class EntityMovement : MonoBehaviour {

    [Header("Basic Movement Stats")]
    public float speed;
    public bool modifySpeedAfterTime;
    public float multiplier;
    public float timeDelay;

    protected Rigidbody2D myBody;

    protected virtual void Start() {
        myBody = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate() {
        if (speed != 0f)
            Move();
    }

    protected abstract void Move();
}
