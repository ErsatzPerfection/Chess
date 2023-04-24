using UnityEngine;

public class GlowControl : MonoBehaviour
{
    private Renderer[] renderers;
    private Color[] originalEmissionColors;
    private bool isGlowing;

    private void Awake()
    {
        InitializeGlow();
    }

    private void InitializeGlow()
    {
        renderers = GetComponentsInChildren<Renderer>();
        originalEmissionColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalEmissionColors[i] = renderers[i].material.GetColor("_EmissionColor");
        }
    }

    public void ToggleGlow(bool state)
    {
        if (isGlowing != state)
        {
            isGlowing = state;
            Color emissionColor = isGlowing ? Color.yellow * 0.5f : Color.black;

            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.SetColor("_EmissionColor", originalEmissionColors[i] + emissionColor);
            }
        }
    }
}