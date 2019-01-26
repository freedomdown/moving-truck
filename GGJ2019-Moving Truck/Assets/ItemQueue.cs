using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemQueue : MonoBehaviour
{
    private Dictionary<Tuple<int, int>, GameObject> prefabs;

    private class ItemData {
        public ItemData(int width, int height, string label, int utility, int joy, int fragility) {
            Width = width;
            Height = height;
            Label = label;
            Utility = utility;
            Joy = joy;
            Fragility = fragility;
        }

        public int Width { get; set; }
        public int Height { get; set; }
        public string Label { get; set; }
        public int Utility { get; set; }
        public int Joy { get; set; }
        public int Fragility { get; set; }
    }

    private void Start() {
        prefabs = new Dictionary<Tuple<int, int>, GameObject>() {
            { Tuple.Create(1, 1), Resources.Load("1by1box") as GameObject },
            { Tuple.Create(1, 2), Resources.Load("1by2box") as GameObject },
            { Tuple.Create(1, 3), Resources.Load("1by3box") as GameObject },
            { Tuple.Create(2, 1), Resources.Load("2by1box") as GameObject },
            { Tuple.Create(2, 2), Resources.Load("2by2box") as GameObject },
            { Tuple.Create(2, 3), Resources.Load("2by3box") as GameObject },
            { Tuple.Create(3, 1), Resources.Load("3by1box") as GameObject },
            { Tuple.Create(3, 2), Resources.Load("3by2box") as GameObject },
            { Tuple.Create(3, 3), Resources.Load("3by3box") as GameObject },
        };
    }

    private readonly List<ItemData> data = new List<ItemData>() {
        { new ItemData(1, 1, "Cards", 0, 100, 0) },
        { new ItemData(2, 3, "Book Shelf", 10, 60, 0) },
        { new ItemData(2, 2, "Computer", 50, 80, 10) },
    };

    private void OnMouseDown() {
        ItemData d = data[Random.Range(0, data.Count)];
    }
}
