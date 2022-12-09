using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeCard : MonoBehaviour
{
    public float Fill { get; set; }
    public bool IsShining
    {
        get => _isShining;
        set
        {
            if (_isShining != value)
            {
                _isShining = value;
                //LeanTween.cancel(_barShine);
                LeanTween.cancel(_crystalShine);
                //LeanTween.alpha(_barShine, value ? 1f : 0f, 0.2f);
                LeanTween.alpha(_crystalShine, value ? 1f : 0f, 0.2f);
            }
        }
    }

    public void SetProgress(int current, int goal)
    {
        _text.text = $"{current}/{goal}";
    }

    public void SetBase(Sprite sprite) => _base.sprite = sprite;

    private void Update()
    {
        _smoothFill = Mathf.Lerp(_smoothFill, Fill, Time.deltaTime * 5f);
        
        SetBarFill(_barDark, _smoothFill);
        // SetBarFill(_barBright, _smoothFill);
        _barCap.anchoredPosition = new Vector3(128 * _smoothFill, 0);
    }

    private void SetBarFill(RectTransform bar, float fill) => bar.sizeDelta = new Vector2(128 * fill, 18);

    [SerializeField]
    private Image _base;
    [SerializeField]
    private RectTransform _barCap;
    [SerializeField]
    private RectTransform _barDark;
    [SerializeField]
    private RectTransform _barBright;
    [SerializeField]
    private RectTransform _barShine;
    [SerializeField]
    private RectTransform _crystalShine;
    [SerializeField]
    private TMP_Text _text;

    private float _smoothFill;
    private bool _isShining;
}
