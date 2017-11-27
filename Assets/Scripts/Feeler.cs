using UnityEngine;
using System.Collections;

public class Feeler : MonoBehaviour {

    public ColorModule owner;
    public LayerMask modTag;

    private ColorModule nearColors;

    public ColorModule CheckArea() {
        Collider2D nearColider = Physics2D.OverlapCircle(transform.position, .01f, modTag);
        if (nearColider != null)
            nearColors = nearColider.GetComponent<ColorModule>();

        if (nearColors != null) {
            nearColors.AddNeighbor(owner);
            return nearColors;
        }
        else
            return null;
    }

    //public Vector2 CheckEmptySpaces() {
    //    Collider2D nearColider = Physics2D.OverlapCircle(transform.position, .01f, modTag);

    //    if (nearColider == null)
    //        return transform.position;
    //    else
    //        return Vector2.zero;
    //}
}