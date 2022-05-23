using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TW;

public static class SaveSystem
{
    public static void Save(SaveVolume pl)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/TOG.savegame";
        Debug.Log(path);
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(pl);

        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log("salvo");
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/TOG.savegame";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            Debug.Log("carrego");

            return data;
        }
        else
        {
            Debug.Log("Sem pasta irmão");
            return null;
        }
    }
}
