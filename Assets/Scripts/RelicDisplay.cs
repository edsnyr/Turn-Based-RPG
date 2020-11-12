using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RelicDisplay : MonoBehaviour
{

    public static float baseHeight = 50;
    public static float textSize = 26;

    public RectTransform display;
    public Image relicImage;
    public TextMeshProUGUI relicName;
    public TextMeshProUGUI relicModifier;


    public void BuildDisplay(Relic relic, Transform parent) {
        /* here> */
        transform.SetParent(parent);
        display.sizeDelta = new Vector2(0, baseHeight * relic.modifiers.Count);
        relicImage.sprite = relic.sprite;
        relicName.text = relic.name;

        for(int i = 0; i < relic.modifiers.Count; i++) {
            TextMeshProUGUI newModifier = Instantiate(relicModifier);
            newModifier.transform.SetParent(display.transform);
            newModifier.rectTransform.anchoredPosition = new Vector3(relicModifier.rectTransform.anchoredPosition.x, baseHeight * i * -1, 0);

            string sign;
            string text;
            if(relic.modifiers[i].buff == Buff.Offense) {
                sign = (relic.modifiers[i].value < 0 ? "-" : "+");
                text = " Damage Dealt with " + relic.modifiers[i].element;
            } else {
                sign = (relic.modifiers[i].value < 0 ? "+" : "-");
                text = " Damage Taken by " + relic.modifiers[i].element;
            }
            newModifier.text = sign + Mathf.Abs(relic.modifiers[i].value) + text;
        }

        Destroy(relicModifier.gameObject);
    }
}
