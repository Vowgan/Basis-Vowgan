using Basis.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Cilboxable]
public class Teleprompter : MonoBehaviour
{

    public enum TeleprompterUiState
    {
        ScrollText,
        ChangeSpeed,
        FontSize,
    }

    public TeleprompterUiState uiState;

    [TextArea(3, 8)] public string text;

    [Header("Font Size")]
    public float fontSize = 20;
    public float fontSizeMin = 15;
    public float fontSizeMax = 50;
    public float fontSizeScrollSpeed = 10;

    [Header("Scroll Speed")]
    public float scrollSpeed = 200;
    public float scrollSpeedMin = 200;
    public float scrollSpeedMax = 1000;
    public float scrollSpeedScrollSpeed = 300;

    [Header("References")]
    public ScrollRect textView;
    public TMP_Text textLabel;
    public BoxCollider canvasCollider;
    public GraphicRaycaster raycaster;
    public Slider fontSizeSlider;
    public Slider scrollSpeedSlider;
    public TMP_InputField textInput;

    public GameObject scrollTextPanel;
    public GameObject changeSpeedPanel;
    public GameObject fontSizePanel;



    private void Awake()
    {
        RefreshUI();

        textInput.onValueChanged.AddListener(OnTextChanged);

        OnTextChanged(text);
    }

    private void OnTextChanged(string input)
    {
        text = input;
        textLabel.text = text;
        textInput.SetTextWithoutNotify(text);
    }


    public void ScrollText(bool reversed = false)
    {
        float speed = (1 / textView.content.rect.height) * scrollSpeed * Time.deltaTime * (reversed ? -1 : 1);
        textView.verticalScrollbar.value += speed;
    }

    public void ScrollSize(bool reversed = false)
    {
        fontSize += fontSizeScrollSpeed * Time.deltaTime * (reversed ? -1 : 1);
        fontSize = Mathf.Clamp(fontSize, fontSizeMin, fontSizeMax);

        RefreshUI();
    }

    public void ScrollSpeed(bool reversed = false)
    {
        scrollSpeed += scrollSpeedScrollSpeed * Time.deltaTime * (reversed ? -1 : 1);
        scrollSpeed = Mathf.Clamp(scrollSpeed, scrollSpeedMin, scrollSpeedMax);
        RefreshUI();
    }




    public void EnableRaycast(bool value)
    {
        canvasCollider.enabled = value;
        raycaster.enabled = value;
    }

    public void InterpretInput(bool reversed)
    {
        switch (uiState)
        {
            case TeleprompterUiState.ScrollText:
                ScrollText(reversed);
                break;
            case TeleprompterUiState.ChangeSpeed:
                ScrollSpeed(reversed);
                break;
            case TeleprompterUiState.FontSize:
                ScrollSize(reversed);
                break;
        }
    }

    public void NextState()
    {
        switch (uiState)
        {
            case TeleprompterUiState.ScrollText:
                SetState(TeleprompterUiState.ChangeSpeed);
                break;
            case TeleprompterUiState.ChangeSpeed:
                SetState(TeleprompterUiState.FontSize);
                break;
            case TeleprompterUiState.FontSize:
                SetState(TeleprompterUiState.ScrollText);
                break;
        }
    }

    public void SetState(TeleprompterUiState state)
    {
        uiState = state;
        RefreshUI();
    }

    private void RefreshUI()
    {
        fontSizeSlider.minValue = fontSizeMin;
        fontSizeSlider.maxValue = fontSizeMax;
        fontSizeSlider.value = fontSize;

        scrollSpeedSlider.minValue = scrollSpeedMin;
        scrollSpeedSlider.maxValue = scrollSpeedMax;
        scrollSpeedSlider.value = scrollSpeed;

        scrollTextPanel.SetActive(uiState == TeleprompterUiState.ScrollText);
        changeSpeedPanel.SetActive(uiState == TeleprompterUiState.ChangeSpeed);
        fontSizePanel.SetActive(uiState == TeleprompterUiState.FontSize);

        textLabel.fontSize = fontSize;
    }

}
