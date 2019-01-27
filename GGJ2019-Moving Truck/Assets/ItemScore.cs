using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScore : MonoBehaviour
{
    public enum State { InQueue, OnTruck, InTrash, Damaged };

    public int Joy;
    public int Utility;
    public State state = State.InQueue;
    
    public double GetScore() {
        if (state == State.OnTruck) {
            return Joy; //todo: maths here
        }
        return 0;
    }
}
