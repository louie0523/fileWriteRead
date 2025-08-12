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
    /// 파일을 저장할 파일 이름
    /// </summary>
    public string textFileName = "textPlayer.text";
    // Json 파일은 구지 .json일 필요는 없다. 구조가 Json이면 됨.
    public string jsonFileName = "jsonPlayer.json";
    /// <summary>
    /// 정보를 저장할 상위 폴더
    /// </summary>
    public string folderName = "PlayerData";
    /// <summary>
    /// 실제 경로들
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
        // 다양한 플랫폼에서 사용
        Debug.Log(Application.persistentDataPath);
        // 현재 어플리케이션의 경로
        Debug.Log(Application.dataPath);
        // Path.Combine = 경로를 잇는 거 ( 예 : A = Data / B = SaveN 이라면 A , B 일때 Data/SaveN 이런식이됨. 스트링으로도 가능 )
        folderPath = Path.Combine(Application.dataPath, folderName);
        txtPath = Path.Combine(Application.dataPath, folderName, textFileName);
        jsonPath = Path.Combine(Application.dataPath, folderName, jsonFileName);
    }



    #region Text 파일
    /// <summary>
    /// 폴더가 있는지 확인하고, 없을지 폴더를 생성함.
    /// </summary>
    public void CreateFolder()
     {
        if(Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            Debug.Log("폴더 생성 완료");
        }
     }



    /// <summary>
    /// 텍스트 파일 저장
    /// </summary>
    /// <param name="content"></param>
    public void SaveText(string content)
    {
        CreateFolder();
        File.WriteAllText(txtPath, content);

        Debug.Log("txt 파일 저장 완료");
    }

    public string LoadText()
    {
        if(File.Exists(txtPath))
        {
            return File.ReadAllText(txtPath);
        }
        Debug.Log("txt 파일 로드 실패");
        return null;
    }

    public void UpateFile(string newContent)
    {
        if(File.Exists(txtPath))
        {
            File.WriteAllText(txtPath, newContent);
            Debug.Log("txt 파일 수정 완료");
        } else
        {
            Debug.LogWarning("txt 파일 수정 실패");
        }
    }

    public void DeleteFile()
    {
        if(File.Exists(txtPath))
        {
            File.Delete(txtPath);
            Debug.Log("txt 파일 삭제 완료");
        } else
        {
            Debug.LogWarning("txt 파일 삭제 실패");
        }
    }
    #endregion



    #region Json 파일

    public void SaveJson(PlayerData player)
    {
        string jsonString = JsonUtility.ToJson(player, true);
        File.WriteAllText(jsonPath, jsonString);
        Debug.Log("Json 저장 완료");
    }

    public PlayerData LoadJson()
    {
        if(File.Exists(jsonPath))
        {
            string json = File.ReadAllText(jsonPath);
            // FromJson을 사용할땐 JsonData와 똑같은 데이터가 있어야 함.
            return JsonUtility.FromJson<PlayerData>(json);
        } else
        {
            Debug.LogWarning("json 파일 로드 실패");
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
            Debug.Log("json 파일 저장 완료");
        }
    }

    public void DeleteJson()
    {
        if (File.Exists(jsonPath))
        {
            File.Delete(jsonPath);
            Debug.Log("Json 파일 삭제 완료");
        }
    }


    #endregion


}
