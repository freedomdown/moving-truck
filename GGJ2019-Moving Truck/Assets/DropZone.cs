using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZone : MonoBehaviour
{
    public GameObject Display;//obj gets enabled to show feedback before dropping
    public DragAndDrop draggingArea;//parent to tell when safetodrop/trash
    public bool Trash = false;//this drop zone is for the trash, not the truck

    void OnMouseEnter()
    {
        if (draggingArea.SelectedObject != null)
        {
            if (!Trash)
                draggingArea.SafeToDrop = true;
            else
                draggingArea.SafeToTrash = true;
            if (Display != null)
                Display.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        if (!Trash)
            draggingArea.SafeToDrop = false;
        else
            draggingArea.SafeToTrash = false;
        if (Display != null)
            Display.SetActive(false);
    }
}
