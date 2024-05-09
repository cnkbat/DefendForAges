using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
public static class SaveSystem
{

    public static void SavePlayerData(PlayerStats playerStats)
    {

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/defend-for-ages-player.txt";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData playersData = new PlayerData(playerStats);
        formatter.Serialize(stream, playersData);
        stream.Close();

    }

    public static PlayerData LoadPlayerData()
    {

        string path = Application.persistentDataPath + "/defend-for-ages-player.txt";

        if (File.Exists(path))
        {

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else
        {

            return null;
        }

    }


    public static void SaveDefencesData(DefencesStatsBase defencesStatsBase, int defencesID)
    {

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/defencesStatsBase-" + defencesID + ".txt";
        FileStream stream = new FileStream(path, FileMode.Create);

        DefencesData defencesData = new DefencesData(defencesStatsBase);
        formatter.Serialize(stream, defencesData);
        stream.Close();

    }

    public static DefencesData LoadDefenceData(int defencesID)
    {

        string path = Application.persistentDataPath + "/defencesStatsBase-" + defencesID + ".txt";

        if (File.Exists(path))
        {

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            DefencesData data = formatter.Deserialize(stream) as DefencesData;
            stream.Close();

            return data;
        }
        else
        {

            return null;
        }

    }
}