using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveInfo(int stageIndex, MainWindows main, StorageButtonGroup skin, StorageButtonGroup car, SoundManager sm, RandomStageInfo randInfo)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/playInfo.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(stageIndex, main, skin, car, sm, randInfo);

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
