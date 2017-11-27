using UnityEngine;
using System.Collections;

public class TeleporterMovement : BasicMovement {

    public enum State {
        FadeIn,
        FadeOut,
        Attack,
        Teleport
    }

    [Header("Teleporter Movement Info")]
    public State mobState;
    public float attackDuration;
    [Space(10)]
    public Color fadeInColor;
    public Color fadeOutColor;

    private SpriteRenderer mySprite;
    private CircleCollider2D myCollider;
    private float attackTimer;
    private EntityEnemy self;
    private Animator myAnim;

    protected override void Start() {
        base.Start();

        mySprite = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<CircleCollider2D>();
        self = GetComponent<EntityEnemy>();
        myAnim = GetComponent<Animator>();
        self.autoFire = false;
        myCollider.enabled = false;
    }

    protected override void Move() {
        switch (mobState) {
            case State.FadeIn:
                FadeIn();

                break;

            case State.FadeOut:
                FadeOut();

                break;

            case State.Teleport:
                Teleport();

                break;

            case State.Attack:
                Attack();

                break;
        }
    }

    void FadeIn() {
        myAnim.SetBool("FadeInB", true);
        if (mySprite.color.a >= fadeInColor.a) {
            myAnim.SetBool("FadeInB", false);
            attackTimer = 0f;
            mobState = State.Attack;
        }
    }

    void Attack() {
        myCollider.enabled = true;
        self.autoFire = true;
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackDuration) {
            self.autoFire = false;
            mobState = State.FadeOut;
        }
    }

    void FadeOut() {
        myAnim.SetBool("FadeOutB", true);
        myCollider.enabled = false;

        if (mySprite.color.a <= fadeOutColor.a) {
            myAnim.SetBool("FadeOutB", false);
            mobState = State.Teleport;
        }
    }

    void Teleport() {
        Vector2 newPos = Random.insideUnitCircle * speed;
        transform.position = newPos;
        mobState = State.FadeIn;
    }
}