using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MasterSpawner : MonoBehaviour {

    [System.Serializable]
    public class Wave {

        [Header("Wave Info")]
        public string waveName;
        public bool upgradeWave;
        public bool bossWave;
        public float spawnCooldown;
        public float waveDuration;

        [Header("Spawn Info")]
        public float maxSpawns;
        public GameObject[] spawns;
        public Transform[] spawnPoints;
        [Space(10)]
        public bool waitForAllEnemies = false;
        public bool useEachSpawnPoint = false;
        public bool randomizeSpawnColor = true;

        private int spawnPointCounter;
        private float lastSpawn;
        private float spawnCount;
        private List<GameObject> currentSpawns = new List<GameObject>();

        public bool CanSpawn() {
            return Time.time > lastSpawn && spawnCount < maxSpawns;
        }

        public void Spawn() {
            int loc = Random.Range(0, spawnPoints.Length);
            int mob = Random.Range(0, spawns.Length);

            if (useEachSpawnPoint) {
                loc = spawnPointCounter;
            }

            GameObject activeSpawn = Instantiate(spawns[mob], spawnPoints[loc].position, spawnPoints[loc].rotation) as GameObject;

            if (randomizeSpawnColor) {
                EnumList.Colors randomColor = (EnumList.Colors)Random.Range(0, 3);
                activeSpawn.GetComponent<EntityEnemy>().unitColor = randomColor;
            }

            currentSpawns.Add(activeSpawn);

            if (useEachSpawnPoint)
                spawnPointCounter++;

            spawnCount++;
            lastSpawn = Time.time + spawnCooldown;
        }

        public bool IsWaveFinished() {
            //if (upgradeWave)
            //    return upgradefinished;

            waveDuration -= Time.deltaTime;
            if (!waitForAllEnemies && waveDuration <= 0) {
                return spawnCount == maxSpawns;
            }
            else if (spawnCount == maxSpawns)
                return CheckCurrentEnemies();
            else
                return false;
        }

        public bool CheckCurrentEnemies() {
            for (int i = currentSpawns.Count - 1; i > -1; i--) {
                if (currentSpawns[i] == null)
                    currentSpawns.RemoveAt(i);
            }
            if (currentSpawns.Count == 0)
                return true;
            else
                return false;
        }
    }//End of Wave SubClass

    public GameObject waveUI;
    public GameObject upgradeScreen;
    public int waveCount;
    [Space(10)]
    public Wave[] waves;

    [HideInInspector]
    public bool beginSpawing = false;

    protected bool gameOver;

    protected virtual void Start() {
        StartCoroutine(Init());
    }

    protected virtual void FixedUpdate() {
        if (waves[waveCount].IsWaveFinished() && waves[waveCount].upgradeWave) {
            GameManager.ToggleUpgradeScreen();
        }

        if (BeginWave() && beginSpawing) {
            waves[waveCount].Spawn();
        }

        if (waves[waveCount].IsWaveFinished() && !waves[waveCount].upgradeWave && waveCount < waves.Length - 1) {
            StartCoroutine(NewWave());
        }

        if (waveCount == waves.Length - 1 && waves[waveCount].IsWaveFinished() && !gameOver) {
            gameOver = true;

            waveUI.SetActive(true);
            waveUI.GetComponent<Text>().text = "You Won The Whole Damn Game!";
        }
    }

    protected virtual bool BeginWave() {
        return waves[waveCount].CanSpawn() && !waves[waveCount].IsWaveFinished() && waveCount < waves.Length;
    }

    protected virtual IEnumerator Init() {
        waveUI.SetActive(true);
        waveUI.GetComponent<Text>().text = "Wave " + (waveCount + 1);
        yield return new WaitForSeconds(2f);
        waveUI.SetActive(false);
        beginSpawing = true;
    }

    protected virtual IEnumerator NewWave() {
        beginSpawing = false;
        waveCount++;
        //lastWave = Time.time + waveCooldown;
        waveUI.SetActive(true);
        Camera.main.transform.position = Camera.main.GetComponent<CameraController>().initPos;
        if (waves[waveCount].bossWave) {
            waveUI.GetComponent<Text>().color = Color.red;
            waveUI.GetComponent<Text>().text = "Boss Incoming!";
            yield return new WaitForSeconds(3f);
        }
        else {
            waveUI.GetComponent<Text>().color = Color.white;
            waveUI.GetComponent<Text>().text = "Wave " + (waveCount + 1);
            yield return new WaitForSeconds(2f);
        }

        //yield return new WaitForSeconds(2f);
        waveUI.SetActive(false);
        beginSpawing = true;
    }
}