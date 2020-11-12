using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack", order = 2)]
public class Attack : ScriptableObject
{
    public int value;
    public int cost;
    public Type type;
    public Element element;
    public Alliance targeting;

    public Attack ApplyOffenseModifier(ElementalMatrix offensiveMatrix) {
        Attack newAttack = this;
        newAttack.value += offensiveMatrix.GetModifier(element);
        return newAttack;
    }
}

public enum Type { Damage, Healing }
public enum Element { Light, Heavy, Fire, Ice , Neutral }
