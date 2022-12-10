using System;
using System.Collections;
using System.Collections.Generic;
using Potions.Global;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Potions.Level
{
    public class LevelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private void Awake()
        {
            _levelText = GetComponentInChildren<TMP_Text>();
            GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.Transitions.LoadLevel(_scene));
            int index = transform.GetSiblingIndex();
            _levelText.text = $"{index + 1}. {_name}";
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _levelText.fontStyle = FontStyles.Underline;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _levelText.fontStyle = FontStyles.Normal;
        }

        [SerializeField]
        private string _scene;
        [SerializeField]
        private string _name;
        private TMP_Text _levelText;
    }
}