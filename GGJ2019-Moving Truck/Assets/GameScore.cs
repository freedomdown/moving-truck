using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameScore : MonoBehaviour
{
    public Transform ItemsGroup;
    public Text utilityScore;
    public Text joyScore;
    public ItemQueue queue;
    public bool AllDone = false;
    public Text name;
    public Text personality;
    private int currentUtility = 0;
    private int currentJoy = 0;

    // Update is called once per frame
    void Update()
    {
        name.text = queue.mover.name;
        personality.text = queue.mover.personality;

        ItemScore[] scores = ItemsGroup.GetComponentsInChildren<ItemScore>();
        int utility = scores.Sum(score => score.GetUtility());
        int joy = scores.Sum(score => score.GetJoy());
        if (currentUtility < utility) {
            currentUtility++;
        } else if (currentUtility > utility) {
            currentUtility--;
        }
        if (currentJoy < joy) {
            currentJoy++;
        } else if (currentJoy > joy) {
            currentJoy--;
        }
        joyScore.text = "Joy\n" + currentJoy;
        utilityScore.text = "Utility\n" + currentUtility;

        if (queue.enqueued.Count == 0 && currentJoy == joy && currentUtility == utility) {
            AllDone = true;
        }
    }
}
