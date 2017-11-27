using UnityEngine;
using System.Collections;

public class ParticleWrapFix : MonoBehaviour {

    private ParticleSystem myParticles;
    private ScreenWrap screenWrap;

    void Start() {
        myParticles = GetComponent<ParticleSystem>();
        screenWrap = GetComponentInParent<ScreenWrap>();
    }

    void Update() {

        if (screenWrap.isWrappingX || screenWrap.isWrappingY && myParticles.isPlaying) {
            myParticles.Stop();
        }
        else if (!screenWrap.isWrappingY && !screenWrap.isWrappingX && myParticles.isStopped) {
            myParticles.Play();
        }

        //if(!myRenderer.isVisible && myParticles.isPlaying)
        //{
        //    myParticles.Stop();
        //}
        //else if (myRenderer.isVisible && myParticles.isStopped)
        //{
        //    myParticles.Play();
        //}
    }
}