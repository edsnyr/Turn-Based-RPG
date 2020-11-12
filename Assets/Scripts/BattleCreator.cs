using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleCreator : MonoBehaviour
{
    GameData gameData;

    public List<CharacterStats> availablePlayerCharacters;
    public List<CharacterStats> availableEnemyCharacters;

    public int maxCharacters = 4;
    public Button confirmButton;
    public Transform playerSelectMenu;
    public Transform enemySelectMenu;
    public Button selectionButtonPrefab;
    public EnemySelectionButton enemySelectionButtonPrefab;
    public ColorBlock chosenPlayerColor;
    public ColorBlock notChosenPlayerColor;

    List<CharacterStats> chosenPlayerCharacters;
    List<CharacterStats> chosenEnemyCharacters;

    void Awake() {
        gameData = FindObjectOfType<GameData>();
        chosenPlayerCharacters = new List<CharacterStats>();
        chosenEnemyCharacters = new List<CharacterStats>();
        confirmButton.onClick.AddListener(GoToBattle);
        SetConfirmButton();
        CreateSelectionMenus();
    }

    private void CreateSelectionMenus() {
        float dist = selectionButtonPrefab.GetComponent<RectTransform>().rect.height;
        for(int i = 0; i < availablePlayerCharacters.Count; i++) {
            Button button = Instantiate(selectionButtonPrefab, playerSelectMenu);
            button.transform.localPosition = new Vector3(0, i * (dist + 5) * -1, 0);
            button.colors = notChosenPlayerColor;
            button.GetComponentInChildren<TextMeshProUGUI>().text = availablePlayerCharacters[i].name;
            int x = i; //dumb lambda function stuff, don't remove
            button.onClick.AddListener(() => AddRemovePlayer(availablePlayerCharacters[x]));
            button.onClick.AddListener(() => button.colors = (chosenPlayerCharacters.Contains(availablePlayerCharacters[x]) ? chosenPlayerColor : notChosenPlayerColor));
        }
        for(int i = 0; i < availableEnemyCharacters.Count; i++) {
            EnemySelectionButton button = Instantiate(enemySelectionButtonPrefab, enemySelectMenu);
            button.transform.localPosition = new Vector3(0, i * (dist + 5) * -1, 0);
            button.InitializeButton(this, availableEnemyCharacters[i]);
        }
    }

    public void AddRemovePlayer(CharacterStats cs) {
        if(chosenPlayerCharacters.Contains(cs))
            chosenPlayerCharacters.Remove(cs);
        else
            chosenPlayerCharacters.Add(cs);
        SetConfirmButton();

    }

    public void AddEnemy(CharacterStats cs) {
        if(chosenEnemyCharacters.Count < maxCharacters) {
            chosenEnemyCharacters.Add(cs);
        }
        SetConfirmButton();
    }

    public void RemoveEnemy(CharacterStats cs) {
        if(chosenEnemyCharacters.Contains(cs)) {
            chosenEnemyCharacters.Remove(cs);
        }
        SetConfirmButton();
    }

    public int GetChosenEnemiesCount(CharacterStats cs) {
        int count = 0;
        foreach(CharacterStats stats in chosenEnemyCharacters) {
            if(stats == cs)
                count++;
        }
        return count;
    }

    public void GoToBattle() {
        if(CheckReady()) {
            gameData.SetChosenCharacters(chosenPlayerCharacters);
            gameData.SetChosenEnemies(chosenEnemyCharacters);
            SceneManager.LoadScene("Battle");
        }
    }

    public void SetConfirmButton() {
        confirmButton.interactable = CheckReady();
    }

    public bool CheckReady() {
        return chosenEnemyCharacters.Count > 0 && chosenEnemyCharacters.Count <= maxCharacters && chosenPlayerCharacters.Count > 0 && chosenPlayerCharacters.Count <= maxCharacters;
    }
}
