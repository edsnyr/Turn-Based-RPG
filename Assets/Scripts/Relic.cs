using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Relic", menuName = "Relic", order = 3)]
public class Relic : ScriptableObject
{

    public Sprite sprite;
    public List<Modifier> modifiers;

}

[Serializable]
public class Modifier {
    public Element element;
    public Buff buff;
    public int value;


    public Modifier() {
        element = Element.Neutral;
        value = 0;
    }

    public Modifier(Element e, int v) {
        element = e;
        value = v;
    }
}

public enum Buff { Offense, Defense }
