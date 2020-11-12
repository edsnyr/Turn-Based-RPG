using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceText : ResourceDisplay {

    TextMeshProUGUI valueDisplay;

    private void Awake() {
        valueDisplay = GetComponent<TextMeshProUGUI>();
    }

    public override void SetName(string s) {
        name = s;
    }

    public override void UpdateDisplay(Resource resource) {
        if(valueDisplay == null)
            valueDisplay = GetComponent<TextMeshProUGUI>();
        valueDisplay.text = resource.current + " / " + resource.max;
    }
}
