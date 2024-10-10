using UnityEngine;

public class volumeControlScript : MonoBehaviour
{
    // -------------------------------------------------
    // SPEAR SOUNDS
    // -------------------------------------------------
    [Header("Spear Sounds")]
    public spearScript spear; 
    public AudioSource spearAudioSource;  
    [Header("Spear Volume")]
    public float spearVolume = 1.0f;

    // -------------------------------------------------
    // SPEAR COLLISION SOUNDS (already in the hierarchy)
    // -------------------------------------------------
    [Header("Spear Collision Sounds")]
    public spearCollisionScript spearCollision; 
    public AudioSource enemyHitAudioSource;  
    public AudioSource wallHitAudioSource;  
    [Header("Spear Collision Volume")]
    public float enemyHitVolume = 1.0f;
    public float wallHitVolume = 1.0f;

    // -------------------------------------------------
    // CRAB SOUNDS (Prefab)
    // -------------------------------------------------
    [Header("Crab Sounds")]
    public crabScript crab;  
    public AudioSource crabDieAudioSource;  
    public AudioSource crabChargeAudioSource;  
    [Header("Crab Volume")]
    public float crabDieVolume = 1.0f;
    public float crabChargeVolume = 1.0f;

    // -------------------------------------------------
    // CYCLOPS SOUNDS (Prefab)
    // -------------------------------------------------
    [Header("Cyclops Sounds")]
    public cyclopsScript cyclops;  
    public AudioSource cyclopsAudioSource;  
    [Header("Cyclops Volume")]
    public float cyclopsVolume = 1.0f;

    // -------------------------------------------------
    // GHOST SOUNDS (Prefab)
    // -------------------------------------------------
    [Header("Ghost Sounds")]
    public ghostScript ghost;  
    public AudioSource ghostDieAudioSource;  
    public AudioSource ghostChargeAudioSource;  
    [Header("Ghost Volume")]
    public float ghostDieVolume = 1.0f;
    public float ghostChargeVolume = 1.0f;

    // -------------------------------------------------
    // MAIN CAMERA SOUND
    // -------------------------------------------------
    [Header("Main Camera Sound")]
    public AudioSource mainCameraAudioSource;  
    [Header("Main Camera Volume")]
    public float mainCameraVolume = 1.0f;

    void Start()
    {
        // Manually assign AudioSources using hardcoded references or via tags
        AssignAudioSources();
    }

    void AssignAudioSources()
    {
        // Spear AudioSource
        if (spear == null)
        {
            spear = GameObject.FindObjectOfType<spearScript>();
        }
        if (spear != null)
        {
            spearAudioSource = spear.GetComponent<AudioSource>();
        }

        // Spear Collision AudioSources
        if (spearCollision != null)
        {
            enemyHitAudioSource = spearCollision.enemyHitAudioSource;
            wallHitAudioSource = spearCollision.wallHitAudioSource;
        }

        // Crab AudioSources (Prefab)
        if (crab == null)
        {
            crab = FindObjectOfType<crabScript>();
        }
        if (crab != null)
        {
            crabDieAudioSource = crab.audioSource;
            crabChargeAudioSource = crab.audioSource2;
        }

        // Cyclops AudioSource (Prefab)
        if (cyclops == null)
        {
            cyclops = FindObjectOfType<cyclopsScript>();
        }
        if (cyclops != null)
        {
            cyclopsAudioSource = cyclops.GetComponent<AudioSource>();
        }

        // Ghost AudioSources (Prefab)
        if (ghost == null)
        {
            ghost = FindObjectOfType<ghostScript>();
        }
        if (ghost != null)
        {
            ghostDieAudioSource = ghost.audioSource;
            ghostChargeAudioSource = ghost.audioSource2;
        }

        // Main Camera AudioSource
        if (mainCameraAudioSource == null)
        {
            mainCameraAudioSource = Camera.main.GetComponent<AudioSource>();
        }
    }

    // Function to update all volumes dynamically
    void UpdateVolumes()
    {
        if (spearAudioSource != null)
            spearAudioSource.volume = spearVolume;

        if (enemyHitAudioSource != null)
            enemyHitAudioSource.volume = enemyHitVolume;

        if (wallHitAudioSource != null)
            wallHitAudioSource.volume = wallHitVolume;

        if (crabDieAudioSource != null)
            crabDieAudioSource.volume = crabDieVolume;

        if (crabChargeAudioSource != null)
            crabChargeAudioSource.volume = crabChargeVolume;

        if (cyclopsAudioSource != null)
            cyclopsAudioSource.volume = cyclopsVolume;

        if (ghostDieAudioSource != null)
            ghostDieAudioSource.volume = ghostDieVolume;

        if (ghostChargeAudioSource != null)
            ghostChargeAudioSource.volume = ghostChargeVolume;

        if (mainCameraAudioSource != null)
            mainCameraAudioSource.volume = mainCameraVolume;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVolumes();
    }
}

