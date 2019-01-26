using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemClick : MonoBehaviour
{
    public int Width = 1;


    public void OnMouseDown()
    {
        DragAndDrop drag = GameObject.Find("dragging area").GetComponent<DragAndDrop>();

        drag.SelectedObject = transform;
        drag.ObjectWidth = Width;
        drag.OriginalPos = transform.position;

        GetComponent<BoxCollider2D>().enabled = false;
    }
}
