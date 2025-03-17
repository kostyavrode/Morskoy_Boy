using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LanguageSelector : MonoBehaviour
{
    [SerializeField] private GameObject languagePopup; // Всплывающее окно
    [SerializeField] private Button englishButton;
    [SerializeField] private Button portugueseButton;

    [SerializeField] private TMP_Text[] textElements; // Все текстовые элементы
    [SerializeField] private string[] englishTexts; // Тексты на английском
    [SerializeField] private string[] portugueseTexts; // Тексты на португальском

    private void Start()
    {
        // Проверяем, есть ли уже сохраненный язык
        if (!PlayerPrefs.HasKey("SelectedLanguage"))
        {
            languagePopup.SetActive(true);
        }
        else
        {
            SetLanguage(PlayerPrefs.GetString("SelectedLanguage"));
        }

        englishButton.onClick.AddListener(() => SelectLanguage("English"));
        portugueseButton.onClick.AddListener(() => SelectLanguage("Portuguese"));
    }

    private void SelectLanguage(string language)
    {
        PlayerPrefs.SetString("SelectedLanguage", language);
        PlayerPrefs.Save();

        SetLanguage(language);
        languagePopup.SetActive(false);
    }

    private void SetLanguage(string language)
    {
        string[] selectedTexts = language == "Portuguese" ? portugueseTexts : englishTexts;

        for (int i = 0; i < textElements.Length; i++)
        {
            if (i < selectedTexts.Length)
            {
                textElements[i].text = selectedTexts[i];
            }
        }
    }
}