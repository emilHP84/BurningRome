using System.IO;
using System.Linq.Expressions;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveLoadSystem // THE SCRIPT TO SAVE TO DISK AND LOAD FROM DISK (see also SaveData class)
{

    public static bool SaveFileExists(int saveIndex) // Call this to check if a save file exists
    {
        return File.Exists(SavePath(saveIndex));
    }

    public static void EraseFile(int saveIndex) // Call this to erase a save file
    {
        if (SaveFileExists(saveIndex)) File.Delete(SavePath(saveIndex));
    }


    public static void Save(SaveData dataToSave, int saveIndex) // Call this to give the save data and write it on a disk file
    {
        SaveData data = dataToSave;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        file = File.Open(SavePath(saveIndex), FileMode.OpenOrCreate);
        try
        {
            bf.Serialize(file, data);
        }
        catch
        {
            Debug.Log("💾⚠️ ERROR while saving file at "+saveIndex);
            file.Close();
            EraseFile(saveIndex);
        }
        file.Close();
        Debug.Log("💾✅Saved data to "+SavePath(saveIndex));
    }


    public static SaveData Load(int saveIndex) // Call this to load the save file and get it in SaveData form
    {
        if (SaveFileExists(saveIndex))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(SavePath(saveIndex), FileMode.Open);
            try
            {
                SaveData data = (SaveData)bf.Deserialize(file);
                file.Close();
                Debug.Log("💾⬆️ Loaded data from file "+saveIndex);
                return data;
            }
            catch
            {
                Debug.Log("💾⚠️ ERROR Loading file at "+saveIndex+", ERASING it");
                file.Close();
                EraseFile(saveIndex);
                return null;
            }
        }
        else
        {
            Debug.Log("💾⚠️ ERROR: TRYING TO LOAD, BUT NO SAVE FILE FOUND AT "+SavePath(saveIndex));
            return null;
        }
    }

    static string SavePath(int saveIndex){return Application.persistentDataPath+"save"+saveIndex+".dat";}

} // SCRIPT END

