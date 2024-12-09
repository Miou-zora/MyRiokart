using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor;

public class LogoSelector : MonoBehaviour
{
    public List<RectTransform> logos;
    public float normalScale = 1.0f;
    public float selectedScale = 1.2f;
    private int selectedIndex = 0;
    public SceneAsset[] coursesScenes;

    void Start()
    {
        UpdateLogoSizes();
        AddClickListeners();
    }

    void Update()
    {
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
        if (selectedIndex < coursesScenes.Length)
        {
            SceneManager.LoadScene(coursesScenes[selectedIndex].name);
        }
        else
        {
            Debug.LogWarning("No scene provided for selected icon");
        }
    }

    void AddClickListeners()
    {
        for (int i = 0; i < logos.Count; i++)
        {
            int index = i; // Capture l'index pour la fermeture du contexte
            Button button = logos[i].GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => OnLogoClicked(index));
            }
        }
    }

    void UpdateLogoSizes()
    {
        for (int i = 0; i < logos.Count; i++)
        {
            if (i == selectedIndex)
                logos[i].localScale = Vector3.one * selectedScale;
            else
                logos[i].localScale = Vector3.one * normalScale;
        }
    }

    public void OnLogoClicked(int index)
    {
        selectedIndex = index;
        UpdateLogoSizes();
        LoadSelectedScene();
    }
}
