using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleMenu : MonoBehaviour
{

    public Button attackButton;
    public Button relicButton;
    public Canvas canvas;
    public Image relicMenuBackground;
    public MenuButton menuButtonPrefab;
    public RelicDisplay relicDisplayPrefab;

    public float spaceBetweenButtons = 5;
    public float spaceBetweenRelics = 5;

    public void SetAttackMenu(Character character, Button backButton) {
        attackButton.onClick.RemoveAllListeners();
        attackButton.onClick.AddListener(() => character.attackMenu.SetActive(true));
        attackButton.onClick.AddListener(() => gameObject.SetActive(false));
        attackButton.onClick.AddListener(() => backButton.gameObject.SetActive(true));
        attackButton.onClick.AddListener(() => SetBackToMenu(character));
    }

    public void SetRelicMenu(Character character, Button backButton) {
        relicButton.onClick.RemoveAllListeners();
        relicButton.onClick.AddListener(() => relicMenuBackground.gameObject.SetActive(true));
        relicButton.onClick.AddListener(() => character.relicMenu.SetActive(true));
        relicButton.onClick.AddListener(() => gameObject.SetActive(false));
        relicButton.onClick.AddListener(() => backButton.gameObject.SetActive(true));
        relicButton.onClick.AddListener(() => SetBackToMenu(character));
    }

    private void SetBackToMenu(Character character) {
        BattleManager.backEvent.RemoveAllListeners();
        BattleManager.backEvent.AddListener(() => character.attackMenu.SetActive(false));
        BattleManager.backEvent.AddListener(() => character.relicMenu.SetActive(false));
        BattleManager.backEvent.AddListener(() => relicMenuBackground.gameObject.SetActive(false));
        BattleManager.backEvent.AddListener(() => BattleManager.changeStateEvent.Invoke(State.Move));
    }

    public void BuildPlayerAttackMenu(Character character) {
        List<Attack> attacks = character.GetAttacks();
        float separation = menuButtonPrefab.GetComponent<RectTransform>().rect.height;
        character.attackMenu = new GameObject(character.name + "'s Attack Menu", typeof(RectTransform));
        character.attackMenu.transform.SetParent(canvas.transform, false);
        character.attackMenu.transform.localPosition = Vector3.zero;
        for(int i = 0; i < attacks.Count; i++) {
            MenuButton menuButton = Instantiate(menuButtonPrefab, character.attackMenu.transform);
            menuButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, i * (separation+spaceBetweenButtons) * -1);
            Attack attack = attacks[i];
            menuButton.SetNameField(attack.name);
            menuButton.SetValueField(attack.value);
            menuButton.GetComponent<Button>().onClick.AddListener(() => BattleManager.targeting = attack.targeting);
            menuButton.GetComponent<Button>().onClick.AddListener(() => BattleManager.queuedAttack = attack);
            menuButton.GetComponent<Button>().onClick.AddListener(() => BattleManager.queuedAttackCharacter = character);
            menuButton.GetComponent<Button>().onClick.AddListener(() => BattleManager.changeStateEvent.Invoke(State.Target));
        }
        character.attackMenu.SetActive(false);
    }

    public void BuildPlayerRelicMenu(Character character) {
        List<Relic> relics = character.GetRelics();
        character.relicMenu = new GameObject("Relic Menu", typeof(RectTransform));
        RectTransform rt = character.relicMenu.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(1, 1);
        rt.sizeDelta = new Vector2(0, 0);
        character.relicMenu.name = character.name + "'s Relic Menu";
        character.relicMenu.transform.SetParent(relicMenuBackground.transform, false);
        character.relicMenu.transform.localPosition = Vector3.zero;

        float dist = 0;
        for(int i = 0; i < relics.Count; i++) {
            RelicDisplay rd = Instantiate(relicDisplayPrefab);
            rd.BuildDisplay(relics[i], character.relicMenu.transform);
            //fix display position
            rd.transform.localPosition = new Vector3(0, dist, 0);
            dist -= rd.display.rect.height + spaceBetweenRelics;
        }

        character.relicMenu.SetActive(false);
    }
}
