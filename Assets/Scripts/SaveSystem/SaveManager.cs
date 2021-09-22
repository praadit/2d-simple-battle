using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class SaveManager : MonoBehaviour {
    #region Singleton
    public static SaveManager instance { get; set; }
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region Variables Declaration
    private int m_highscore;
    #endregion

    #region Save and Load State
    public void SaveData()
    {
        BinaryFormatter bin = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/attack_data.mfile");
        GameDataFile data = new GameDataFile();
        
        //put your save variables here
        data.highscore = m_highscore;

        bin.Serialize(file, data);
        file.Close();
    }

    public void LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/attack_data.mfile"))
        {
            BinaryFormatter bin = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/attack_data.mfile", FileMode.Open);

            GameDataFile data = bin.Deserialize(file) as GameDataFile;
            
            file.Close();

            //put your load variables here
            m_highscore = data.highscore;

        }

    }
    #endregion

    #region Set Data
    public void SetHscore(int _highscore) { m_highscore = _highscore; }
    #endregion

    #region Get Data
    public int GetHscore() { return m_highscore; }
    #endregion
}

[Serializable]
public class GameDataFile
{
    public int highscore;
}
