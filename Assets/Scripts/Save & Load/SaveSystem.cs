using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
public static class SaveSystem
{

    #region Player Saving
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


    public static void DeletePlayerData()
    {
        string path = Application.persistentDataPath + "/defend-for-ages-player.txt";
        File.Delete(path);
    }
    #endregion

    #region Defences Saving
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


    public static void DeleteDefencesData(int defencesID)
    {
        string path = Application.persistentDataPath + "/defencesStatsBase-" + defencesID + ".txt";
        File.Delete(path);
    }

    #endregion

    #region Buyable Area Saving
    public static void SaveBuyableAreaData(BuyableArea buyableArea, int buyableAreaID)
    {

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/buyableArea-" + buyableAreaID + ".txt";
        FileStream stream = new FileStream(path, FileMode.Create);

        BuyableAreaData buyableAreaData = new BuyableAreaData(buyableArea);
        formatter.Serialize(stream, buyableAreaData);
        stream.Close();

    }

    public static BuyableAreaData LoadBuyableAreaData(int buyableAreaID)
    {

        string path = Application.persistentDataPath + "/buyableArea-" + buyableAreaID + ".txt";

        if (File.Exists(path))
        {

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            BuyableAreaData data = formatter.Deserialize(stream) as BuyableAreaData;
            stream.Close();

            return data;
        }
        else
        {

            return null;
        }

    }


    public static void DeleteBuyableAreaData(int buyableAreaID)
    {
        string path = Application.persistentDataPath + "/buyableArea-" + buyableAreaID + ".txt";
        File.Delete(path);
    }

    #endregion

    #region City Manager Saving
    public static void SaveCityManagerData(CityManager cityManager, string cityName)
    {

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/city-" + cityName + ".txt";
        FileStream stream = new FileStream(path, FileMode.Create);

        CityManagerData cityManagerData = new CityManagerData(cityManager);
        formatter.Serialize(stream, cityManagerData);
        stream.Close();

    }

    public static CityManagerData LoadCityManagerData(string cityName)
    {

        string path = Application.persistentDataPath + "/city-" + cityName + ".txt";


        if (File.Exists(path))
        {

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            CityManagerData data = formatter.Deserialize(stream) as CityManagerData;
            stream.Close();

            return data;
        }
        else
        {

            return null;
        }

    }


    public static void DeleteCityManagerData(string cityName)
    {
        string path = Application.persistentDataPath + "/city-" + cityName + ".txt";
        File.Delete(path);
    }
    #endregion
}