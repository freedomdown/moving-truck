using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsWindow : MonoBehaviour
{
    public GameScore score;
    public float Distance = 600f;
    public float SnapPoint = 5f;
    public float MoveSpeed = 2f;
    private bool DonePrep = false;
    private Vector3 TargetPosition;

    [Header("Things to destroy")]
    public GameObject BioWindow;
    public DragAndDrop Dragger;


    // Update is called once per frame
    void Update()
    {
        if (score.AllDone)
        {
            if (!DonePrep)
                PrepForScroe();

            if (transform.position != TargetPosition)//move towards target position
            {
                Vector3.Lerp(transform.position, TargetPosition, Time.deltaTime * MoveSpeed);
                transform.position = Vector3.Lerp(transform.position, TargetPosition, Time.deltaTime * MoveSpeed);

                if (Vector3.Distance(transform.position, TargetPosition) <= SnapPoint)//snap to target point if we are close enough
                    transform.position = TargetPosition;
            }
            else //done moving towards target position
            {
                //display score
            }
        }
    }

    public void PrepForScroe()
    {
        BioWindow.SetActive(false);
        Destroy(Dragger);

        TargetPosition = transform.position + new Vector3(0f, Distance, 0f);

        DonePrep = true;
    }
}
