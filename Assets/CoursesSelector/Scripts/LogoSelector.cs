using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LogoSelector : MonoBehaviour
{
    public GameObject characterSelectionCanvas; // Canvas de s�lection des personnages
    public GameObject vehicleSelectionCanvas;
    public GameObject courseSelectionCanvas; // Canvas de s�lection des courses

    public enum MenuType { Character, Vehicle, Circuit }
    public MenuType currentMenu = MenuType.Character;

    public List<RectTransform> characters; // Liste des personnages (prefabs)
    public string[] characterNames; // Noms des personnages
    public TextMeshProUGUI characterNameText; // TextMeshPro pour afficher le nom du personnage
    private int selectedCharacterIndex = 0; // Index du personnage s�lectionn�
    public int columns = 2; // Nombre de colonnes pour les personnages
    public Image characterImageInVehicleMenu;
    public List<Image> characterImages;

    public List<GameObject> vehicles; // Liste des v�hicules (prefabs)
    public string[] vehicleNames; // Noms des v�hicules
    public TextMeshProUGUI vehicleNameText; // TextMeshPro pour afficher le nom du v�hicule
    private int selectedVehicleIndex = 0; // Index du v�hicule s�lectionn�
    public int vehicleColumns = 2;
    public List<Image> vehicleImages;
    public Image characterImageInCourseMenu;
    public Image vehicleImageInCourseMenu;

    public List<RectTransform> logos; // Ajoute ici tes logos dans l'ordre
    public float normalScale = 1.0f; // Taille normale des logos
    public float selectedScale = 1.2f; // Taille des logos s�lectionn�s
    private int selectedIndex = 0; // Index du logo actuellement s�lectionn�
    public string[] sceneNames;
 

    void Start()
    {
        UpdateLogoSizes();
        if (currentMenu == MenuType.Character)
        {
            UpdateCharacterSelection();
        }
        else if (currentMenu == MenuType.Vehicle)
        {
            UpdateVehicleSelection();
        }
    }

    void Update()
    {
        if (currentMenu == MenuType.Circuit)
        {
            HandleCircuitSelection();
        }
        else if (currentMenu == MenuType.Character)
        {
            HandleCharacterSelection();
        }
        else if (currentMenu == MenuType.Vehicle)
        {
            HandleVehicleSelection();
        }

        // Passer du menu des circuits au menu des personnages, puis du menu des personnages au menu des v�hicules
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchMenu();
        }
    }

    void HandleCircuitSelection()
    {
        // G�rer les fl�ches directionnelles pour naviguer entre les logos
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

    void HandleCharacterSelection()
    {
        // Navigation dans le menu des personnages
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectedCharacterIndex = (selectedCharacterIndex + 1) % characters.Count;
            UpdateCharacterSelection();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectedCharacterIndex = (selectedCharacterIndex - 1 + characters.Count) % characters.Count;
            UpdateCharacterSelection();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedCharacterIndex = (selectedCharacterIndex - columns + characters.Count) % characters.Count;
            UpdateCharacterSelection();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedCharacterIndex = (selectedCharacterIndex + columns) % characters.Count;
            UpdateCharacterSelection();
        }

        // Confirmer le choix du personnage
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Personnage s�lectionn� : " + characterNames[selectedCharacterIndex]);
            if (characterImages != null && selectedCharacterIndex < characterImages.Count)
            {
                characterImageInVehicleMenu.sprite = characterImages[selectedCharacterIndex].sprite;
                characterImageInCourseMenu.sprite = characterImages[selectedCharacterIndex].sprite;
            }
            SwitchMenu();
        }
    }

    void HandleVehicleSelection()
    {
        // Navigation dans le menu des v�hicules
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectedVehicleIndex = (selectedVehicleIndex + 1) % vehicles.Count;
            UpdateVehicleSelection();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectedVehicleIndex = (selectedVehicleIndex - 1 + vehicles.Count) % vehicles.Count;
            UpdateVehicleSelection();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedVehicleIndex = (selectedVehicleIndex - vehicleColumns + vehicles.Count) % vehicles.Count;
            UpdateVehicleSelection();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedVehicleIndex = (selectedVehicleIndex + vehicleColumns) % vehicles.Count;
            UpdateVehicleSelection();
        }

        // Confirmer le choix du v�hicule et passer au menu des circuits
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("V�hicule s�lectionn� : " + vehicleNames[selectedVehicleIndex]);
            if (vehicleImages != null && selectedVehicleIndex < vehicleImages.Count)
            {
                vehicleImageInCourseMenu.sprite = vehicleImages[selectedVehicleIndex].sprite;
            }
            SwitchMenu();  // Bascule vers le menu de s�lection des circuits
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
            Debug.LogWarning("Aucune sc�ne assign�e pour ce logo !");
        }
    }

    void UpdateCharacterSelection()
    {
        // Mettre � jour la taille des personnages s�lectionn�s
        for (int i = 0; i < characters.Count; i++)
        {
            if (i == selectedCharacterIndex)
            {
                characters[i].transform.localScale = Vector3.one * selectedScale;
            }
            else
            {
                characters[i].transform.localScale = Vector3.one * normalScale;
            }
        }

        // Mettre � jour le nom du personnage s�lectionn�
        if (characterImages != null && selectedCharacterIndex < characterImages.Count)
        {
            characterImageInVehicleMenu.sprite = characterImages[selectedCharacterIndex].sprite;
            characterImageInCourseMenu.sprite = characterImages[selectedCharacterIndex].sprite;
        }

    }

    void UpdateVehicleSelection()
    {
        // Mettre � jour la taille des v�hicules s�lectionn�s
        for (int i = 0; i < vehicles.Count; i++)
        {
            if (i == selectedVehicleIndex)
            {
                vehicles[i].transform.localScale = Vector3.one * selectedScale;
            }
            else
            {
                vehicles[i].transform.localScale = Vector3.one * normalScale;
            }
        }

        // Mettre � jour le nom du v�hicule s�lectionn�
        if (vehicleNameText != null && selectedVehicleIndex < vehicleNames.Length)
        {
            vehicleNameText.text = vehicleNames[selectedVehicleIndex];
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

    void SwitchMenu()
    {
        // Bascule entre le menu des circuits et celui des personnages
        if (currentMenu == MenuType.Circuit)
        {
            currentMenu = MenuType.Character;
            characterSelectionCanvas.SetActive(true);
            vehicleSelectionCanvas.SetActive(false);
            courseSelectionCanvas.SetActive(false);
            UpdateCharacterSelection();
        }
        else if (currentMenu == MenuType.Character)
        {
            currentMenu = MenuType.Vehicle;
            characterSelectionCanvas.SetActive(false);
            vehicleSelectionCanvas.SetActive(true);
            courseSelectionCanvas.SetActive(false);
            UpdateVehicleSelection();
        }
        else if (currentMenu == MenuType.Vehicle)
        {
            currentMenu = MenuType.Circuit;
            characterSelectionCanvas.SetActive(false);
            vehicleSelectionCanvas.SetActive(false);
            courseSelectionCanvas.SetActive(true);
            UpdateLogoSizes();
        }
    }
}
