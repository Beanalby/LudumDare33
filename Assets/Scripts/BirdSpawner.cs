using UnityEngine;
using System.Collections;

namespace LudumDare33 {
    public class BirdSpawner : MonoBehaviour {

        public GameObject birdPrefab;
        private int numBirds = 3;
        private float spawnDelay = 1f;

        public void Start() {
            for (int i = 0; i < numBirds; i++) {
                SpawnBird();
            }
        }

        private void SpawnBirdDelayed() {
            StartCoroutine(__spawnBirdDelayed());
        }
        private IEnumerator __spawnBirdDelayed() {
            yield return new WaitForSeconds(spawnDelay);
            SpawnBird();
        }
        private void SpawnBird() {
            if (!StageDriver.Instance.IsRunning) {
                return;
            }
            Transform point = transform.GetChild(Random.Range(0, transform.childCount - 1));
            GameObject newBird = (GameObject)Instantiate(birdPrefab, point.transform.position, Quaternion.identity);
            newBird.GetComponent<Mover>().OnDeath = OnBirdDeath;
        }

        public void OnBirdDeath(Mover bird) {
            Debug.Log("Bird died, make another!");
            SpawnBirdDelayed();
        }
    }
}