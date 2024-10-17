using UnityEngine;

public class fireBallVisualScript : MonoBehaviour
{
    private ParticleSystem particles;
    private AudioSource audioSource;

    void Start()
    {
        // Get the ParticleSystem and AudioSource components attached to this GameObject
        particles = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();

        // Check if the components are correctly assigned
        if (particles == null)
        {
            Debug.LogWarning("ParticleSystem component not found on this GameObject.");
        }

        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource component not found on this GameObject.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // This could contain any logic to trigger particles/sound automatically
    }

    // Function to play particles
    public void Particles()
    {
        if (particles != null)
        {
            particles.Play();
        }
        else
        {
            Debug.LogWarning("No ParticleSystem to play.");
        }
    }

    // Function to play sound
    public void Sound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No AudioSource to play.");
        }
    }
}

