using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FIleManager : MonoBehaviour
{
    public InputField TextInput;
    public InputField nameInput;
    public InputField stageInput;
    public InputField scoreInput;
    public Text outputText;




    #region txt 입력 UI

    public void OnSaveText()
    {
        if (!string.IsNullOrEmpty(TextInput.text))
        {
            FileInOut.instance.SaveText(TextInput.text);
            outputText.text = "txt 저장 완료";
        }
        else
        {
            outputText.text = "txt 저장 실패";
        }
    }

    public void OnLoadText()
    {
        string result = FileInOut.instance.LoadText();

        if (result != null)
        {
            outputText.text = result;
        }
        else
        {
            outputText.text = "text 파일 로드 실패";
        }
    }

    public void OnAppendText()
    {
        if (!string.IsNullOrEmpty(TextInput.text))
        {
            FileInOut.instance.UpateFile(TextInput.text);
            outputText.text = "text 수정 완료";
        }
        else
        {
            outputText.text = "txt 수정 실패";
        }
    }

    public void OnDeleteText()
    {
        FileInOut.instance.DeleteFile();
        outputText.text = "TXT 삭제 완료!";
    }

    #endregion

    #region Json 입력

    public void OnSaveJson()
    {
        // Try Catch 문 : 오류가 없다면 Try, 있다면 Catch를 실행하며, 심각한 오류가 될 수 있는 상황에서 쓴다. 파일 입출력은 매우 중요하기 때문에 자주 씀
        //자료형.Parse() : 스트링을 자료형 형태로 변환하는 것.
        try
        {
            PlayerData player = new PlayerData();
            player.Name = nameInput.text;
            player.Stage = int.Parse(stageInput.text);
            player.Score = int.Parse(scoreInput.text);

            FileInOut.instance.SaveJson(player);
            outputText.text = "Json 저장 완료";
        }
        catch (System.Exception e)
        {
            outputText.text = "json 저장 실패 : " + e;
        }
    }

    public void OnLoadJson()
    {
        PlayerData player = FileInOut.instance.LoadJson();
        if(player != null)
        {
            outputText.text = $"이름 : {player.Name}, 레벨 : {player.Stage}, 점수 : {player.Score}";
        } else
        {
            outputText.text = "json 로드 실패";
        }
    }

    public void OnUpdateLoad()
    {
        try
        {
            FileInOut.instance.UpdateJsonField(nameInput.text, int.Parse(stageInput.text), int.Parse(scoreInput.text));
            outputText.text = "Json 수정 완료";
        }
        catch (System.Exception e)
        {
            outputText.text = $"Json 수정 실패 : {e.Message}";
        }
    }

    public void OnDeleteJson()
    {
        FileInOut.instance.DeleteJson();
        outputText.text = "Json 삭제 완료";
    }


    #endregion
}
