using UnityEngine;
using System.Collections;

public class TutVisualAid : MonoBehaviour {

    public enum TutType {
        Movement,
        Collection,
        Shooting
    }

    public TutType tutType;
    public string triggerTag;
    //public bool isComplete;
    public float arrowDelay;
    public GameObject arrow;

    private Reticle myReticle;
    private SimpleTutorial parentTut;
    private bool inPhase;
    private float arrowTimer;

    void Start() {

        myReticle = GetComponentInChildren<Reticle>();
        parentTut = FindObjectOfType<SimpleTutorial>();
    }

    void Update() {
        if (tutType == TutType.Collection) {
            arrowTimer += Time.deltaTime;

            if (arrowTimer >= arrowDelay && !inPhase) {
                StartCoroutine(SpawnArrow());
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == triggerTag) {
            switch (tutType) {
                case TutType.Movement:

                    if (!inPhase)
                        StartCoroutine(MoveTut());

                    break;

                case TutType.Shooting:

                    break;

                case TutType.Collection:

                    break;
            }
        }
    }

    public IEnumerator SpawnArrow() {
        inPhase = true;
        GameObject activeArrow = Instantiate(arrow, transform.position, transform.rotation) as GameObject;
        activeArrow.transform.parent = transform;
        GameManager.SetUIText("Collect These!");
        yield return new WaitForSeconds(1.5f);

    }

    public IEnumerator MoveTut() {
        inPhase = true;
        GetComponent<Collider2D>().enabled = false;
        myReticle.lockedOn = true;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(parentTut.TransitionTut());
        Destroy(transform.parent.gameObject, parentTut.transitionDelay + 0.1f);
    }
}