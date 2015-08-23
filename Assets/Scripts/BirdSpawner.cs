using UnityEngine;
using System.Collections;

namespace LudumDare33 {
    public class BirdSpawner : MonoBehaviour {

        public GameObject birdPrefab;
        private int numBirds = 3;
        private float spawnDelay = 1f;

        #region MonoBehavior
        public void Start() {
            StageDriver.Instance.OnStageStart += OnStageStart;
            StageDriver.Instance.OnStageEnd += OnStageEnd;
        }
        #endregion

        #region public
        public void OnStageStart() {
            for (int i = 0; i < numBirds; i++) {
                SpawnBird();
            }
        }
        public void OnStageEnd() {
            gameObject.SetActive(false);
        }
        public void BirdKilled(Mover bird) {
            StageDriver.Instance.BirdKilled();
            SpawnBirdDelayed();
        }
        #endregion

        #region private
        private void SpawnBirdDelayed() {
            StartCoroutine(__spawnBirdDelayed());
        }
        private IEnumerator __spawnBirdDelayed() {
            yield return new WaitForSeconds(spawnDelay);
            SpawnBird();
        }
        private void SpawnBird() {
            Transform point = transform.GetChild(Random.Range(0, transform.childCount - 1));
            GameObject newBird = (GameObject)Instantiate(birdPrefab, point.transform.position, Quaternion.identity);
            newBird.GetComponent<Mover>().OnDeath = BirdKilled;
        }
        #endregion
    }
}