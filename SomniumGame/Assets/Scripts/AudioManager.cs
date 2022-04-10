using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] public AudioSource backgroundMusicSource, chaseMusicSource, sfxSource;
    [SerializeField] private AudioClip bgMusic, chaseMusic;
    
    [SerializeField] private AudioClip[] sfx;
    // AudioSource chaseMusic;
    // AudioSource bgMusic;

    private bool anySuspicion;
    private bool isPlayingBGMusic;
    private bool isPlayingChaseMusic;

    private bool enemiesSet = false;

    private float suspicionTimer = 0.0f;
    private float currentEnemySusTimer = 0.0f;

    private int footstepCounter = 0;

    private List<GameObject> enemies = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // bgMusic.Play(0);
        // bgMusic.loop = true;

        // backgroundMusicSource.PlayOneShot(bgMusic);
        backgroundMusicSource.Play();
        sfxSource.Play();
        // backgroundMusicSource.loop = true;
        // Invoke("crossfade", 2);

        isPlayingBGMusic = true;
        isPlayingChaseMusic = false;
        
        backgroundMusicSource.PlayOneShot(chaseMusic);


    }

    // Update is called once per frame
    void Update()
    {
        if (!enemiesSet)
        {
            enemies = GameObject.Find("Level Builder").GetComponent<Builder>()._enemies;
            enemiesSet = true;
        }

        setAnySuspicion();
        setMaxSuspicionTime();

        crossfade();

        // if the enemy with the max suspicion timer is no longer suspicious, set anySuspicion to false
        if (Time.time > suspicionTimer)
            anySuspicion = false;
    }

    void FixedUpdate()
    {
        
    }


    private void crossfade()
    {
        float timeToFade = 2.0f;

        if (anySuspicion == true && isPlayingBGMusic)
        {
            chaseMusicSource.Play();

            StartCoroutine(FadeSrc.StartFade(backgroundMusicSource, timeToFade, 0f));
            StartCoroutine(FadeSrc.StartFade(chaseMusicSource, timeToFade, 0.3f));
            isPlayingBGMusic = !isPlayingBGMusic;
            isPlayingChaseMusic = !isPlayingChaseMusic;
        }
        else if (anySuspicion == false && isPlayingChaseMusic)
        {
            backgroundMusicSource.Play();

            StartCoroutine(FadeSrc.StartFade(backgroundMusicSource, timeToFade, 0.3f));
            StartCoroutine(FadeSrc.StartFade(chaseMusicSource, timeToFade, 0f));
            isPlayingBGMusic = !isPlayingBGMusic;
            isPlayingChaseMusic = !isPlayingChaseMusic;
        }
     }
    

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
        }
    }

    void setAnySuspicion()
    {
        // Once we find an instance where isInPursuit is true stop loop
        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<Stalker>()._isInPursuit  == true)
            {
                anySuspicion = true;
                float currentEnemySusTimer = enemy.GetComponent<Stalker>()._suspicionTimer;

                // sets the suspicion timer to that of the enemy with the highest suspicion timer
                if (suspicionTimer < currentEnemySusTimer)
                    suspicionTimer = currentEnemySusTimer;
            }
        }
    }

    void setMaxSuspicionTime()
    {
        // Once we find an instance where isInPursuit is true stop loop
        foreach (GameObject enemy in enemies)
        {
            
            // find the enemy with the max sus timer, update suspicionTimer to that time if a greater one found
            if (anySuspicion)
            {
                // checks current enemy inPursuit
                if (enemy.GetComponent<Stalker>()._isInPursuit  == true)
                {
                    currentEnemySusTimer = enemy.GetComponent<Stalker>()._suspicionTimer;

                    // sets the suspicion timer to that of the enemy with the highest suspicion timer
                    if (suspicionTimer < currentEnemySusTimer)
                        suspicionTimer = currentEnemySusTimer;
                }
            }
        }
    }

    public void playSound(int i)
    {
        sfxSource.PlayOneShot(sfx[i]);
    }

    public void playFootstep()
    {
        // sfxSource.PlayOneShot(footsteps[footstepCounter % 4]);
        footstepCounter++;
    }
}
