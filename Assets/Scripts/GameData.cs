using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{

    public CharacterLibrary characterLibrary;
    public AttackLibrary attackLibrary;
    public RelicLibrary relicLibrary;

    List<CharacterProgression> characterProgressions;
    List<CharacterStats> chosenCharacters;
    List<CharacterStats> chosenEnemies;

    private void Awake() {
        if(FindObjectsOfType<GameData>().Length > 1)
            Destroy(gameObject);
        characterLibrary = GetComponent<CharacterLibrary>();
        attackLibrary = GetComponent<AttackLibrary>();
        relicLibrary = GetComponent<RelicLibrary>();
        characterProgressions = new List<CharacterProgression>();
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateCharacterProgression(Character character) {
        foreach(CharacterProgression progression in characterProgressions) {
            if(progression.CompareCharacter(character)) {
                progression.UpdateProgression(character);
                return;
            }
        }
        characterProgressions.Add(new CharacterProgression(character));
    }

    public void ApplyCharacterProgression(Character character) {
        foreach(CharacterProgression progression in characterProgressions) {
            if(progression.CompareCharacter(character)) {
                progression.ApplyProgression(character);
                return;
            }
        }
        CharacterProgression p = new CharacterProgression(character);
        characterProgressions.Add(p);
        p.ApplyProgression(character);
    }

    public void AddCharacter(CharacterStats cs) {
        Debug.Log("Add character " + cs.name);
        foreach(CharacterProgression cp in characterProgressions) {
            if(cp.CompareCharacter(cs)) {
                Debug.Log("Already in progression list");
                return;
            }
        }
        characterProgressions.Add(new CharacterProgression(cs));
    }

    public List<CharacterStats> GetAllCharacters() {
        List<CharacterStats> characters = new List<CharacterStats>();
        foreach(CharacterProgression cp in characterProgressions) {
            characters.Add(cp.GetCharacter());
        }
        return characters;
    }

    public void SetChosenCharacters(List<CharacterStats> cs) {
        chosenCharacters = cs;
    }

    public void SetChosenEnemies(List<CharacterStats> cs) {
        chosenEnemies = cs;
    }

    public List<CharacterStats> GetChosenCharacters() {
        return chosenCharacters;
    }

    public List<CharacterStats> GetChosenEnemies() {
        return chosenEnemies;
    }
}

public class CharacterProgression {

    CharacterStats character;
    Resource health;
    List<Attack> attacks;
    List<Relic> relics;

    public CharacterProgression(CharacterStats characterStats) {
        character = characterStats;
        health = new Resource(characterStats.maxHealth);
        attacks = characterStats.startingAttacks;
        relics = characterStats.startingRelics;
    }

    public CharacterProgression(Character c) {
        character = c.stats;
        health = new Resource(c.stats.maxHealth);
        attacks = c.stats.startingAttacks;
        relics = c.stats.startingRelics;
    }

    public bool CompareCharacter(Character c) {
        return character == c.stats;
    }

    public bool CompareCharacter(CharacterStats c) {
        return character == c;
    }

    public CharacterStats GetCharacter() {
        return character;
    }

    public void UpdateProgression(Character c) {
        health = c.GetHealth();
        attacks = c.GetAttacks();
        relics = c.GetRelics();
    }

    public void ApplyProgression(Character c) {
        c.SetHealth(health);
        c.SetAttacks(attacks);
        c.SetRelics(relics);
    }
    
}
