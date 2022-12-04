using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Potions.Gameplay
{
    [RequireComponent(typeof(ItemHolder))]
    public class ItemGhost : MonoBehaviour
    {
        public void Setup(string id)
        {
            GetComponent<ItemHolder>().SetItem(id);
            LeanTween.moveLocalY(gameObject, transform.position.y + 0.15f, 0.6f).setEaseOutCubic();
            LeanTween.alpha(gameObject, 0, 0.5f);
            LeanTween.delayedCall(gameObject, 0.25f, () => Destroy(gameObject));
        }
    }
}
