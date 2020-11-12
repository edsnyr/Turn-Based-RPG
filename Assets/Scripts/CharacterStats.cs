using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Stats", menuName = "Character Stats", order = 1)]
public class CharacterStats : ScriptableObject
{

    public int maxHealth;
    public Sprite sprite;
    public Color color;
    public List<Attack> startingAttacks;
    public List<Relic> startingRelics;

}
