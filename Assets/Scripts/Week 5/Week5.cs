using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;
using SimpleJSON;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class Week5 : MonoBehaviour
{
    /*
     * Write a CSV parser - that takes in a CSV file of players and returns a list of those players as class objects.
     * To help you out, I've defined the player class below.  An example save file is in the folder "CSV Examples".
     *
     * NOTES:
     *     - the first row of the file has the column headings: don't include those!
     *     - location is tricky - because the location has a comma in it!!
     */

    private class Player
    {
        public enum Class : byte
        {
            Undefined = 0,
            Monk,
            Wizard,
            Druid,
            Thief,
            Sorcerer
        }

        public Class classType;
        public string name;
        public uint maxHealth;
        public int[] stats;
        public bool alive;
        public Vector2 location;

        public Player(Class classType, string name, uint maxHealth, int[] stats, bool alive, Vector2 location)
        {
            this.classType = classType;
            this.name = name;
            this.maxHealth = maxHealth;
            this.stats = stats;
            this.alive = alive;
            this.location = location;
        }
    }

    // Sorry, the project couldn't compile at all on my version of Unity so I have no way of testing if my code compiles or not.
    private List<Player> CSVParser(TextAsset toParse)
    {
        var toReturn = new List<Player>();
        string[] values = toParse.ToString().Split('\n'); // Assuming CSV is divided by , character

        string[] temp = values[0].Split(',');
        int numOfStats = temp.Length - 6;

        for (int i = 1; i < values.Length; i++)
        {
            temp = values[i].Split('\n');
            int[] stats = new int[numOfStats];
            for (int j = 0; j < numOfStats; j++) { stats[j] = int.Parse(temp[3 + j]);}

            toReturn.Add(new Player((Player.Class) int.Parse(temp[0]), temp[1], uint.Parse(temp[2]), stats, bool.Parse(temp[temp.Length - 3]), new Vector2(float.Parse(temp[temp.Length - 2].Remove(1, temp[temp.Length - 2].Length)), float.Parse(temp[temp.Length - 2].Remove(0, temp[temp.Length - 2].Length - 1)))));
        }

        return toReturn;
    }

    /*
     * Provided is a high score list as a JSON file.  Create the functions that will find the highest scoring name, and
     * the number of people with a score above a score.
     *
     * I've included a library "SimpleJSON", which parses JSON into a dictionary of strings to strings or dictionaries
     * of strings.
     *
     * JSON.Parse(someJSONString)["someKey"] will return either a string value, or a Dictionary of strings to
     * JSON objects.
     */

    // JSON Library didn't appear for me, so I'm unsure if I'm using each method correctly.
    public int NumberAboveScore(TextAsset jsonFile, int score)
    {
        var toReturn = 0;

        Dictionary<string, int> scores = new Dictionary<string, int>();
        // scores = JSON.Parse(jsonFile);
        //Assuming Json.Parse(jsonFile) is a Dictionary<string, int> val
        foreach (KeyValuePair<string, int> player in scores)
        {
            if (player.Value == score)
            {
                return toReturn;
            }
            else
            {
                toReturn++;
            }
        }
        return -1;
    }

    public string GetHighScoreName(TextAsset jsonFile)
    {
        int highscore = 0;
        string player = "";

        Dictionary<string, int> scores = new Dictionary<string, int>();
        // scores = JSON.Parse(jsonFile);
        //Assuming Json.Parse(jsonFile) is a Dictionary<string, int> val
        foreach (KeyValuePair<string, int> score in scores)
        {
            if (highscore < score.Value) { highscore = score.Value; player = score.Key; }
        }

        return player;
    }

    // =========================== DON'T EDIT BELOW THIS LINE =========================== //

    public TextMeshProUGUI csvTest, networkTest;
    public TextAsset csvExample, jsonExample;
    private Coroutine checkingScores;

    private void Update()
    {
        csvTest.text = "CSV Parser\n<align=left>\n";

        var parsedPlayers1 = CSVParser(csvExample);

        if (parsedPlayers1.Count == 0)
        {
            csvTest.text += "Need to return some players.";
        }
        else
        {
            csvTest.text += Success(parsedPlayers1.Any(p => p.name == "Jeff") &&
                                    parsedPlayers1.Any(p => p.name == "Stonks")
                            ) + " read in player names correctly.\n";
            csvTest.text +=
                Success(parsedPlayers1.First(p => p.name == "Jeff").alive &&
                        !parsedPlayers1.First(p => p.name == "Stonks").alive) + " Correctly read 'alive'.\n";
            csvTest.text +=
                Success(parsedPlayers1.First(p => p.name == "Stonks").classType == Player.Class.Wizard &&
                        parsedPlayers1.First(p => p.name == "Twergle").classType == Player.Class.Thief) +
                " Correctly read player class.\n";
            csvTest.text +=
                Success(parsedPlayers1.First(p => p.name == "Fortune").location == new Vector2(12.322f, 42f)) +
                " Correctly read in location.\n";
            csvTest.text += Success(parsedPlayers1.First(p => p.name == "Jeff").maxHealth == 23) +
                            " Correctly read in max health.\n";
            csvTest.text +=
                Success(parsedPlayers1.First(p => p.name == "Fortune").location == new Vector2(12.322f, 42f)) +
                " Correctly read in location.\n";
        }

        networkTest.text = "JSON Data\n<align=left>\n";
        networkTest.text += Success(NumberAboveScore(jsonExample, 10) == 6) + " number above score worked correctly.\n";
        networkTest.text += Success(GetHighScoreName(jsonExample) == "GUW") + " get high score name worked correctly.\n";
    }

    private string Success(bool test)
    {
        return test ? "<color=\"green\">PASS</color>" : "<color=\"red\">FAIL</color>";
    }
}