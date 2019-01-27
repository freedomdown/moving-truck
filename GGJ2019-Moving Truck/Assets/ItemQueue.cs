using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemQueue : MonoBehaviour
{
    public DragAndDrop dragAndDrop;

    private Dictionary<Tuple<int, int>, GameObject> prefabs;

    private List<ItemData> items;

    public int QueueLength = 8;
    public List<GameObject> enqueued = new List<GameObject>();
    private bool drawQueue = false;

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

        items = Shuffle(new List<ItemData>() {
            { new ItemData(1, 1, "Cards", 0, 100, 0) },
            { new ItemData(2, 3, "Book Shelf", 10, 60, 0) },
            { new ItemData(2, 2, "Computer", 50, 80, 10) },
            { new ItemData(3, 2, "Bed", 50, 80, 10) },
            { new ItemData(1, 2, "Lamp", 50, 80, 10) },
            { new ItemData(1, 1, "Collectables", 50, 80, 10) },
        });

        while (QueueHasSpaceForNext()) { SpawnNextItem(); }
    }

    private void Update() {
        if (drawQueue) {
            float offset = 0;
            foreach (GameObject item in enqueued) {
                float x = gameObject.transform.position.x - offset;
                float y = gameObject.transform.position.y;
                item.transform.position = new Vector3(x, y, 0);
                offset += item.transform.lossyScale.x;
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
        return items.Count >0 
            && enqueued.Sum(item => item.transform.localScale.x) + PeekData().Width <= QueueLength;
    }

    internal void RemoveFromQueue(Transform selectedObject) {
        enqueued.Remove(selectedObject.gameObject);
        SpawnNextItem();
    }

    private void OnMouseDown() {
        //SpawnNextItem();
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

        GameObject item = Instantiate(prefabs[Tuple.Create(data.Width, data.Height)]);
        TextMesh text = item.GetComponentInChildren(typeof(TextMesh)) as TextMesh;
        text.text = data.Label;
        ItemScore score = item.GetComponentInChildren(typeof(ItemScore)) as ItemScore;
        score.Joy = data.Joy;
        score.Utility = data.Utility;
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
}
