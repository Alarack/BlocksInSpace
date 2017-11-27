using UnityEngine;
using System.Collections;

public class BasicMovement : EntityMovement {

    public enum Direction {
        Up,
        Down,
        Left,
        Right,
        Still,
        Directed,
        None
    }

    public enum MoveType {
        SetVelocity,
        AddForce,
        TranslateDirectly
    }

    public Direction direction;
    public MoveType moveType = MoveType.SetVelocity;
    [Header("Move Only Once")]
    public bool moveOnce;
    public float moveOnceDuration;
    public bool spinOnStop;
    [Header("World Direction")]
    public bool worldUp;
    public float worldSpeed;

    [HideInInspector]
    public Vector3 target;

    private float moveOnceTimer;

    protected override void Start() {
        base.Start();

        if(GetComponent<Projectile>() != null && !GetComponent<Projectile>().playerShot) {
            worldUp = false;
        }

        if (modifySpeedAfterTime) {
            Invoke("ModifySpeed", timeDelay);
        }
    }


    protected override void Move() {
        if (moveOnce)
            moveOnceTimer += Time.deltaTime;

        if (moveOnce && spinOnStop && direction == Direction.Still)
            SpinOnMoveOnce();

        if (moveOnceTimer > moveOnceDuration)
            direction = Direction.Still;

        //Movement for setting the enemy's velocity
        if (moveType == MoveType.SetVelocity) {
            switch (direction) {
                case Direction.Up:
                    myBody.velocity = transform.up * speed * Time.deltaTime;

                    if (worldUp)
                        myBody.velocity += Vector2.up * worldSpeed * Time.deltaTime;

                    break;

                case Direction.Down:
                    myBody.velocity = -transform.up * speed * Time.deltaTime;

                    break;

                case Direction.Right:
                    myBody.velocity = transform.right * speed * Time.deltaTime;

                    break;

                case Direction.Left:
                    myBody.velocity = -transform.right * speed * Time.deltaTime;

                    break;

                case Direction.Directed:
                    myBody.velocity = ((target - transform.position) * speed * Time.deltaTime);

                    break;

                case Direction.Still:
                    myBody.velocity = Vector2.zero;

                    break;

                case Direction.None:


                    break;
            }
        }

        //Movement for adding force to an enemy
        if (moveType == MoveType.AddForce) {
            switch (direction) {
                case Direction.Up:
                    myBody.AddForce(transform.up * speed * Time.deltaTime);

                    break;

                case Direction.Down:
                    myBody.AddForce(-transform.up * speed * Time.deltaTime);

                    break;

                case Direction.Right:
                    myBody.AddForce(transform.right * speed * Time.deltaTime);

                    break;

                case Direction.Left:
                    myBody.AddForce(-transform.right * speed * Time.deltaTime);

                    break;


                case Direction.Directed:
                    myBody.AddForce((target - transform.position) * speed * Time.deltaTime);

                    break;

                case Direction.Still:
                    myBody.velocity = Vector2.zero;

                    break;

                case Direction.None:


                    break;
            }
        }

        //Movement that avoids using a RigidBody by translating directly
        if (moveType == MoveType.TranslateDirectly) {
            switch (direction) {
                case Direction.Up:
                    transform.Translate(transform.up * speed * Time.deltaTime);

                    break;

                case Direction.Down:
                    transform.Translate(-transform.up * speed * Time.deltaTime);

                    break;

                case Direction.Right:
                    transform.Translate(transform.right * speed * Time.deltaTime);

                    break;

                case Direction.Left:
                    transform.Translate(-transform.right * speed * Time.deltaTime);

                    break;

                case Direction.Directed:
                    transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

                    break;

                case Direction.Still:
                    //myBody.velocity = Vector2.zero;

                    break;

                case Direction.None:


                    break;
            }
        }
    }

    void SpinOnMoveOnce() {
        gameObject.AddComponent<AutoSpin>();

        AutoSpin mySpin = GetComponent<AutoSpin>();

        mySpin.rotateSpeed = 250f;
        mySpin.spinTransform = true;

    }

    public virtual void ModifySpeed() {
        speed *= multiplier;
    }


}