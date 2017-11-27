using UnityEngine;
using System.Collections;

public class SpinnerMovement : BasicMovement {

    [Header("Spinner Movement Info")]
    public float spinAcceleration;
    public float maxSpin;
    public float moveDuration;
    public float rotateSpeed;
    public float rotatDuration;
    public float spinDelay;
    [Space(10)]
    public bool targetAquired = false;
    [Space(10)]
    public Color targetColor;

    private float moveTimer;
    private SpriteRenderer mySprite;
    private EntityEnemy self;
    private Vector2 curDirection;

    public State mobState;

    public enum State {
        Seeking,
        Rotating,
        SpinningUp,
        SpinningDown,
        Attack
    }

    protected override void Start() {
        base.Start();
        mySprite = GetComponent<SpriteRenderer>();
        self = GetComponent<EntityEnemy>();
        self.autoFire = false;
    }

    protected override void Move() {
        base.Move();

        switch (mobState) {
            case State.Seeking:
                direction = Direction.Up;
                Seeking();

                break;

            case State.Rotating:
                direction = Direction.Still;
                StartCoroutine(Rotate());

                break;

            case State.SpinningUp:
                direction = Direction.Still;
                StartCoroutine(SpinUp());

                break;

            case State.Attack:
                direction = Direction.Still;
                StartCoroutine(Attack());

                break;

            case State.SpinningDown:
                direction = Direction.Still;
                SpinDown();

                break;
        }
    }

    public IEnumerator Rotate() {
        if (!targetAquired) {
            curDirection = Random.insideUnitCircle;
            targetAquired = true;
        }

        float angle = (Mathf.Atan2(curDirection.y, curDirection.x) * Mathf.Rad2Deg) - 90f;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotateSpeed);

        yield return new WaitForSeconds(rotatDuration);

        mobState = State.Seeking;
    }

    public void Seeking() {
        moveTimer += Time.deltaTime;

        if (moveTimer >= moveDuration) {
            targetAquired = false;
            mobState = State.SpinningUp;
        }
    }

    public IEnumerator SpinUp() {
        moveTimer = 0f;
        if (myBody.angularVelocity < maxSpin)
            myBody.angularVelocity += spinAcceleration;

        mySprite.color = Color.Lerp(mySprite.color, targetColor, 0.06f);

        if (myBody.angularVelocity >= maxSpin) {
            yield return new WaitForSeconds(0f);
            mobState = State.Attack;
        }
    }

    public IEnumerator Attack() {
        self.autoFire = true;

        yield return new WaitForSeconds(spinDelay);
        mobState = State.SpinningDown;
    }

    public void SpinDown() {
        self.autoFire = false;
        if (myBody.angularVelocity > 0)
            myBody.angularVelocity += -spinAcceleration / 3;

        if (myBody.angularVelocity <= 0)
            myBody.angularVelocity = 0;

        mySprite.color = Color.Lerp(mySprite.color, Color.white, 0.08f);
        //&& mySprite.color == Color.white
        if (myBody.angularVelocity == 0f) {
            mobState = State.Rotating;
        }
    }
}