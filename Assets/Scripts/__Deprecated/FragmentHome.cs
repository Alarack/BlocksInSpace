using UnityEngine;
using System.Collections;

public class FragmentHome : MonoBehaviour {



    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Fragment")
        {
            Destroy(other.gameObject);
        }
    }
}
