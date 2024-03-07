using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class PVSaveSystem
{
    public static void SavePearls (CollectablesCounter collectablesCounter)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        // the path where unity is going to save the save file. NOTE: can use anyfile data, i used .path here.
        string path = Application.persistentDataPath + "/pearl.path";

        //File stream is used to read and write from a file.
        FileStream stream = new FileStream(path, FileMode.Create); //creat a file.

        PVData data = new PVData(collectablesCounter);

        // insert the data into a file.
        formatter.Serialize(stream, data); // writing in the stream.
        stream.Close();
    }

    public static PVData LoadPearl()
    {
        string path = Application.persistentDataPath + "/pearl.path";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open); //open the created file.

            PVData data = formatter.Deserialize(stream) as PVData; // reading from the stream.
            stream.Close(); //always need to close it.

            return data;
        }
        else
        {
            Debug.LogError("SavePearls file not found " + path);
            return null;
        }
    }
}
