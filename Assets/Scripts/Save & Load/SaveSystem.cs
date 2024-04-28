using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
public static class SaveSystem
{
    public static void SavePlayerData(PlayerStats playerStats)
    {

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/stairCombatPlayer-version-0.2.txt";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData playersData = new PlayerData(playerStats);
        formatter.Serialize(stream, playersData);
        stream.Close();

    }

    public static PlayerData LoadPlayerData()
    {

        string path = Application.persistentDataPath + "/stairCombatPlayer-version-0.2.txt";

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

}