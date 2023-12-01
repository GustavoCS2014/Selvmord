using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

[RequireComponent(typeof(ParticleSystem))]
public class SoulPickup : MonoBehaviour
{
    [SerializeField] private float startSleep;
    [SerializeField] private AnimationCurve speedCurve;
    [SerializeField, Range(0.05f, 1)] private float speed;
    [SerializeField] private float pickUpRange;
    private ParticleSystem pS;
    private MainSystem MS;
    private Particle[] particles = new Particle[500];

    private int counter;

    private void Awake() {
        pS = GetComponent<ParticleSystem>();
        MS = GameObject.FindGameObjectWithTag("MainSystem").GetComponent<MainSystem>();
    }

    private void Update() {
        Transform _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        int _particleCount = pS.GetParticles(particles);
        for(int i = 0; i < _particleCount; i++) {
            var particle = particles[i];
            float distance = Vector2.Distance(_playerTransform.position, particle.position);
            if(particle.remainingLifetime > particle.startLifetime - startSleep) break;

            if(distance > pickUpRange) {
                particle.remainingLifetime = particle.startLifetime - startSleep;
                particles[i] = particle;
                continue;
            }

            if(distance < .5f) {
                particle.remainingLifetime = 0;
                counter += 1;
                if(counter % 3 == 0) {
                    counter -= 3;
                    MS.AddSoul(1);
                }
                particles[i] = particle;
                if(_particleCount < 1) pS.Stop();
                Destroy(gameObject);
                continue;
            }
            if(distance > .1f) {
                particle.position = Vector2.Lerp(particle.position, _playerTransform.position, speedCurve.Evaluate(Remap(particle.remainingLifetime, particle.startLifetime, 0, 0, 1))*speed);
                particles[i] = particle;
            }
        }
        //Debug.Log(particles[1].remainingLifetime);
        pS.SetParticles(particles,_particleCount);
        //if(pS.GetParticles(particles) == 0) pS.Stop();
    }

    private float Remap(float _value, float _fromOne, float _toOne, float _fromTwo, float _toTwo) {
        return (_value - _fromOne) / (_toOne - _fromOne) * (_toTwo - _fromTwo) + _fromTwo;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickUpRange);
    }

}
