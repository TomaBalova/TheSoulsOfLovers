using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public Vector2 playerPos;
    //public SerializableDictionary<string, bool> defeatedEnemies;
    public List<string> EnemiesID;
    public List<bool> EnemiesDef;

    public GameData()
    {
        this.playerPos = Vector2.zero;
        EnemiesID = new List<string>();
        EnemiesDef = new List<bool>();
    }

    public static string GetJsonFromGameData(GameData gameData)
    {
        return JsonConvert.SerializeObject(gameData, Formatting.Indented);
    }
    public static GameData GetFromJsonGameData(string json)
    {
        return JsonConvert.DeserializeObject<GameData>(json);
    }
}
