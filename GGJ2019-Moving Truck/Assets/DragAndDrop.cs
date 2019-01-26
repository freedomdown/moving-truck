using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public Transform SelectedObject;//the object itself
    public int ObjectWidth = 1;//the grid width of the object
    public float GridSpacing = 1f;//how wide is a grid space
    public int LeftLimit = -1;//x coord left most side of grid
    public int RightLimit = 1;//y coord right most side of grid

    public bool SafeToDrop = false;//is it safe to drop an item
    public bool SafeToTrash = false;
    
    void OnMouseDrag()
    {
        if (SelectedObject != null)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
            curPosition = new Vector3(curPosition.x, curPosition.y, 0f);

            if (SafeToDrop)
            {
                int gridNum = (int)(curPosition.x / GridSpacing);
                switch (ObjectWidth)
                {
                    case 1:
                        if (gridNum < LeftLimit)
                            gridNum = LeftLimit;
                        else if (gridNum > RightLimit)
                            gridNum = RightLimit;
                        break;
                    case 2:
                        if (gridNum < LeftLimit)
                            gridNum = LeftLimit;
                        else if (gridNum > RightLimit - 1)
                            gridNum = RightLimit - 1;
                        break;
                    case 3:
                        if (gridNum < LeftLimit)
                            gridNum = LeftLimit;
                        else if (gridNum > RightLimit - 2)
                            gridNum = RightLimit - 2;
                        break;
                }
                curPosition = new Vector3(gridNum * GridSpacing, 3f, 0f);
            }

            SelectedObject.position = curPosition;
        }
    }

    void OnMouseUp()
    {
        if (SafeToDrop)
        {
            if (SelectedObject != null)
            {
                SelectedObject.SendMessage("Fall");//tell object to fall
                SelectedObject = null;//is no longer selected
            }
        }
        else if (SafeToTrash)
        {
            if (SelectedObject != null)
            {
                GameObject.Destroy(SelectedObject);
            }
        }
        else
        {
            //do something other than drop it
            //Back to selection thing?
            if (SelectedObject != null)
                SelectedObject.position = new Vector3(4f, 4f, 0f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(RightLimit + Mathf.Abs(LeftLimit) + 1, 8f, 0.1f));
    }
}
