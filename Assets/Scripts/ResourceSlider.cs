using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceSlider : ResourceDisplay
{

    Slider slider;

    private void Awake() {
        slider = GetComponent<Slider>();
    }

    public override void UpdateDisplay(Resource resource) {
        if(slider == null)
            slider = GetComponent<Slider>();
        slider.maxValue = resource.max;
        slider.value = resource.current;
    }

    public override void SetName(string s) {
        name = s;
    }
}
