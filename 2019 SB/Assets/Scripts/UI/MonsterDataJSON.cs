using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class Serializer
{
    public static T Load<T>(string filename) where T : class
    {
        try
        {
            using (Stream stream = File.OpenRead(filename))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(stream) as T;
            }

        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        return default(T);
    }

    public static void Save<T>(string filename, T data) where T : class
    {
        using (Stream stream = File.OpenWrite(filename))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, data);
        }
    }
    public static void objectToJSON(string fileName, object type)
    { //save a json file to the StreamingAssets folder.
        string data = JValue.Parse(JsonConvert.SerializeObject(type)).ToString(Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText(Application.streamingAssetsPath + "/" + fileName, data);
    }

    public static T JSONtoObject<T>(string fileName)
    { //load a json file from the StreamingAssets folder.
        FileStream file;
        StreamReader sr;
        file = new FileStream(Application.streamingAssetsPath + "/" + fileName, FileMode.Open, FileAccess.Read);
        sr = new StreamReader(file);
        string jData = sr.ReadToEnd();
        file.Close();
        return JsonConvert.DeserializeObject<T>(jData);
    }
}


public class MonsterData
{
    public static Dictionary<string, Dictionary<string, float>> TypeEffectiveness
        = new Dictionary<string, Dictionary<string, float>>();

    public static Dictionary<int, Dictionary<int, int>> ExpGroupData
    = new Dictionary<int, Dictionary<int, int>>();

    public static Dictionary<string, int> BaseExpYield = new Dictionary<string, int>();

}

public class AreaData
{
    //arealevels
    //monsters in area with chance of spawn
}


public class MonsterDataJSON : MonoBehaviour
{
     void Awake()
    {
        MonsterData.TypeEffectiveness = Serializer.JSONtoObject<Dictionary<string, 
            Dictionary<string, float>>>("typeEffectiveness.json");

        MonsterData.ExpGroupData = Serializer.JSONtoObject<Dictionary<int,
            Dictionary<int, int>>>("ExpGroupData.json");

        MonsterData.BaseExpYield = Serializer.JSONtoObject<Dictionary<string, int>> ("BaseExpYield.json");
    }

}
