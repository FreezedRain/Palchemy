using System;
using System.Collections;
using System.Collections.Generic;
using Potions.Global;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Potions.Level
{
    public class LevelButton : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.Transitions.LoadLevel(_name));
            GetComponentInChildren<TMP_Text>().text = _name;
        }

        [SerializeField]
        private string _name;
    }
}