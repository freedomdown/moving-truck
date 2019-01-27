using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BioReader : MonoBehaviour
{
    public TextAsset bioFile;
    private List<Mover> movers = new List<Mover>();

    // Start is called before the first frame update
    void Start()
    {
        ProcessMoversJSON(bioFile.text);
    }

    List<Mover> GetMovers()
    {
        if (movers.Count == 0)
        {
            ProcessMoversJSON(bioFile.text);
        }
        return movers;
    }

    public Mover GetRandomBio()
    {
        if (movers.Count == 0)
        {
            ProcessMoversJSON(bioFile.text);
        }
        return movers[Random.Range(0, movers.Count)];
    }

    void ProcessMoversJSON(string moverJson)
    {
        var dict = MiniJSON.Json.Deserialize(moverJson) as Dictionary<string, object>;
        var moversRaw = new List<object>();
        moversRaw = (List<object>)(((Dictionary<string, object>)dict)["movers"]);
        foreach (Dictionary<string, object> userD in moversRaw)
        {
            var newMover = new Mover();
            newMover.moverName = (string)userD["name"];
            newMover.isGamer = ((string)userD["style"]).Equals("gamer");
            newMover.personality = GetPersonalityText(newMover, userD);
            if (userD.ContainsKey("code"))
            {
                newMover.code = (string)userD["code"];
            }
            newMover.joyValues = new Dictionary<string, int>();
            newMover.specialItems = new List<string>();
            object itemsObj;
            if (userD.TryGetValue("items", out itemsObj))
            {
                var itemDictionary = itemsObj as Dictionary<string, object>;
                foreach (KeyValuePair<string, object> itemEntry in itemDictionary)
                {
                    var itemDetails = itemEntry.Value as Dictionary<string, object>;
                    int value = (int)(long)itemDetails["value"]; // *shrug*
                    newMover.joyValues.Add(itemEntry.Key, value);
                    if (itemDetails.ContainsKey("description"))
                    {
                        newMover.specialItems.Add(itemEntry.Key + ": " + (string)itemDetails["description"]);
                    }
                }
            }
            movers.Add(newMover);
        }
    }

    private string GetPersonalityText(Mover newMover, Dictionary<string, object> userD)
    {
        var personalityText = newMover.moverName;
        if (newMover.isGamer)
        {
            personalityText += "'s home is a tech hideout, ";
        }
        else
        {
            personalityText += "'s home is a chill oasis, ";
        }
        if ((bool)userD["personal"])
        {
            personalityText += "filled with personal style ";
        }
        else
        {
            personalityText += "functionally outfitted ";
        }
        if ((bool)userD["sentimental"])
        {
            personalityText += "and showcasing sentimenal belongings.";
        }
        else
        {
            personalityText += "and focused on meeting their practical needs.";
        }
        if ((bool)userD["organized"])
        {
            personalityText += " Everything is organized in it's proper place.";
        }
        else
        {
            personalityText += " Organization is not their strong suit.";
        }
        return personalityText;
    }

    public class Mover
    {
        public string moverName;
        public bool isGamer;
        public string personality;
        public string code;
        public Dictionary<string, int> joyValues;
        public List<string> specialItems;
    }
}
