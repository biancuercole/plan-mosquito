using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PointsManager : MonoBehaviour
{
    public static PointsManager Instance; 

    private int _points;
    public TextMeshProUGUI _textPoints;

    private const string PointsKey = "PlayerPoints";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantiene el objeto entre escenas
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Cargar puntos guardados
        _points = PlayerPrefs.GetInt(PointsKey, 0);
        // Si la escena actual es MainMenu, forzamos a 0 según la petición
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            _points = 0;
            PlayerPrefs.SetInt(PointsKey, 0);
            PlayerPrefs.Save();
        }
        UpdatePoints();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            // Reiniciar puntos cuando se entra al MainMenu
            _points = 0;
            PlayerPrefs.SetInt(PointsKey, 0);
            PlayerPrefs.Save();
            UpdatePoints();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            AddPoints(5);

        if (Input.GetKeyDown(KeyCode.O))
            SubtractPoints(5);

        if(Input.GetKeyDown(KeyCode.R))
            RestartPlayerPrefs();
    }

    public void AddPoints(int amount)
    {
        AudioManager.Instance._sfxSource.PlayOneShot(AudioManager.Instance._addPoints);
        _points += amount;
        PlayerPrefs.SetInt(PointsKey, _points); 
        PlayerPrefs.Save();
        UpdatePoints();
    }

    public void SubtractPoints(int amount)
    {
        AudioManager.Instance._sfxSource.PlayOneShot(AudioManager.Instance._subtractPoints);
        _points -= amount;
        PlayerPrefs.SetInt(PointsKey, _points);
        PlayerPrefs.Save();
        UpdatePoints();
    }

    public void UpdatePoints()
    {
        _textPoints.text = "PUNTOS: " + _points;
    }

    public void RestartPlayerPrefs()
    {
        // Borra únicamente la clave de puntos para no eliminar otras preferencias del juego
        if (PlayerPrefs.HasKey(PointsKey))
        {
            PlayerPrefs.DeleteKey(PointsKey);
            PlayerPrefs.Save();
        }
        _points = 0;
        UpdatePoints();
    }
}
