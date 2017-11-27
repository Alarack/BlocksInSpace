using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SimpleTutorial : MonoBehaviour {

    public enum TutType {
        Movement,
        Collection,
        Shooting,
        Enemy
    }

    public TutType tutType;
    public GameObject destructionPrefab;
    public GameObject nextTut;
    public GameObject tutEnemy;
    public GameObject reticle;
    public GameObject tutMod;
    public float transitionDelay;
    [Space(10)]
    public bool shootVisual;

    private bool isTransitioning;
    private GameObject tutModActive;


    void Start() {
        if (tutType == TutType.Enemy) {
            GameManager.SetUIText("Enemy Incomming!");
        }

        if (tutType == TutType.Collection) {
            GameManager.SetUIText("Enemies Drop Modules");
            GameObject activeTutMod = Instantiate(tutMod, transform.position, transform.rotation) as GameObject;
            //activeTutMod.GetComponentInChildren<Reticle>().myTarget = activeTutMod.gameObject;
            tutModActive = activeTutMod;
            reticle.GetComponent<Reticle>().myTarget = activeTutMod.gameObject;
        }
    }

    void Update() {
        if (tutType == TutType.Enemy && tutEnemy != null && !isTransitioning) {
            if (tutEnemy.GetComponent<Health>().isDead) {
                if (reticle != null)
                    Destroy(reticle);
                StartCoroutine(TransitionTut());
            }
        }

        if (tutType == TutType.Collection && tutModActive == null && !isTransitioning) {
            if (reticle != null)
                Destroy(reticle);
            StartCoroutine(TransitionTut());
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == 8 && !isTransitioning && tutType == TutType.Shooting) {
            StartCoroutine(TransitionTut());
        }
    }

    public IEnumerator TransitionTut() {
        isTransitioning = true;
        GameManager.SetUIText(TutorialMessage(tutType));

        if (destructionPrefab != null) {
            GameObject boom = Instantiate(destructionPrefab, transform.position, Quaternion.identity) as GameObject;
            Destroy(boom, 1.5f);
        }

        if (shootVisual) {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }

        yield return new WaitForSeconds(transitionDelay);

        if (nextTut != null) {
            Instantiate(nextTut, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else {
            StartCoroutine(EndTutorial());
        }
    }

    string TutorialMessage(TutType type) {
        string message = " ";

        switch (tutType) {
            case TutType.Movement:
                message = "Nice Moves!";

                break;

            case TutType.Shooting:
                message = "Bullseye!";

                break;

            case TutType.Collection:

                break;

            case TutType.Enemy:
                message = "That'll teach those uppity blocks!";
                break;
        }

        return message;
    }

    IEnumerator EndTutorial() {
        GameManager.SetUIText("You're SO good at this game!");
        yield return new WaitForSeconds(3f);
        //Application.LoadLevel("MainMenu");
        SceneManager.LoadScene("MainMenu");
    }
}