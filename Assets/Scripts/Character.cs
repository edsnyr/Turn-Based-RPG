using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    public CharacterStats stats;
    public Alliance alliance;

    public SpriteRenderer sr;
    public SpriteRenderer selectionArrow;

    [HideInInspector] public GameObject attackMenu;
    [HideInInspector] public GameObject relicMenu;

    Resource health;
    public List<ResourceDisplay> healthDisplays;
    List<Attack> attacks;
    public List<Relic> relics;

    ElementalMatrix offensiveMatrix;
    ElementalMatrix defensiveMatrix;


    public void InitializeCharacter(CharacterStats cs, Alliance a) {
        stats = cs;
        alliance = a;
        name = stats.name;
        sr.sprite = stats.sprite;
        sr.color = stats.color;
        InitializeSelectionArrow();
    }

    public void AddHealthDisplays(ResourceDisplay rd) {
        foreach(ResourceDisplay display in rd.GetComponentsInChildren<ResourceDisplay>()) {
            if(!healthDisplays.Contains(display))
                healthDisplays.Add(display);
        }
        InitializeDisplays();
    }

    private void InitializeDisplays() {
        foreach(ResourceDisplay rd in healthDisplays) {
            rd.SetName(name + "'s Health");
        }
    }

    public void AddRelic(Relic relic) {
        foreach(Modifier modifier in relic.modifiers) {
            if(modifier.buff == Buff.Offense) {
                offensiveMatrix.AddModifier(modifier);
            } else {
                defensiveMatrix.AddModifier(modifier);
            }
        }
        relics.Add(relic);
    }

    public void RemoveRelic(Relic relic) {
        if(relics.Contains(relic)) {
            foreach(Modifier modifier in relic.modifiers) {
                if(modifier.buff == Buff.Offense) {
                    offensiveMatrix.RemoveModifier(modifier);
                } else {
                    defensiveMatrix.RemoveModifier(modifier);
                }
                relics.Remove(relic);
            }
        }
    }

    private void InitializeSelectionArrow() {
        selectionArrow.gameObject.SetActive(false);
    }

    public ElementalMatrix GetMatrix(Buff buff) {
        if(buff == Buff.Offense) {
            return offensiveMatrix;
        } else {
            return defensiveMatrix;
        }
    }

    public void ApplyAttack(Attack attack, Character attackingCharacter) {
        int offensiveValue = (attack.value + attackingCharacter.GetMatrix(Buff.Offense).GetModifier(attack.element)) * (attack.type == Type.Healing ? 1 : -1);
        int defensiveValue = defensiveMatrix.GetModifier(attack.element) * (attack.type == Type.Healing ? 1 : -1);
        health.AddValue(offensiveValue - defensiveValue);
    }

    public bool GetAlive() {
        return health.current > 0;
    }

    public Resource GetHealth() {
        return health;
    }

    public List<Attack> GetAttacks() {
        return attacks;
    }

    public List<Relic> GetRelics() {
        return relics;
    }

    /// <summary>
    /// Copies the given resource into the character's health.
    /// Any current health displays are given to the new health resource.
    /// </summary>
    /// <param name="h"></param>
    public void SetHealth(Resource h) {
        health = h;
        Debug.Log(health.current);
        health.SetDisplays(healthDisplays);
    }

    /// <summary>
    /// Sets the given list of Attack to the character.
    /// If the character is a player, the battle menu must be recreated.
    /// </summary>
    /// <param name="a"></param>
    public void SetAttacks(List<Attack> a) {
        attacks = a;
    }

    /// <summary>
    /// Resets the relic and matrix lists and applies the given set of relics.
    /// </summary>
    /// <param name="r"></param>
    public void SetRelics(List<Relic> r) {
        Debug.Log(name + " set relics");
        relics = new List<Relic>();
        offensiveMatrix = new ElementalMatrix();
        defensiveMatrix = new ElementalMatrix();
        foreach(Relic relic in r) {
            AddRelic(relic);
        }
    }

    private void OnMouseEnter() {
        if(BattleManager.state == State.Target && BattleManager.targeting == alliance) {
            selectionArrow.gameObject.SetActive(true);
        }
    }

    private void OnMouseExit() {
        selectionArrow.gameObject.SetActive(false);
    }

    private void OnMouseDown() {
        if(BattleManager.state == State.Target && BattleManager.targeting == alliance) {
            BattleManager.abilityTargets.AddListener(ApplyAttack);
            selectionArrow.gameObject.SetActive(false);
            BattleManager.changeStateEvent.Invoke(State.Confirm);
        }
    }

    public void ApplyDefaultStats() {
        SetHealth(new Resource(stats.maxHealth));
        SetAttacks(stats.startingAttacks);
        SetRelics(stats.startingRelics);
    }
}

public enum Alliance { Enemy, Ally, Neutral };

public class ElementalMatrix {

    List<Modifier> modifiers;

    public ElementalMatrix() {
        modifiers = new List<Modifier>();
        foreach(Element element in Enum.GetValues(typeof(Element))) {
            modifiers.Add(new Modifier(element, 0));
        } 
    }

    public void AddModifier(Modifier newModifier) {
        foreach(Modifier modifier in modifiers) {
            if(modifier.element == newModifier.element) {
                modifier.value += newModifier.value;
                break;
            }
        }
    }

    public void RemoveModifier(Modifier newModifier) {
        foreach(Modifier modifier in modifiers) {
            if(modifier.element == newModifier.element) {
                modifier.value -= newModifier.value;
                break;
            }
        }
    }

    public int GetModifier(Element element) {
        foreach(Modifier modifier in modifiers) {
            if(modifier.element == element) {
                return modifier.value;
            }
        }
        Debug.Log("No matching element");
        return 0;
    }

}


