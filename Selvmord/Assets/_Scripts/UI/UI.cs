using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    public MenusControler MC;

    private void OnEnable()
    {
        Invoke("StartConfiguration", 0.6f);
    }

    void StartConfiguration()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button btnContinue = root.Q<Button>("continue");
        Button btnNewGame = root.Q<Button>("newgame");
        Button btnLoad = root.Q<Button>("load");
        Button btnSettings = root.Q<Button>("settings");
        Button btnExit = root.Q<Button>("exit");

        btnContinue.clicked += () => MC.Continue();
        btnNewGame.clicked += () => MC.NewGame();
        btnLoad.clicked += () => MC.LoadGame();
        btnSettings.clicked += () => MC.Settings();
        btnExit.clicked += () => MC.Quit();
    }

    /*
    private void Start()
    {
        PlayerPrefs.SetInt("LastGame", 0);
        PlayerPrefs.SetFloat("CPX" + 1, 0);
        PlayerPrefs.SetFloat("CPY" + 1, 0);
        PlayerPrefs.SetInt("SpawnConter" + 1, 0);
        PlayerPrefs.SetInt("SpawnActive" + 1, 0);
        PlayerPrefs.SetInt("LastGame" + 1, 0);
        PlayerPrefs.SetInt("Life" + 1, 3);
        PlayerPrefs.SetFloat("Heal" + 1, 100);
        PlayerPrefs.SetFloat("Soul" + 1, 0);
        PlayerPrefs.SetFloat("CPX" + 2, 0);
        PlayerPrefs.SetFloat("CPY" + 2, 0);
        PlayerPrefs.SetInt("SpawnConter" + 2, 0);
        PlayerPrefs.SetInt("SpawnActive" + 2, 0);
        PlayerPrefs.SetInt("LastGame" + 2, 0);
        PlayerPrefs.SetInt("Life" + 2, 3);
        PlayerPrefs.SetFloat("Heal" + 2, 100);
        PlayerPrefs.SetFloat("Soul" + 2, 0);
        PlayerPrefs.SetFloat("CPX" + 3, 0);
        PlayerPrefs.SetFloat("CPY" + 3, 0);
        PlayerPrefs.SetInt("SpawnConter" + 3, 0);
        PlayerPrefs.SetInt("SpawnActive" + 3, 0);
        PlayerPrefs.SetInt("LastGame" + 3, 0);
        PlayerPrefs.SetInt("Life" + 3, 3);
        PlayerPrefs.SetFloat("Heal" + 3, 100);
        PlayerPrefs.SetFloat("Soul" + 3, 0);
    }*/
}
