using UnityEngine;
using TMPro;
public class ChangeVictoryText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _victoryText;
    [SerializeField] private TextMeshProUGUI _tryAgainText;

    [SerializeField] private PointsManager _pointsManager;

    private float _currentPoints;
    void Start()
    {
        _currentPoints = PlayerPrefs.GetInt("PlayerPoints");

        Debug.Log("Points:" +  _currentPoints);

        if (_currentPoints <= 0)
        {
           _tryAgainText.gameObject.SetActive(true);
            _victoryText.gameObject.SetActive(false);
        }
        else
        {
            _tryAgainText.gameObject.SetActive(false);
            _victoryText.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
