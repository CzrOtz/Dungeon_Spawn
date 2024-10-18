using UnityEngine;

public class fireballRender : MonoBehaviour
{
    private ParticleSystem fireParticleSystem; // For the continuous fire effect
    private testAgentScript boss;

    void Start()
    {
        // Get the fire particle system attached to this object
        fireParticleSystem = GetComponent<ParticleSystem>();
        boss = FindObjectOfType<testAgentScript>();

        if (fireParticleSystem == null)
        {
            Debug.LogWarning("Fire particle system not found on fireballRender.");
        }

        if (boss == null)
        {
            Debug.LogError("Boss not found on fireballRender.");
        }

        // Start the fire effect
        StartFireEffect();
    }

    void Update()
    {
        if (boss.dead)
        {
            StopFireEffect();
            HideSprites();
        }
    }

    public void StartFireEffect()
    {
        if (fireParticleSystem != null)
        {
            fireParticleSystem.Play();
        }
    }

    public void StopFireEffect()
    {
        if (fireParticleSystem != null)
        {
            fireParticleSystem.Stop();
        }
    }

    public void HideSprites()
    {
        // Disable all renderers (sprites)
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer renderer in renderers)
        {
            renderer.enabled = false;
        }
    }
}


