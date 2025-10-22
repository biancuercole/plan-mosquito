using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class Menu : MonoBehaviour
{
    private string[] levelNames = new string[3]
    {
        "NivelCasas",
        "NivelPatio",
        "NivelBaldes"
    };

    private static HashSet<string> playedLevels = new HashSet<string>();

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void CorrectAnswer(string theme)
    {
        SceneManager.LoadScene("CorrectAnswer" + theme);
    }

    public void WrongAnswer(string theme)
    {
        SceneManager.LoadScene("WrongAnswer" + theme);
    }

    public void ReturnMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void RandomLevel()
    {
        List<string> availableLevels = levelNames.Where(level => !playedLevels.Contains(level)).ToList();

        if (availableLevels.Count == 0)
        {
            Debug.Log("¡Todos los niveles completados! Cargando escena de Victoria.");
            playedLevels.Clear();
            SceneManager.LoadScene("Victory");
            return;
        }

        int randomIndex = Random.Range(0, availableLevels.Count);
        string selectedLevel = availableLevels[randomIndex];

        playedLevels.Add(selectedLevel);

        Debug.Log($"Cargando nivel: {selectedLevel}. Niveles jugados: {playedLevels.Count}/{levelNames.Length}");

        SceneManager.LoadScene(selectedLevel);
    }

    public void ResetPlayedLevels()
    {
        playedLevels.Clear();
        Debug.Log("Lista de niveles jugados reiniciada.");
    }

    public void ShowProgress()
    {
        Debug.Log($"Niveles jugados en esta sesión: {playedLevels.Count}/{levelNames.Length}");
        if (playedLevels.Count > 0)
        {
            Debug.Log($"Niveles completados: {string.Join(", ", playedLevels)}");
        }
    }
}
