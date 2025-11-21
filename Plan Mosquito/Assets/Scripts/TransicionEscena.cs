using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class TransicionEscena : MonoBehaviour
{
    public RectTransform panel;
    public float duration = 0.6f;
    public static TransicionEscena Instance;

    [Header("Fade Settings")]

    public float fadeDuration = 0.6f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (panel == null)
            Debug.LogError("Panel no asignado");
    }

    public void TransitionTo(string sceneName)
    {
        float width = panel.rect.width;

        // Arranca FUERA de la pantalla (izquierda)
        panel.anchoredPosition = new Vector2(-width, 0);

        // ENTRA al centro
        panel.DOAnchorPos(Vector2.zero, duration)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                SceneManager.LoadScene(sceneName);

                // SALE hacia la derecha
                panel.DOAnchorPos(new Vector2(width, 0), duration)
                    .SetEase(Ease.InCubic);
            });
    }
}