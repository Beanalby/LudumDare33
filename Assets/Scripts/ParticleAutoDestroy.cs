using UnityEngine;
using System.Collections;

namespace LudumDare33 {
    public class ParticleAutoDestroy : MonoBehaviour {
        private void Start() {
            Destroy(gameObject, GetComponent<ParticleSystem>().main.duration);
        }
    }
}