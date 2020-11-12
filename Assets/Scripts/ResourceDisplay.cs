using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ResourceDisplay : MonoBehaviour
{
    public abstract void UpdateDisplay(Resource resource);
    public abstract void SetName(string s);
}
