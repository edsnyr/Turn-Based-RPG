using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource
{

    public int max;
    public int current;
    public List<ResourceDisplay> resourceDisplays;

    public Resource(int value) {
        max = value;
        current = value;
    }

    public Resource(int value, List<ResourceDisplay> rd) {
        max = value;
        current = value;
        resourceDisplays = rd;
        UpdateDisplays();
    }

    public void SetDisplays(List<ResourceDisplay> rd) {
        resourceDisplays = rd;
        UpdateDisplays();
    }

    public int AddValue(int value) {
        current += value;
        ConstrainCurrent();
        UpdateDisplays();
        return current;
    }


    public void UpdateMax(int value) {
        max = value;
        ConstrainCurrent();
        UpdateDisplays();
    }

    public void SetCurrent(int value) {
        current = value;
        ConstrainCurrent();
        UpdateDisplays();
    }

    private void ConstrainCurrent() {
        if(current > max) {
            current = max;
        } else if(current < 0) {
            current = 0;
        }
    }

    private void UpdateDisplays() {
        foreach(ResourceDisplay rd in resourceDisplays) {
            rd.UpdateDisplay(this);
        }
    }
}
