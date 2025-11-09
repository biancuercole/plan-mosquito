using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sources")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] public AudioSource _sfxSource;

    [Header("Music")]
    public AudioClip[] _gameMusic;

    [Header("Sounds")]
    public AudioClip _spray;
    public AudioClip _damage;
    public AudioClip _addPoints;
    public AudioClip _subtractPoints;
    public AudioClip _buttonHover;
    void Awake()
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
    }
    void Start()
    {
        
    }

}
