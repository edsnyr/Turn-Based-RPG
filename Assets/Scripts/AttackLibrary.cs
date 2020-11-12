using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackLibrary : MonoBehaviour
{

    public List<Attack> attacks;

    public int GetID(Attack attack) {
        return attacks.IndexOf(attack);
    }

    public Attack GetAttack(int id) {
        return attacks[id];
    }
}
