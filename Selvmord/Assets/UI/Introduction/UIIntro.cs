using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIIntro : MonoBehaviour
{
    [SerializeField] GameObject UI;
    [SerializeField] AudioClip SoundClick;

    // Start is called before the first frame update
    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button btnCon = root.Q<Button>("ContinueBTN");

        btnCon.clicked += () => CloseUI();

        if (PlayerPrefs.GetInt("FistStart") > 0)
        {
            UI.SetActive(false);
        }


    }

    void CloseUI()
    {
        AudioManager.Instance.ReproduceClick(SoundClick);
        PlayerPrefs.SetInt("FistStart",1);
        UI.SetActive(false);
    }

}
