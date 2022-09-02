using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace TheLonelyOne
{
  public class PersistentDataHandler 
  {
    protected readonly string DirPath;
    protected readonly string FileName;
    protected readonly string FullPath;

    protected bool            useEncryption;
    protected readonly string encryptionCodeWord;

    public PersistentDataHandler(string _dirPath, string _fileName, bool _useEncryption = false)
    {
      DirPath  = _dirPath;
      FileName = _fileName;
      FullPath = Path.Combine(DirPath, FileName);

      useEncryption      = _useEncryption;
      encryptionCodeWord = "burn";
    }

    public GameData Load()
    {
      GameData gameData = null;

      if (File.Exists(FullPath))
      {
        try
        {
          string dataToLoad = "";
          using(FileStream stream = new FileStream(FullPath, FileMode.Open))
          {
            using (StreamReader reader = new StreamReader(stream))
              dataToLoad = reader.ReadToEnd();

          }

          if (useEncryption)
            dataToLoad = EncryptDecrypt(dataToLoad);

          gameData = JsonUtility.FromJson<GameData>(dataToLoad);
        }
        catch (Exception e)
        {
          Debug.LogError($"Error \"{e}\" occured when trying to load data from file \"{FullPath}\"");
        }
      }

      return gameData;
    }

    public void Save(GameData _gameData)
    {
      try
      {
        Directory.CreateDirectory(Path.GetDirectoryName(FullPath));

        string dataToSave = JsonUtility.ToJson(_gameData, false);

        if (useEncryption)
          dataToSave = EncryptDecrypt(dataToSave);

        using (FileStream stream = new FileStream(FullPath, FileMode.Create))
        {
          using (StreamWriter writer = new StreamWriter(stream))
            writer.Write(dataToSave);
        }
      }
      catch (Exception e)
      {
        Debug.LogError($"Error \"{e}\" occured when trying to save data to file \"{FullPath}\"");
      }
    }

    protected string EncryptDecrypt(string _data)
    {
      StringBuilder builder = new StringBuilder("", _data.Length);

      for (int i = 0; i < _data.Length; ++i)
        builder.Append((char)(_data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]));

      return builder.ToString();
    }
  }
}
