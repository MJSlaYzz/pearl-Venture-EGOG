using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public static class Cop28SaveSystem
{
    public static void SaveProgress (Cop28DataManager cop28DataManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        // the path where unity is going to save the save file. NOTE: can use anyfile data, i used .path here.
        string path = Application.persistentDataPath + "/pearl.path";

        //File stream is used to read and write from a file.
        FileStream stream = new FileStream(path, FileMode.Create); //creat a file.

        Cop28Data data = new Cop28Data(cop28DataManager);

        // insert the data into a file.
        formatter.Serialize(stream, data); // writing in the stream.
        stream.Close();
        Debug.Log("Data Saved to the path: " + path);
    }

    public static Cop28Data LoadProgress()
    {
        string path = Application.persistentDataPath + "/pearl.path";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open); //open the created file.

            Cop28Data data = formatter.Deserialize(stream) as Cop28Data; // reading from the stream.
            stream.Close(); //always need to close it.
            Debug.Log("Data loaded successfully from the path:" + path);

            return data;
        }
        else
        {
            Debug.LogError("SavePearls file not found " + path);
            return null;
        }
    }
    public static void DeleteProgress()
    {
        string path = Application.persistentDataPath + "/pearl.path";

        if (File.Exists(path))
        {
            try
            {
                File.Delete(path);
                Debug.Log("Saved data deleted successfully from: " + path);
            }
            catch (Exception e)
            {
                Debug.LogError("Error deleting saved data: " + e.Message);
            }
        }
        else
        {
            Debug.Log("No saved data found at: " + path);
        }
    }

}
