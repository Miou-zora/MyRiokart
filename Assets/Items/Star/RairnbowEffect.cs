using UnityEngine;

public class RainbowEffect : MonoBehaviour
{
    public Material rainbowMaterial; // Matériau avec l'effet arc-en-ciel

    private Material[] originalMaterials;
    SaltoAndAlign saltoAndAlign;

    void Start()
    {
        saltoAndAlign = GetComponent<SaltoAndAlign>();
    }

    public void TemporarilyApplyRainbowEffect(float duration)
    {
        ApplyRainbowEffect();
        Invoke("RemoveRainbowEffect", duration);
    }

    public void ApplyRainbowEffect()
    {
        if (saltoAndAlign != null)
        {
            saltoAndAlign.enabled = false;
        }
        // Stocker les matériaux d'origine pour les restaurer plus tard
        var renderers = GetComponentsInChildren<Renderer>();
        originalMaterials = new Material[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].material;
            // duplicate the material to avoid changing the original material
            // renderers[i].material = new Material(rainbowMaterial);
            renderers[i].material = rainbowMaterial; // Appliquer le matériau arc-en-ciel
            renderers[i].material.SetTexture("_BaseMap", originalMaterials[i].GetTexture("_MainTex"));
        }
    }

    public void RemoveRainbowEffect()
    {
        // Restaurer les matériaux d'origine
        if (saltoAndAlign != null)
        {
            saltoAndAlign.enabled = true;
        }
        var renderers = GetComponentsInChildren<Renderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            if (i < originalMaterials.Length)
            {
                renderers[i].material = originalMaterials[i];
            }
        }
    }
}
