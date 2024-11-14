using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LogoSelector : MonoBehaviour
{
    public List<RectTransform> logos; // Ajoute ici tes logos dans l'ordre
    public float normalScale = 1.0f; // Taille normale des logos
    public float selectedScale = 1.2f; // Taille des logos sélectionnés
    private int selectedIndex = 0; // Index du logo actuellement sélectionné
    public string[] sceneNames;

    void Start()
    {
        UpdateLogoSizes();
    }

    void Update()
    {
        // Gérer les flèches directionnelles pour naviguer entre les logos
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectedIndex = (selectedIndex + 1) % logos.Count;
            UpdateLogoSizes();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectedIndex = (selectedIndex - 1 + logos.Count) % logos.Count;
            UpdateLogoSizes();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedIndex = (selectedIndex - 4 + logos.Count) % logos.Count;
            UpdateLogoSizes();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedIndex = (selectedIndex + 4) % logos.Count;
            UpdateLogoSizes();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            LoadSelectedScene();
        }
    }

    void LoadSelectedScene()
    {
        if (selectedIndex < sceneNames.Length)
        {
            SceneManager.LoadScene(sceneNames[selectedIndex]);
        }
        else
        {
            Debug.LogWarning("Aucune scène assignée pour ce logo !");
        }
    }

    void UpdateLogoSizes()
    {
        // Parcourir les logos et ajuster leur taille
        for (int i = 0; i < logos.Count; i++)
        {
            if (i == selectedIndex)
                logos[i].localScale = Vector3.one * selectedScale;
            else
                logos[i].localScale = Vector3.one * normalScale;
        }
    }
}
