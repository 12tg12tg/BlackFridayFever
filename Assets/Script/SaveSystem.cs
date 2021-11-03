using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveInfo(MainWindows main, StorageButtonGroup skin, StorageButtonGroup car, SoundManager sm)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/playInfo.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(main, skin, car, sm);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData LoadInfo()
    {
        string path = Application.persistentDataPath + "/playInfo.fun";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
