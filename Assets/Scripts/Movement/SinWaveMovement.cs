using UnityEngine;
using System.Collections;

public class SinWaveMovement : BasicMovement {

    [Header("SinWave Info")]
    public float frequency = 20.0f;  // Speed of sine movement
    public float magnitude = 0.5f;   // Size of sine movement
    [Space(10)]
    public bool swapDir;
    public bool inheritBaseMove;
    public bool rotate;
    public float angle;
    public float rotateSpeed;
    public Direction facingEnum = Direction.Up;

    private static int dir = 1;

    protected Vector3 facing;
    protected Vector3 axis;
    protected Vector3 pos;
    protected float timer;
    protected ScreenWrap screenWrap;

    protected override void Start() {
        myBody = GetComponent<Rigidbody2D>();
        screenWrap = GetComponent<ScreenWrap>();

        pos = transform.position;
        //pos = new Vector2(transform.position.x, transform.position.y);

        if (swapDir) {
            dir = EnumList.Alternator(dir);

            if (dir == 1) {
                axis = -transform.right;
                angle *= -1;
            }

            else {
                axis = transform.right;
            }
                
        }
        else {
            axis = transform.right;
        }

        switch (facingEnum) {

            case Direction.Up:
                facing = transform.up;

                break;

            case Direction.Down:
                facing = -transform.up;

                break;

        }



    }

    protected virtual void Update() {
        timer += Time.deltaTime;

        if (screenWrap != null && timer > 1f) {
            Vector2 myViewportPosition = Camera.main.WorldToViewportPoint(transform.position);

            if (myViewportPosition.x > 0.95f || myViewportPosition.x < 0.05f || myViewportPosition.y > 0.95f || myViewportPosition.y < 0.05f) {
                //speed *= -1f;
                transform.rotation = TargetUtils.SmoothRotation(Vector2.zero, transform, 10f);
            }

            if (!screenWrap.IsBeingRendered()) {
                transform.position = Vector2.zero;
            }
        }
    }

    protected override void Move() {
        pos += facing * Time.deltaTime * speed;

        if (inheritBaseMove) {
            base.Move();
            myBody.velocity = axis * Mathf.Sin(timer * frequency) * magnitude;
        }
        else {
            transform.position = pos + axis * Mathf.Sin(timer * frequency) * magnitude;
        }

        if (rotate) {
            float phase = Mathf.Sin(timer * rotateSpeed);
            transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, phase * angle));

        }
    }
}