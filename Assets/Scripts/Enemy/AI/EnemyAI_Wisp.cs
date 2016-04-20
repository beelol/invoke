using UnityEngine;
using System.Collections;

public class EnemyAI_Wisp : EnemyAI {

    // Smoothly fade out the light if dead.
    private bool fadeLight;

    // Light source.
    private Light light;

    // How long the wisp will last after it dies.
    private float lifetime = 5;

    public override void Die()
    {
        base.Die();

        DisableVisuals();

        Invoke("Destroy", lifetime);
    }

    private void DisableVisuals()
    {
        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
        foreach(ParticleSystem pS in particles)
        {
            pS.Stop();
        }

        light = GetComponentInChildren<Light>();

        fadeLight = true;
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    void Update()
    {
        if (fadeLight)
        {
            light.intensity -= 0.05f;
        }
    }
}
