using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemySelectionButton : MonoBehaviour
{

    BattleCreator battleCreator;
    CharacterStats characterStats;

    public TextMeshProUGUI mainButtonText;
    public Button AddButton;
    public Button SubtractButton;


    public void InitializeButton(BattleCreator bc, CharacterStats cs) {
        battleCreator = bc;
        characterStats = cs;

        mainButtonText.text = characterStats.name + ": 0";
        AddButton.onClick.AddListener(() => battleCreator.AddEnemy(cs));
        AddButton.onClick.AddListener(UpdateButtonText);
        SubtractButton.onClick.AddListener(() => battleCreator.RemoveEnemy(cs));
        SubtractButton.onClick.AddListener(UpdateButtonText);
    }

    public void UpdateButtonText() {
        mainButtonText.text = characterStats.name + ": " + battleCreator.GetChosenEnemiesCount(characterStats);
    }
}
