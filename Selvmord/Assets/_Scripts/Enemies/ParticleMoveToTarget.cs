using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleMoveToTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float startSleep;
    [SerializeField] private AnimationCurve speedCurve;
    [SerializeField, Range(0.05f, 1)] private float speed;
    [SerializeField] private float pickUpRange;
    private ParticleSystem pS;
    private Particle[] particles = new Particle[500];
    public float TIME;

    private void Awake() {
        pS = GetComponent<ParticleSystem>();
    }

    private void Update() {
        if(InputManager.InteractiveKey && !pS.isPlaying) pS.Play();

        int _particleCount = pS.GetParticles(particles);
        for(int i = 0; i < _particleCount; i++) {
            var particle = particles[i];
            float distance = Vector2.Distance(target.position, particle.position);

            if(particle.remainingLifetime > particle.startLifetime - startSleep) break;

            if(distance > pickUpRange) {
                particle.remainingLifetime = particle.startLifetime - startSleep;
                particles[i] = particle;
                continue;
            }

            if(distance < .5f) {
                particle.remainingLifetime = 0;
                particles[i] = particle;
                if(_particleCount < 1) pS.Stop();
                continue;
            }
            if(distance > .1f) {
                particle.position = Vector2.Lerp(particle.position, target.position, speedCurve.Evaluate(Remap(particle.remainingLifetime, particle.startLifetime, 0, 0, 1))*speed);
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
