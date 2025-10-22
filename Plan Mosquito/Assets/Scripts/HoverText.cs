using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class HoverText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Text Settings")]
    [SerializeField] private GameObject textObject;
    [SerializeField] private string hoverMessage = "Texto de hover";

    [Header("Auto Level Name Settings")]
    [SerializeField] private bool useLevelNamesFromMenu = true;
    [SerializeField] private int levelIndex = 0;

    [Header("Optional: Text Component Reference")]
    [SerializeField] private Text legacyTextComponent;
    [SerializeField] private TextMeshProUGUI tmpTextComponent;

    [Header("Detection Method")]
    [SerializeField] private bool useUIEvents = true;
    [SerializeField] private bool debugMode = false;

    private bool isHovering = false;
    private Menu menuScript;

    //agrandar imag

    [SerializeField] private Button _button;
    [SerializeField] private Vector3 _scale = new Vector3(1.2f, 1.2f, 1.2f);
    private Vector3 _normalScale;

    void Start()
    {
        if (textObject != null)
        {
            textObject.SetActive(false);
        }

        if (useUIEvents)
        {
            Canvas canvas = GetComponentInParent<Canvas>();
            if (canvas != null)
            {
                if (canvas.GetComponent<GraphicRaycaster>() == null)
                {
                    if (debugMode) Debug.LogWarning($"HoverText en {gameObject.name}: Canvas necesita GraphicRaycaster para eventos UI.");
                }
            }

            if (GetComponent<Image>() == null && GetComponent<Button>() == null && GetComponent<Text>() == null)
            {
                if (debugMode) Debug.LogWarning($"HoverText en {gameObject.name}: Necesita un componente Image, Button o Text para recibir eventos UI.");
            }
        }
        else
        {
            if (GetComponent<Collider>() == null && GetComponent<Collider2D>() == null)
            {
                if (debugMode) Debug.LogWarning($"HoverText en {gameObject.name}: Se necesita un Collider o Collider2D para detectar el hover del mouse.");
            }
        }

        if (debugMode) Debug.Log($"HoverText configurado en {gameObject.name} - Modo UI: {useUIEvents}");

        _normalScale = _button.transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (useUIEvents && !isHovering)
        {
            if (debugMode) Debug.Log($"UI Hover Enter en {gameObject.name}");
            ShowText();
            isHovering = true;
        }

        if (_button.interactable)
            _button.transform.localScale = _scale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (useUIEvents && isHovering)
        {
            if (debugMode) Debug.Log($"UI Hover Exit en {gameObject.name}");
            HideText();
            isHovering = false;
        }

        _button.transform.localScale = _normalScale;
    }

    void OnMouseEnter()
    {
        if (!useUIEvents && !isHovering)
        {
            if (debugMode) Debug.Log($"Mouse Enter en {gameObject.name}");
            ShowText();
            isHovering = true;
        }
    }

    void OnMouseExit()
    {
        if (!useUIEvents && isHovering)
        {
            if (debugMode) Debug.Log($"Mouse Exit en {gameObject.name}");
            HideText();
            isHovering = false;
        }
    }

    private void ShowText()
    {
        if (textObject != null)
        {
            textObject.SetActive(true);
            if (debugMode) Debug.Log($"Mostrando texto: {hoverMessage}");

            UpdateTextContent();
        }
        else
        {
            if (debugMode) Debug.LogWarning($"textObject no asignado en {gameObject.name}");
        }
    }

    private void HideText()
    {
        if (textObject != null)
        {
            textObject.SetActive(false);
            if (debugMode) Debug.Log($"Ocultando texto en {gameObject.name}");
        }
    }

    private void UpdateTextContent()
    {
        if (legacyTextComponent == null && tmpTextComponent == null && textObject != null)
        {
            legacyTextComponent = textObject.GetComponent<Text>();
            tmpTextComponent = textObject.GetComponent<TextMeshProUGUI>();

            if (legacyTextComponent == null && tmpTextComponent == null)
            {
                legacyTextComponent = textObject.GetComponentInChildren<Text>();
                tmpTextComponent = textObject.GetComponentInChildren<TextMeshProUGUI>();
            }

            if (debugMode && legacyTextComponent == null && tmpTextComponent == null)
            {
                Debug.LogWarning($"No se encontró componente de texto en {textObject.name}");
            }
        }

        if (legacyTextComponent != null)
        {
            legacyTextComponent.text = hoverMessage;
            if (debugMode) Debug.Log($"Texto actualizado (UI Text): {hoverMessage}");
        }

        if (tmpTextComponent != null)
        {
            tmpTextComponent.text = hoverMessage;
            if (debugMode) Debug.Log($"Texto actualizado (TextMeshPro): {hoverMessage}");
        }
    }

    public void SetHoverMessage(string newMessage)
    {
        hoverMessage = newMessage;
        if (isHovering && textObject != null && textObject.activeInHierarchy)
        {
            UpdateTextContent();
        }
    }

    public void SetTextObject(GameObject newTextObject)
    {
        if (textObject != null && textObject.activeInHierarchy)
        {
            textObject.SetActive(false);
        }

        textObject = newTextObject;

        if (isHovering && textObject != null)
        {
            ShowText();
        }
    }

    public void ForceShowText()
    {
        ShowText();
        isHovering = true;
    }

    public void ForceHideText()
    {
        HideText();
        isHovering = false;
    }


    // agrandar imag


}