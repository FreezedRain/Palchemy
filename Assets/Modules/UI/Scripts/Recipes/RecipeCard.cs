using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeCard : MonoBehaviour
{
    public RectTransform barfill;
    public Image crystalShine;
    public RectTransform barcap;
    public Image mainSprite;

    public void SetSprite(Sprite sprite) => mainSprite.sprite = sprite;

    public void SetShine(bool shine)
    {
        LeanTween.alpha(crystalShine.rectTransform, shine ? 1f : 0f, 0.2f);
    }

    public void SetFill(float amountFill)
    {
        barfill.sizeDelta = new Vector2(128 * amountFill, 18);
        barcap.anchoredPosition = new Vector3(128 * amountFill, 0);

        // if (amountFill >= 1 && !shining)
        // {
        //     LeanTween.color(crystalShine.rectTransform, Color.white, 0.2f);
        //     shining = true;
        // }
        //
        // if (amountFill <= 1 && shining)
        // {
        //     LeanTween.color(crystalShine.rectTransform, new Color(1, 1, 1, 0), 0.2f);
        //     shining = false;
        // }
    }

}
