using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public readonly int MaxHealthPoints = 500;
    public readonly int HealthSize = 100;
    public readonly int MaxSouls = 20;

    [Tooltip("Used to set the amount of souls necesary to heal.")]
    public readonly int SoulRequirement = 5;
    public int HealthPoints { private set; get; }
    public int Souls { private set; get; }
    private int SoulHealRate;

    [SerializeField, Range(0,500)] private int debugHealth;
    [SerializeField, Range(0,20)] private int debugSouls;

    private void Start() {
        HealthPoints = MaxHealthPoints;
        SoulHealRate = 100 / MaxSouls;
        Souls = 0;
    }

    private void Update() {
        Heal();
    }

    public int HurtPlayer(int _damage) => HealthPoints -= _damage;


    private void Heal() {
        if(Souls < SoulRequirement) return;
        if(InputManager.HealInput) {
            HealthPoints += Souls * SoulHealRate;
            Souls = 0;
        }
    }


    private void OnValidate() {
        HealthPoints = debugHealth;
        Souls = debugSouls;
    }
}
