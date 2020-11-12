using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuButton : MonoBehaviour
{

    public TextMeshProUGUI nameField;
    public TextMeshProUGUI valueField;

    public void SetNameField(string text) {
        nameField.text = text;
    }

    public void SetValueField(int value) {
        valueField.text = value.ToString();
    }

}
