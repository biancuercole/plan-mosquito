using System;
using TMPro;
using UnityEngine;

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
        _points = PlayerPrefs.GetInt(PointsKey, 0);
        UpdatePoints();
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
        _textPoints.text = "Points: " + _points;
    }

    public void RestartPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        _points = 0;
        UpdatePoints();
    }
}
