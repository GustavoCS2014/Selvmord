using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private Image[] HealthDisplays = new Image[5];
    [SerializeField] private Image SoulsDisplay;

    [SerializeField] private PlayerStats playerStats;

    private int lifeIndex;
    private void Update() {
        UpdateHealthBar();
        UpdateSoulsBar();
    }

    private void UpdateHealthBar() {
        lifeIndex = Mathf.FloorToInt(playerStats.HealthPoints / playerStats.HealthSize);
        lifeIndex = lifeIndex == 5 ? 4 : lifeIndex;
        int _currentHealth = playerStats.HealthPoints - (lifeIndex * playerStats.HealthSize);
        HealthDisplays[lifeIndex].fillAmount = _currentHealth * 0.01f;
        for(int i = 0; i < HealthDisplays.Length; i++) {
            if(i > lifeIndex) {
                HealthDisplays[i].fillAmount = 0;
            }
        }
    }

    private void UpdateSoulsBar() {
        float _fillAmount = 1f/playerStats.MaxSouls;
        SoulsDisplay.fillAmount = _fillAmount * playerStats.Souls;
    }
}
