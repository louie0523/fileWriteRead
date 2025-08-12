using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;



[SerializeField]
public class PlayerData
{
    public string Name;
    public int Stage;
    public int Score;
}

public class FileInOut : MonoBehaviour
{
    public static FileInOut instance = null;
    /// <summary>
    /// ������ ������ ���� �̸�
    /// </summary>
    public string textFileName = "textPlayer.text";
    // Json ������ ���� .json�� �ʿ�� ����. ������ Json�̸� ��.
    public string jsonFileName = "jsonPlayer.json";
    /// <summary>
    /// ������ ������ ���� ����
    /// </summary>
    public string folderName = "PlayerData";
    /// <summary>
    /// ���� ��ε�
    /// </summary>
    string folderPath;
    string txtPath;
    string jsonPath;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // �پ��� �÷������� ���
        Debug.Log(Application.persistentDataPath);
        // ���� ���ø����̼��� ���
        Debug.Log(Application.dataPath);
        // Path.Combine = ��θ� �մ� �� ( �� : A = Data / B = SaveN �̶�� A , B �϶� Data/SaveN �̷����̵�. ��Ʈ�����ε� ���� )
        folderPath = Path.Combine(Application.dataPath, folderName);
        txtPath = Path.Combine(Application.dataPath, folderName, textFileName);
        jsonPath = Path.Combine(Application.dataPath, folderName, jsonFileName);
    }



    #region Text ����
    /// <summary>
    /// ������ �ִ��� Ȯ���ϰ�, ������ ������ ������.
    /// </summary>
    public void CreateFolder()
     {
        if(Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            Debug.Log("���� ���� �Ϸ�");
        }
     }



    /// <summary>
    /// �ؽ�Ʈ ���� ����
    /// </summary>
    /// <param name="content"></param>
    public void SaveText(string content)
    {
        CreateFolder();
        File.WriteAllText(txtPath, content);

        Debug.Log("txt ���� ���� �Ϸ�");
    }

    public string LoadText()
    {
        if(File.Exists(txtPath))
        {
            return File.ReadAllText(txtPath);
        }
        Debug.Log("txt ���� �ε� ����");
        return null;
    }

    public void UpateFile(string newContent)
    {
        if(File.Exists(txtPath))
        {
            File.WriteAllText(txtPath, newContent);
            Debug.Log("txt ���� ���� �Ϸ�");
        } else
        {
            Debug.LogWarning("txt ���� ���� ����");
        }
    }

    public void DeleteFile()
    {
        if(File.Exists(txtPath))
        {
            File.Delete(txtPath);
            Debug.Log("txt ���� ���� �Ϸ�");
        } else
        {
            Debug.LogWarning("txt ���� ���� ����");
        }
    }
    #endregion



    #region Json ����

    public void SaveJson(PlayerData player)
    {
        string jsonString = JsonUtility.ToJson(player, true);
        File.WriteAllText(jsonPath, jsonString);
        Debug.Log("Json ���� �Ϸ�");
    }

    public PlayerData LoadJson()
    {
        if(File.Exists(jsonPath))
        {
            string json = File.ReadAllText(jsonPath);
            // FromJson�� ����Ҷ� JsonData�� �Ȱ��� �����Ͱ� �־�� ��.
            return JsonUtility.FromJson<PlayerData>(json);
        } else
        {
            Debug.LogWarning("json ���� �ε� ����");
            return null;    
        }
    }

    public void UpdateJsonField(string nae, int stage, int socre)
    {
        PlayerData data = LoadJson();
        if(data != null)
        {
            data.Name = nae;
            data.Stage = stage;
            data.Score = socre;
            SaveJson(data);
            Debug.Log("json ���� ���� �Ϸ�");
        }
    }

    public void DeleteJson()
    {
        if (File.Exists(jsonPath))
        {
            File.Delete(jsonPath);
            Debug.Log("Json ���� ���� �Ϸ�");
        }
    }


    #endregion


}
