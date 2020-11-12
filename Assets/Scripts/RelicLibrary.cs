using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicLibrary : MonoBehaviour
{
    public List<Relic> relics;
    public int GetID(Relic relic) {
        return relics.IndexOf(relic);
    }

    public Relic GetAttack(int id) {
        return relics[id];
    }
}
