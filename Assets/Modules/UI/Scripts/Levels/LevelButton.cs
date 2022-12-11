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
    public class LevelButton : MonoBehaviour, IPointerEnterHandler
    {
        private void Awake()
        {
            _levelText = GetComponentInChildren<TMP_Text>();
            GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.Transitions.LoadLevel(_scene));
            int index = transform.GetSiblingIndex();
            if (index == 0)
            {
                LeanTween.cancel(gameObject);
                LeanTween.delayedCall(gameObject, 0.2f, () => EventSystem.current.SetSelectedGameObject(gameObject));
            }
            // _levelText.text = $"{index + 1}. {_name}";
            _levelText.text = $"{_name}";

        }

        private void Start()
        {
            SetCompleted(GameManager.Instance.SaveData.CompletedLevels.Contains(_scene));
        }

        private void Update()
        {
            if (EventSystem.current.currentSelectedGameObject == gameObject)
            {
                _levelText.fontStyle = FontStyles.Underline;
            }
            else
            {
                _levelText.fontStyle = FontStyles.Normal;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // _levelText.fontStyle = FontStyles.Underline;
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // _levelText.fontStyle = FontStyles.Normal;
        }

        private void SetCompleted(bool completed)
        {
            _completedTick.SetActive(completed);
            _uncompletedTick.SetActive(!completed);
        }

        [SerializeField]
        private string _scene;
        [SerializeField]
        private string _name;
        [SerializeField]
        private GameObject _completedTick;
        [SerializeField]
        private GameObject _uncompletedTick;
        private TMP_Text _levelText;
    }
}