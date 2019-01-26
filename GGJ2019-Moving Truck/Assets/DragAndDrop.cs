using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public Transform SelectedObject;
    public float GridSpacing = 1f;
    public int LeftLimit = -1;
    public int RightLimit = 1;
    
    void OnMouseDrag()
    {
        if (SelectedObject != null)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);

            int gridNum = (int)(curPosition.x / GridSpacing);
            if (gridNum < LeftLimit)
                gridNum = LeftLimit;
            else if (gridNum > RightLimit)
                gridNum = RightLimit;
            curPosition = new Vector3(gridNum * GridSpacing, curPosition.y, 0f);

            SelectedObject.position = curPosition;
        }
    }

    void OnMouseUp()
    {
        if (SelectedObject != null)
        {
            SelectedObject.SendMessage("Fall");
            SelectedObject = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(RightLimit + Mathf.Abs(LeftLimit) + 1, 8f, 0.1f));
    }
}
