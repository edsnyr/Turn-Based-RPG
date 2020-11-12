using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{

    GameData gameData;

    public Relic tempReward;

    public static AbilityTargets abilityTargets;
    public static ChangeMenu changeStateEvent;
    public static UnityEvent backEvent;

    public Canvas canvas;
    public BattleMenu battleMenu;
    public Button confirmButton;
    public Button backButton;
    public Character characterPrefab;
    public ResourceDisplay playerSliderPrefab;
    public ResourceDisplay enemySliderPrefab;

    public List<Vector3> playerLocations;
    public List<Vector3> enemyLocations;

    public static Alliance targeting;
    public static State state;
    public static Attack queuedAttack;
    public static Character queuedAttackCharacter;

    List<Character> characters;
    Character currentCharacter;
    Queue<Character> characterQueue;

    private void Awake() {
        gameData = FindObjectOfType<GameData>();
        if(abilityTargets == null) { abilityTargets = new AbilityTargets(); }
        if(changeStateEvent == null) { changeStateEvent = new ChangeMenu(); }
        if(backEvent == null) { backEvent = new UnityEvent(); }
        characterQueue = new Queue<Character>();
        changeStateEvent.AddListener(ChangeState);
        changeStateEvent.AddListener(ModifyButtons);
        state = State.Move;
        battleMenu.gameObject.SetActive(false);
        InitializeButtons();
    }

    private void Start() {
        CreateCharacters();
        NextCharacter();
    }

    private void InitializeButtons() {
        confirmButton.onClick.AddListener(() => changeStateEvent.Invoke(State.Combat));
        confirmButton.gameObject.SetActive(false);
        backButton.onClick.AddListener(() => backEvent.Invoke());
        backButton.gameObject.SetActive(false);
    }

    private void CreateCharacters() {
        characters = new List<Character>();
        List<CharacterStats> cs = gameData.GetChosenCharacters();
        for(int i = 0; i < cs.Count; i++) {
            Debug.Log("Create a character " + cs[i].name);
            Character newChar = Instantiate(characterPrefab);
            newChar.InitializeCharacter(cs[i], Alliance.Ally);

            ResourceDisplay rd = Instantiate(playerSliderPrefab, canvas.transform);
            newChar.AddHealthDisplays(rd);
            PlaceDisplay(newChar, rd, i);

            gameData.ApplyCharacterProgression(newChar);
            battleMenu.BuildPlayerAttackMenu(newChar);
            battleMenu.BuildPlayerRelicMenu(newChar);
            PlaceCharacter(newChar, i);


            characters.Add(newChar);
            characterQueue.Enqueue(newChar);
        }
        cs = gameData.GetChosenEnemies();
        for(int i = 0; i < cs.Count; i++) {
            Debug.Log("Create a character " + cs[i].name);
            Character newChar = Instantiate(characterPrefab);
            newChar.InitializeCharacter(cs[i], Alliance.Enemy);
            PlaceCharacter(newChar, i);

            ResourceDisplay rd = Instantiate(enemySliderPrefab, canvas.transform);
            newChar.AddHealthDisplays(rd);
            PlaceDisplay(newChar, rd, i);

            newChar.ApplyDefaultStats();
            
            characters.Add(newChar);
            characterQueue.Enqueue(newChar);
        }
    }

    private void PlaceCharacter(Character character, int i) {
        if(character.alliance == Alliance.Ally) {
            if(playerLocations.Count <= i) {
                Debug.Log("No location Data");
                character.transform.position = Vector3.zero;
                return;
            }
            character.transform.position = playerLocations[i];
        }
        if(character.alliance == Alliance.Enemy) {
            if(enemyLocations.Count <= i) {
                Debug.Log("No location Data");
                character.transform.position = Vector3.zero;
                return;
            }
            character.transform.position = enemyLocations[i];
        }
    }

    private void PlaceDisplay(Character character, ResourceDisplay rd, int count) {
        if(character.alliance == Alliance.Ally) {
            RectTransform rt = rd.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0, 1);
            rt.anchorMax = new Vector2(0, 1);
            rt.anchoredPosition = new Vector2(20, -20 - (35 * count));
        } else {
            Vector3 pos = character.transform.position;
            pos = new Vector3(pos.x, pos.y - 1, pos.z);
            Vector2 canvasPos;
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(pos);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPoint, null, out canvasPos);
            rd.transform.localPosition = canvasPos;
        }
    }

    private void NextCharacter() {
        currentCharacter = characterQueue.Dequeue();
        characterQueue.Enqueue(currentCharacter);
        if(currentCharacter.GetAlive())
            changeStateEvent.Invoke(State.Move);
        else
            NextCharacter();
    }


    private void ChangeState(State newState) {
        switch (newState) {
            case State.Move:
                StartCoroutine(CharacterMove());
                break;
            case State.Target:
                StartCoroutine(CharacterTargetSelect());
                break;
            case State.Confirm:
                StartCoroutine(ConfirmAction());
                break;
            case State.Combat:
                StartCoroutine(ResolveCombat());
                break;
            case State.End:
                StartCoroutine(EndPhase());
                break;
            default:
                Debug.Log("Invalid State");
                break;
        }
    }

    private void ModifyButtons(State state) {
        switch(state) {
            case State.Move:
                confirmButton.gameObject.SetActive(false);
                backButton.gameObject.SetActive(false);
                break;
            case State.Target:
                confirmButton.gameObject.SetActive(false);
                backButton.gameObject.SetActive(true);
                backEvent.RemoveAllListeners();
                backEvent.AddListener(() => changeStateEvent.Invoke(State.Move));
                break;
            case State.Confirm:
                confirmButton.gameObject.SetActive(true);
                backButton.gameObject.SetActive(true);
                backEvent.RemoveAllListeners();
                backEvent.AddListener(() => abilityTargets.RemoveAllListeners());
                backEvent.AddListener(() => changeStateEvent.Invoke(State.Target));
                break;
            case State.Combat:
                confirmButton.gameObject.SetActive(false);
                backButton.gameObject.SetActive(false);
                break;
            case State.End:
                confirmButton.gameObject.SetActive(false);
                backButton.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }


    private IEnumerator CharacterMove() {
        if(currentCharacter.alliance == Alliance.Ally) {
            StartCoroutine(DisplayBattleMenu());
        } else if(currentCharacter.alliance == Alliance.Enemy) {
            StartCoroutine(SelectEnemyAction());
        }
        yield return null;
    }

    private IEnumerator DisplayBattleMenu() {
        state = State.Move;
        battleMenu.gameObject.SetActive(true);
        battleMenu.SetAttackMenu(currentCharacter, backButton);
        battleMenu.SetRelicMenu(currentCharacter, backButton);
        yield return null;
    }

    private IEnumerator SelectEnemyAction() {
        List<Attack> attacks = currentCharacter.GetAttacks();
        queuedAttack = attacks[Random.Range(0, attacks.Count)];
        queuedAttackCharacter = currentCharacter;
        changeStateEvent.Invoke(State.Target);
        yield return null;
    }

    private IEnumerator CharacterTargetSelect() {
        if(currentCharacter.alliance == Alliance.Ally) {
            StartCoroutine(PlayerTargetSelect());
        } else if(currentCharacter.alliance == Alliance.Enemy) {
            StartCoroutine(EnemyTargetSelect());
        }
        yield return null;
    }

    private IEnumerator PlayerTargetSelect() {
        currentCharacter.attackMenu.gameObject.SetActive(false);
        state = State.Target;
        yield return null;
    }

    private IEnumerator EnemyTargetSelect() {
        Alliance targetAlliance;
        if(queuedAttack.targeting == Alliance.Enemy) {
            targetAlliance = Alliance.Ally;
        } else if(queuedAttack.targeting == Alliance.Ally) {
            targetAlliance = Alliance.Enemy;
        } else {
            targetAlliance = Alliance.Neutral;
        }

        List<Character> targets = new List<Character>();
        if(targetAlliance == Alliance.Neutral) {
            targets = characters;
        } else {
            foreach(Character character in characters) {
                if(character.alliance == targetAlliance && character.GetAlive()) {
                    targets.Add(character);
                }
            }
        }

        Character target = targets[Random.Range(0, targets.Count)];
        abilityTargets.AddListener(target.ApplyAttack);
        changeStateEvent.Invoke(State.Combat);
        yield return null;
    }

    private IEnumerator ConfirmAction() {
        state = State.Confirm;
        yield return null;
    }

    private IEnumerator ResolveCombat() {
        state = State.Combat;
        abilityTargets.Invoke(queuedAttack, queuedAttackCharacter);
        abilityTargets.RemoveAllListeners();
        changeStateEvent.Invoke(State.End);
        yield return null;
    }


    private IEnumerator EndPhase() {
        if(CheckWin())
            NextCharacter();
        yield return null;
    }

    private bool CheckWin() {
        bool ally = false;
        bool enemy = false;
        foreach(Character character in characters) {
            if(character.GetAlive()) {
                if(character.alliance == Alliance.Ally)
                    ally = true;
                else
                    enemy = true;
            }
        }
        if(!ally) {
            StartCoroutine(Defeat());
            return false;
        } else if(!enemy) {
            StartCoroutine(Win());
            return false;
        } else {
            return true;
        }
    }

    private IEnumerator Defeat() {
        Debug.Log("Defeated");
        yield return null;
    }

    private IEnumerator Win() {
        Debug.Log("Victory");
        foreach(Character character in characters) {
            if(character.alliance == Alliance.Ally) {
                character.AddRelic(tempReward);
                gameData.UpdateCharacterProgression(character);
            }
        }
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("BattleCreator");
    }

}

public enum State { Move, Target, Confirm, Combat, End };
public class AbilityTargets : UnityEvent<Attack, Character> { }
public class ChangeMenu : UnityEvent<State> { }