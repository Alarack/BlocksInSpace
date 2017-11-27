using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class ScreenWrap : MonoBehaviour {

    public bool screenWrapY = false;
    public bool screenWrapX = false;
    [Space(10)]
    public bool waitForEntry;

    [HideInInspector]
    public bool isWrappingX;
    [HideInInspector]
    public bool isWrappingY;

    private Renderer myRenderer;
    private Transform myTransform;
    private Camera myCamera;
    private Vector2 myViewportPosition;
    private Vector2 myNewPosition;
    private float wrapDelay = 0.9f;

    void Start() {
        myRenderer = GetComponent<Renderer>();
        myTransform = transform;
        myCamera = Camera.main;
        myViewportPosition = Vector2.zero;
        isWrappingX = false;
        isWrappingY = false;
        myNewPosition = transform.position;
    }

    void Update() {
        if (waitForEntry) {
            if (IsBeingRendered()) {
                wrapDelay -= Time.deltaTime;

                if (wrapDelay <= 0) {
                    screenWrapX = true;
                    screenWrapY = true;
                    waitForEntry = false;
                }
            }
            else {
                screenWrapX = false;
                screenWrapY = false;
            }
        }
    }

    void LateUpdate() {
        Wrap();
    }

    public bool IsBeingRendered() {
        if (myRenderer != null && myRenderer.isVisible)
            return true;
        else
            return false;
    }

    void Wrap() {
        bool isVisable = IsBeingRendered();

        if (isVisable) {
            isWrappingX = false;
            isWrappingY = false;
        }

        myNewPosition = myTransform.position;
        myViewportPosition = myCamera.WorldToViewportPoint(myNewPosition);

        if (screenWrapX) {
            if (!isWrappingX) {
                if (myViewportPosition.x > 1) {
                    myNewPosition.x = myCamera.ViewportToWorldPoint(Vector2.zero).x;
                    isWrappingX = true;
                }
                else if (myViewportPosition.x < 0) {
                    myNewPosition.x = myCamera.ViewportToWorldPoint(Vector2.one).x;
                    isWrappingX = true;
                }
            }
        }

        if (screenWrapY) {
            if (!isWrappingY) {
                if (myViewportPosition.y > 1) {
                    myNewPosition.y = myCamera.ViewportToWorldPoint(Vector2.zero).y;
                    isWrappingY = true;
                }
                else if (myViewportPosition.y < 0) {
                    myNewPosition.y = myCamera.ViewportToWorldPoint(Vector2.one).y;
                    isWrappingY = true;
                }
            }
        }

        myTransform.position = myNewPosition;
    }
}