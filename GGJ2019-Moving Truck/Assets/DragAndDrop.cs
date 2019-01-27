using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    [Header("Selected Object")]
    public Transform SelectedObject;//the object itself
    public int ObjectWidth = 1;//the grid width of the object
    public Vector3 OriginalPos;//where it came from originally
    public ItemQueue itemQueue;

    [Header("Grid Settings")]
    public float GridSpacing = 1f;//how wide is a grid space
    public int LeftLimit = -1;//x coord left most side of grid
    public int RightLimit = 1;//y coord right most side of grid

    [Header("Drop States")]
    public bool SafeToDrop = false;//is it safe to drop an item
    public bool SafeToTrash = false;//is it safe to drop it in the trash

    [Header("Sounds")]
    public AudioSource dropSnd;
    public AudioSource TrashSnd;

    
    //public void OnMouseDrag()
    //{
    //    if (SelectedObject != null)
    //    {
    //        //get mouse position on screen and turn it into world coords
    //        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
    //        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
    //        curPosition = new Vector3(curPosition.x, curPosition.y, 0f);

    //        if (SafeToDrop)//if over the truck drop point
    //        {
    //            //then align to grid
    //            int gridNum = (int)(curPosition.x / GridSpacing);
    //            switch (ObjectWidth)
    //            {
    //                case 1:
    //                    if (gridNum < LeftLimit)
    //                        gridNum = LeftLimit;
    //                    else if (gridNum > RightLimit)
    //                        gridNum = RightLimit;
    //                    break;
    //                case 2:
    //                    if (gridNum < LeftLimit)
    //                        gridNum = LeftLimit;
    //                    else if (gridNum > RightLimit - 1)
    //                        gridNum = RightLimit - 1;
    //                    break;
    //                case 3:
    //                    if (gridNum < LeftLimit)
    //                        gridNum = LeftLimit;
    //                    else if (gridNum > RightLimit - 2)
    //                        gridNum = RightLimit - 2;
    //                    break;
    //            }
    //            curPosition = new Vector3(gridNum * GridSpacing, 3f, 0f);
    //        }

    //        SelectedObject.position = curPosition;
    //    }
    //}

    public void OnMouseOver()
    {
        if (SelectedObject != null)
        {
            //get mouse position on screen and turn it into world coords
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
            curPosition = new Vector3(curPosition.x, curPosition.y, 0f);

            if (SafeToDrop)//if over the truck drop point
            {
                //then align to grid
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

            if (!Input.GetMouseButton(0))
            {
                OnMouseUp();//make sure it is actually called
            }
        }
    }

    public void OnMouseUp() {
        if (SelectedObject != null) {
            bool gravityActive = SelectedObject.GetComponent<GridMovement>().GravityActive;
            if (SafeToDrop && !gravityActive)//actually drop item in truck
            {
                SelectedObject.SendMessage("Fall");//tell object to fall
                ItemScore score = SelectedObject.GetComponentInChildren(typeof(ItemScore)) as ItemScore;
                score.state = ItemScore.State.OnTruck;
                itemQueue.RemoveFromQueue(SelectedObject);
                SelectedObject = null;//is no longer selected
                dropSnd.Play();
            }
            else if (SafeToTrash && !gravityActive)//drop item in trash
            {
                itemQueue.RemoveFromQueue(SelectedObject);
                TrashSnd.Play();
                SelectedObject.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
                Destroy(SelectedObject.gameObject, TrashSnd.clip.length);
                SelectedObject = null;//is no longer selected
            }
            else//item was dropped somewhere else on screen
            {
                //do something other than drop it
                //Back to selection thing?
                SelectedObject.position = OriginalPos;
                SelectedObject.GetComponent<BoxCollider2D>().enabled = true;
                SelectedObject = null;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(RightLimit + Mathf.Abs(LeftLimit) + 1, 8f, 0.1f));
    }
}
