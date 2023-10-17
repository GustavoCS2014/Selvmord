using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{

    private PlayerMovement player;

    [SerializeField] private ParticleSystem LandParticles;
    [SerializeField] private ParticleSystem HoodieDashParticles;
    [SerializeField] private ParticleSystem PantsDashParticles;

    // Start is called before the first frame update
    private void OnEnable() {
        EventManager.Instance.OnPlayerLand += Land;
    }

    private void OnDisable() {
        EventManager.Instance.OnPlayerLand -= Land;
    }

    private void Awake() {
        player = GetComponent<PlayerMovement>();
    }

    private void Update() {
        Dash();
    }

    private void Land() {
        LandParticles.Play();
    }

    private void Dash() {
        if(!player.IsDashing) {
            HoodieDashParticles.Stop();
            PantsDashParticles.Stop();
            return;
        }
        if(player.IsDashing) {
            HoodieDashParticles.Play();
            PantsDashParticles.Play();
            return;
        }
    }
}
