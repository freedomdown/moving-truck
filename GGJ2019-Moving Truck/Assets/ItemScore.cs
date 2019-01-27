using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScore : MonoBehaviour
{
    public enum State { InQueue, OnTruck, InTrash, Damaged };

    public int Joy;
    public int Utility;
    public State state = State.InQueue;
    
    public int GetJoy() {
        if (state == State.OnTruck)
        {
            return Joy;
        }
        return 0;
    }

    public int GetUtility() {
        if (state == State.OnTruck) {
            return Utility;
        }
        return 0;
    }
}
