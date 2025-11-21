using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using DG.Tweening;


public class Menu : MonoBehaviour
{
    // Nombre de la escena especial que debe aparecer en la mitad
    private string hideSceneName = "HideFromRaid";

    // Lista blanca de niveles que SÍ deben entrar en el índice aleatorio
    private readonly List<string> allowedLevelNames = new List<string>
    {
        "NivelCasas",
        "NivelPatio",
        "NivelBaldes",
        "NivelRopa",
        "NivelNenes"
    };

    // Lista construida desde Build Settings (excluye MainMenu, Victory, CorrectAnswer*/WrongAnswer* y HideFromRaid)
    private List<string> playableLevels;

    private static HashSet<string> playedLevels = new HashSet<string>();

    // Controla si ya se mostró la escena intermedia en esta sesión (static para persistir aunque Menu se reinicie)
    private static bool hideShown = false;

    public RectTransform mosquito;

    private TransicionEscena transicion;

    void Awake()
    {
        // Obtener escenas desde Build Settings
        playableLevels = new List<string>();
        int sceneCount = SceneManager.sceneCountInBuildSettings;

        // Construir playableLevels como la intersección entre Build Settings y la lista blanca
        for (int i = 0; i < sceneCount; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = Path.GetFileNameWithoutExtension(path);
            if (string.IsNullOrEmpty(name))
                continue;

            // Excluir pantallas que no son niveles jugables
            if (name == "MainMenu" || name.StartsWith("Victory"))
                continue;
            if (name.StartsWith("CorrectAnswer") || name.StartsWith("WrongAnswer"))
                continue;
            if (name == hideSceneName)
                continue;

            // Sólo añadir si está en la lista blanca de niveles
            if (allowedLevelNames.Contains(name))
            {
                playableLevels.Add(name);
            }
        }

        // Avisar si alguna escena de la lista blanca no está en Build Settings
        var missing = allowedLevelNames.Where(n => !playableLevels.Contains(n)).ToList();
        if (missing.Count > 0)
        {
            Debug.LogWarning($"Las siguientes escenas de la lista blanca no están en Build Settings: {string.Join(", ", missing)}");
        }

        // Fallback si no se detectó nada en Build Settings
        if (playableLevels.Count == 0)
        {
            // Si por alguna razón no se detectaron niveles en Build Settings, usar la lista blanca como fallback
            playableLevels = new List<string>(allowedLevelNames);
            Debug.LogWarning("No se detectaron niveles válidos en Build Settings; usando lista blanca como fallback.");
        }

        transicion = FindObjectOfType<TransicionEscena>();

    }

    public void ChangeScene(string sceneName)
    {
        Debug.Log($"Menu.ChangeScene called with: {sceneName}");
        if (sceneName == "Victory")
        {
            Debug.Log($"Menu.ChangeScene(Victory) called. StackTrace:\n{StackTraceUtility.ExtractStackTrace()}");
        }
        SceneManager.LoadScene(sceneName);
    }
    public void CorrectAnswer(Transform target, Vector3 offset, string theme)
    {
        PointsManager.Instance.AddPoints(10);
        CorrectAnimation(target, offset, theme);
    }



    public void WrongAnswer(Transform target, Vector3 offset, string theme)
    {
        PointsManager.Instance.SubtractPoints(5);
        WrongAnimation(target, offset, theme);
    }
    public void CorrectAnimation(Transform target, Vector3 offset, string theme)
    {
        DOTween.Sequence()
            .Append(mosquito.DOMove(target.position, 0.6f)) // append es que espera a la anterior y join para uqe lo hagan a la vez
            .Join(mosquito.DOScale(0.2f, 0.5f)) //tamaño - tiempo 
            .Append(mosquito.DOScale(0.4f, 0.2f))
            .OnComplete(() => transicion.TransitionTo("CorrectAnswer" + theme));
    }

    public void WrongAnimation(Transform target, Vector3 offset, string theme)
    {
        DOTween.Sequence()
            .Append(mosquito.DOMove(target.position, 0.6f))
            .Join(mosquito.DOScale(0.2f, 0.5f))
            .Join(mosquito.DOShakePosition(0.4f, 0.3f))
            .OnComplete(() => transicion.TransitionTo("WrongAnswer" + theme));
    }

    public void ReturnMenu()
    {
        playedLevels.Clear();
        hideShown = false;
        PointsManager.Instance.RestartPlayerPrefs();
        transicion.TransitionTo("MainMenu");
    }

    public void RandomLevel()
    {
        // Niveles disponibles (excluyendo los ya jugados)
        List<string> availableLevels = playableLevels.Where(level => !playedLevels.Contains(level)).ToList();

        int totalPlayable = playableLevels.Count;
        int middlePosition = Mathf.CeilToInt(totalPlayable / 2f); // posición 1-based donde debe ir HideFromRaid

        // Si aún no mostramos HideFromRaid y estamos a punto de cargar el nivel medio, cargamos HideFromRaid
        if (!hideShown && playedLevels.Count == (middlePosition - 1))
        {
            hideShown = true;
            Debug.Log($"Mostrando escena intermedia: {hideSceneName}");
            SceneManager.LoadScene(hideSceneName);
            return;
        }

        if (availableLevels.Count == 0)
        {
            Debug.Log("¡Todos los niveles completados! Cargando escena de Victoria.");
            playedLevels.Clear();
            hideShown = false;
            transicion.TransitionTo("Victory");
            return;
        }

        int randomIndex = Random.Range(0, availableLevels.Count);
        string selectedLevel = availableLevels[randomIndex];

        playedLevels.Add(selectedLevel);

        Debug.Log($"Cargando nivel: {selectedLevel}. Niveles jugados: {playedLevels.Count}/{playableLevels.Count}");

        transicion.TransitionTo(selectedLevel);
    }

    // Llamar desde la escena HideFromRaid cuando el jugador la supere para continuar con el siguiente nivel aleatorio restante
    public void ContinueAfterHide()
    {
        List<string> availableLevels = playableLevels.Where(level => !playedLevels.Contains(level)).ToList();

        if (availableLevels.Count == 0)
        {
            Debug.Log("No quedan niveles después de Hide. Cargando Victory.");
            playedLevels.Clear();
            hideShown = false;
            transicion.TransitionTo("Victory");
            return;
        }

        int randomIndex = Random.Range(0, availableLevels.Count);
        string selectedLevel = availableLevels[randomIndex];
        playedLevels.Add(selectedLevel);

        Debug.Log($"Continuando después de Hide: Cargando nivel {selectedLevel}. Niveles jugados: {playedLevels.Count}/{playableLevels.Count}");
        transicion.TransitionTo(selectedLevel);
    }

    public void ResetPlayedLevels()
    {
        playedLevels.Clear();
        hideShown = false;
        PointsManager.Instance.RestartPlayerPrefs();
        Debug.Log("Lista de niveles jugados reiniciada.");
    }

    // Método público para iniciar un nuevo juego desde el MainMenu o un botón "Nuevo juego".
    // Reinicia puntos, niveles jugados y carga el MainMenu.
    public void StartNewGame()
    {
        Debug.Log("StartNewGame: Reiniciando progreso y puntos.");
        playedLevels.Clear();
        hideShown = false;
        if (PointsManager.Instance != null)
        {
            PointsManager.Instance.RestartPlayerPrefs();
        }
        else
        {
            Debug.LogWarning("StartNewGame: PointsManager.Instance es null. Asegúrate de que PointsManager esté presente y sea persistente.");
        }
        transicion.TransitionTo("MainMenu");
    }

    public void ShowProgress()
    {
        Debug.Log($"Niveles jugados en esta sesión: {playedLevels.Count}/{playableLevels.Count}");
        if (playedLevels.Count > 0)
        {
            Debug.Log($"Niveles completados: {string.Join(", ", playedLevels)}");
        }
    }

    // Llamar desde la escena Victory (botón) para cargar la siguiente escena según el orden de Build Settings
    public void LoadNextSceneByIndex()
    {
        // Si acabamos de venir del minijuego HideFromRaid (hideShown == true) preferimos continuar con la lógica de playableLevels
        // para evitar que el minijuego vuelva a aparecer.
        List<string> availableAfterHide = playableLevels.Where(level => !playedLevels.Contains(level)).ToList();
        if (hideShown && availableAfterHide.Count > 0)
        {
            Debug.Log("LoadNextSceneByIndex: hideShown==true, delegando a ContinueAfterHide() para cargar el siguiente nivel jugable.");
            ContinueAfterHide();
            return;
        }

        int current = SceneManager.GetActiveScene().buildIndex;
        int total = SceneManager.sceneCountInBuildSettings;
        Debug.Log($"LoadNextSceneByIndex called. currentIndex={current}, totalBuildScenes={total}");

        int next = current + 1;
        while (next < total)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(next);
            string name = Path.GetFileNameWithoutExtension(path);
            if (!string.IsNullOrEmpty(name))
            {
                Debug.Log($"LoadNextSceneByIndex: Loading scene at buildIndex={next} name={name}");

                // Si la escena que vamos a cargar es un nivel jugable, marcarla como jugada para evitar repeticiones
                if (playableLevels != null && playableLevels.Contains(name) && !playedLevels.Contains(name))
                {
                    playedLevels.Add(name);
                    Debug.Log($"LoadNextSceneByIndex: Marca {name} como jugado.");
                }

                // Si la escena siguiente es el minijuego HideFromRaid, marcar hideShown para que no vuelva a aparecer más tarde
                if (name == hideSceneName)
                {
                    hideShown = true;
                }

                SceneManager.LoadScene(next);
                return;
            }
            next++;
        }

        // Si no hay siguiente escena en build settings, volvemos al MainMenu
        Debug.Log("LoadNextSceneByIndex: No next scene in Build Settings, loading MainMenu");
        transicion.TransitionTo("MainMenu");
    }

}