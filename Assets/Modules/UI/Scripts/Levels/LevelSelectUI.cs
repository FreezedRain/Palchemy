using UnityEngine;

public class LevelSelectUI : MonoBehaviour
{
    public bool IsActive { get; private set; }

    public void Show()
    {
        _contents.anchoredPosition = new Vector3(0, -1000f, 0f);
        LeanTween.cancel(_contents);
        _contents.gameObject.SetActive(true);
        LeanTween.moveY(_contents, 0, 0.35f)
            .setEaseOutCubic();
        IsActive = true;
    }

    public void Hide()
    {
        _contents.transform.position = Vector3.zero;
        LeanTween.cancel(_contents);
        LeanTween.moveY(_contents, -1000f, 0.35f)
            .setEaseInCubic()
            .setOnComplete(() => _contents.gameObject.SetActive(false));
        IsActive = false;
    }

    [SerializeField] private RectTransform _contents;
}