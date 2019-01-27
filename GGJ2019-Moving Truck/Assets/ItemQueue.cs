using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Mover = BioReader.Mover;

public class ItemQueue : MonoBehaviour
{
    public DragAndDrop dragAndDrop;
    public Transform ItemsGroup;

    private Dictionary<Tuple<int, int>, GameObject> prefabs;

    private List<ItemData> items;

    public int QueueLength = 6;
    public List<GameObject> enqueued = new List<GameObject>();
    private bool drawQueue = false;

    public BioReader BioReader;
    public Mover mover;

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

        mover = BioReader.GetRandomBio();
        print(mover.name);

        items = new List<ItemData>()
        {
            { new ItemData(1, 1, "Art", 50, mover.joyValues["art"], 10) },
            { new ItemData(3, 2, "Bed", 50, mover.joyValues["bed"], 10) },
            { new ItemData(2, 2, "Chair", 0, mover.joyValues["chair"], 0) },
            { new ItemData(1, 1, "Computer", 50, mover.joyValues["computer"], 10) },
            { new ItemData(2, 3, "Sofa", 50, mover.joyValues["sofa"], 10) },
            { new ItemData(2, 2, "Desk", 50, mover.joyValues["desk"], 10) },
            { new ItemData(2, 2, "Dresser", 50, mover.joyValues["dresser"], 10) },
            { new ItemData(1, 3, "Lamp", 10, mover.joyValues["lamp"], 0) },
            { new ItemData(1, 3, "Plant", 50, mover.joyValues["plants"], 10) },
            { new ItemData(2, 3, "Shelves", 50, mover.joyValues["shelves"], 10) },
            { new ItemData(2, 2, "SideTable", 50, mover.joyValues["sidetable"], 10) }
        };

        if (mover.isGamer)
        {
            items.Add(new ItemData(1, 1, "Console", 50, mover.joyValues["console"], 10));
            items.Add(new ItemData(1, 1, "Collectible", 50, mover.joyValues["collectible"], 10));
            items.Add(new ItemData(1, 1, "Headset", 50, mover.joyValues["headset"], 10));
            items.Add(new ItemData(1, 1, "Plush", 50, mover.joyValues["plush"], 10));
        } else
        {
            items.Add(new ItemData(1, 1, "Journal", 50, mover.joyValues["journals"], 10));
            items.Add(new ItemData(1, 1, "Instrument", 50, mover.joyValues["instrument"], 10));
            items.Add(new ItemData(1, 1, "Rug", 50, mover.joyValues["rug"], 10));
            items.Add(new ItemData(1, 1, "Stereo", 50, mover.joyValues["music"], 10));
        }
        print(items.Count);

        items = Shuffle(items);

        while (QueueHasSpaceForNext()) { SpawnNextItem(); }
    }

    private void Update() {
        if (drawQueue) {
            float offset = 0;
            foreach (GameObject item in enqueued) {
                float width = item.GetComponent<ItemClick>().Width;
                float x = gameObject.transform.position.x - offset - width;
                float y = gameObject.transform.position.y;
                item.transform.position = new Vector3(x, y, 0);
                offset += width * 1.2f;
            }
            drawQueue = false;
        }
    }

    private List<T> Shuffle<T>(List<T> list) {
        int n = list.Count;
        while (n > 1) {
            int k = Random.Range(0, n);
            n--;
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }

    private bool QueueHasSpaceForNext() {
        float currentQueueLen = 0;
        foreach (GameObject item in enqueued) {
            currentQueueLen += item.GetComponent<ItemClick>().Width;
        }
        return items.Count > 0 && currentQueueLen + PeekData().Width <= QueueLength;
    }

    internal void RemoveFromQueue(Transform selectedObject) {
        enqueued.Remove(selectedObject.gameObject);
        SpawnNextItem();
    }

    private ItemData PopData() {
        ItemData item = items[items.Count - 1];
        items.RemoveAt(items.Count - 1);
        return item;
    }

    private ItemData PeekData() {
        return items[items.Count - 1];
    }

    private void SpawnNextItem() {
        if (!QueueHasSpaceForNext()) {
            return;
        }
        ItemData data = PopData();

        string root = "Gamer/";
        if (mover.isGamer)
        {
            root = "Gamer/";
        } else
        {
            root = "Comfy/";
        }
        GameObject item = Instantiate(Resources.Load(root + data.Label) as GameObject);
        item.transform.SetParent(ItemsGroup);
//        TextMesh text = item.GetComponentInChildren<TextMesh>();
 //       text.text = data.Label;
        ItemScore score = item.GetComponentInChildren<ItemScore>();
        score.Joy = data.Joy;
        score.Utility = data.Utility;
        Durability durability = item.GetComponentInChildren<Durability>();
        durability.Weight = data.Width + data.Height;
        durability.Strength = data.Width * 5;
        enqueued.Add(item);
        drawQueue = true;
    }

    private static Vector3 getCurrentPosition() {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
        curPosition = new Vector3(curPosition.x, curPosition.y, 0f);
        return curPosition;
    }

    private class ItemData {
        public ItemData(int width, int height, string label, int utility, int joy, int strength) {
            Width = width;
            Height = height;
            Label = label;
            Utility = utility;
            Joy = joy;
            Strength = strength;
        }

        public int Width { get; set; }
        public int Height { get; set; }
        public string Label { get; set; }
        public int Utility { get; set; }
        public int Joy { get; set; }
        public int Strength { get; set; }
    }
}
