using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLibrary : MonoBehaviour
{
    public List<CharacterStats> characterStats;
    public int GetID(CharacterStats cs) {
        return characterStats.IndexOf(cs);
    }

    public CharacterStats GetAttack(int id) {
        return characterStats[id];
    }
}
