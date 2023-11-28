using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UlMenuPause : MonoBehaviour
{

    [SerializeField] MenusControler MC;
    [SerializeField] GameObject Settings;
    [SerializeField] GameObject RetrunToMenu;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button btnContinue = root.Q<Button>("resume");
        Button btnNewGame = root.Q<Button>("settings");
        Button btnLoad = root.Q<Button>("mainmenu");

        btnContinue.clicked += () => MC.Resume();
        btnNewGame.clicked += () => settings();
        btnLoad.clicked += () => ReturnToMenu();
    }

    void settings()
    {
        Settings.SetActive(true);
        MC.menuPauseClose();
    }

    public void CloseSettings()
    {
        Settings.SetActive(false);
    }

    void ReturnToMenu()
    {
        MC.menuPauseClose();
        RetrunToMenu.SetActive(true);
    }
    public void pause()
    {
        Settings.SetActive(false);
        MC.Pause();
    }
}
