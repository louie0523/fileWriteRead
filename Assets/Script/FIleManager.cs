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




    #region txt �Է� UI

    public void OnSaveText()
    {
        if (!string.IsNullOrEmpty(TextInput.text))
        {
            FileInOut.instance.SaveText(TextInput.text);
            outputText.text = "txt ���� �Ϸ�";
        }
        else
        {
            outputText.text = "txt ���� ����";
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
            outputText.text = "text ���� �ε� ����";
        }
    }

    public void OnAppendText()
    {
        if (!string.IsNullOrEmpty(TextInput.text))
        {
            FileInOut.instance.UpateFile(TextInput.text);
            outputText.text = "text ���� �Ϸ�";
        }
        else
        {
            outputText.text = "txt ���� ����";
        }
    }

    public void OnDeleteText()
    {
        FileInOut.instance.DeleteFile();
        outputText.text = "TXT ���� �Ϸ�!";
    }

    #endregion

    #region Json �Է�

    public void OnSaveJson()
    {
        // Try Catch �� : ������ ���ٸ� Try, �ִٸ� Catch�� �����ϸ�, �ɰ��� ������ �� �� �ִ� ��Ȳ���� ����. ���� ������� �ſ� �߿��ϱ� ������ ���� ��
        //�ڷ���.Parse() : ��Ʈ���� �ڷ��� ���·� ��ȯ�ϴ� ��.
        try
        {
            PlayerData player = new PlayerData();
            player.Name = nameInput.text;
            player.Stage = int.Parse(stageInput.text);
            player.Score = int.Parse(scoreInput.text);

            FileInOut.instance.SaveJson(player);
            outputText.text = "Json ���� �Ϸ�";
        }
        catch (System.Exception e)
        {
            outputText.text = "json ���� ���� : " + e;
        }
    }

    public void OnLoadJson()
    {
        PlayerData player = FileInOut.instance.LoadJson();
        if(player != null)
        {
            outputText.text = $"�̸� : {player.Name}, ���� : {player.Stage}, ���� : {player.Score}";
        } else
        {
            outputText.text = "json �ε� ����";
        }
    }

    public void OnUpdateLoad()
    {
        try
        {
            FileInOut.instance.UpdateJsonField(nameInput.text, int.Parse(stageInput.text), int.Parse(scoreInput.text));
            outputText.text = "Json ���� �Ϸ�";
        }
        catch (System.Exception e)
        {
            outputText.text = $"Json ���� ���� : {e.Message}";
        }
    }

    public void OnDeleteJson()
    {
        FileInOut.instance.DeleteJson();
        outputText.text = "Json ���� �Ϸ�";
    }


    #endregion
}
