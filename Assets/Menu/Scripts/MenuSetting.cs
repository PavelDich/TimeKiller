using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuSetting : MonoBehaviour
{
    public InputFieldData[] inputFieldData;
    [Serializable]
    public class InputFieldData
    {
        public string SaveName;
        public TMP_InputField inputField;
    }
    public ButtonData[] buttonData;
    [Serializable]
    public class ButtonData
    {
        public string SaveName;
        public Button button;
        public int ID;
        public string[] IDnames;
    }



    private void Start()
    {
        ImportInputField();
        ImportButton();
    }

    public void SaveInputField()
    {
        foreach (InputFieldData i in inputFieldData)
        {
            if (i.inputField.text != "")
            {
                PlayerPrefs.SetString(i.SaveName, i.inputField.text);
                i.inputField.text = "";
            }
        }
        ImportInputField();
    }
    public void ImportInputField()
    {
        foreach (InputFieldData i in inputFieldData)
        {
            TMP_Text a = i.inputField.placeholder.GetComponent<TMP_Text>();

            a.text = PlayerPrefs.GetString(i.SaveName);

            if (a.text == "" || a.text == null)
                a.text = "100";
        }
    }

    public void SetButton(int ButtonNumer)
    {
        buttonData[ButtonNumer].ID++;
        if (buttonData[ButtonNumer].ID >= buttonData[ButtonNumer].IDnames.Length) buttonData[ButtonNumer].ID = 0;
        SaveButton();
    }
    public void SaveButton()
    {
        foreach (ButtonData i in buttonData)
            PlayerPrefs.SetInt(i.SaveName, i.ID);

        ImportButton();
    }
    public void ImportButton()
    {
        foreach (ButtonData i in buttonData)
        {
            i.ID = PlayerPrefs.GetInt(i.SaveName);
            TMP_Text a = i.button.GetComponentInChildren<TMP_Text>();
            a.text = i.IDnames[i.ID];
            Debug.Log(i.ID);
        }
    }
}
