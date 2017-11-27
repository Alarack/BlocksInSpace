using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class RandomSpawner : MasterSpawner {

    public enum Difficulty {
        Simple,
        Easy,
        Moderate,
        Taxing,
        Hard,
        Stressful,
        Brutal
    }


    [Header("Random Waves Info")]
    public Difficulty difficulty;
    public float difficultyValue;
    public float difficultyIncreaseIncrement = 25f;
   
    [Header("Spawn Points")]
    public List<Transform> leftScreenSpawnPoints = new List<Transform>();
    public List<Transform> rightScreenSpawnPoints = new List<Transform>();
    public List<Transform> topScreenSpawnPoints = new List<Transform>();
    public List<Transform> onScreenSpawnPoints = new List<Transform>();
    [Header("Enemies")]
    public List<GameObject> enemies = new List<GameObject>();
    public int numTypes;
    //public float numSpawnMod;

    //[HideInInspector]
    public List<Wave> randomWaves = new List<Wave>();

    protected List<Transform> masterSpawnPointList = new List<Transform>();
    protected float spawnCooldown;

    protected override void Start () {
        
        masterSpawnPointList.AddRange(leftScreenSpawnPoints);
        masterSpawnPointList.AddRange(rightScreenSpawnPoints);
        masterSpawnPointList.AddRange(topScreenSpawnPoints);
        base.Start();
    }

	void Update () {
	
	}


    protected override void FixedUpdate() {
        if (randomWaves[waveCount].IsWaveFinished() && randomWaves[waveCount].upgradeWave) {
            GameManager.ToggleUpgradeScreen();
        }

        if (BeginWave() && beginSpawing) {
            randomWaves[waveCount].Spawn();
        }

        if (randomWaves[waveCount].IsWaveFinished() && !randomWaves[waveCount].upgradeWave && waveCount < randomWaves.Count - 1) {
            StartCoroutine(NewWave());
        }

        if (waveCount == randomWaves.Count - 1 && randomWaves[waveCount].IsWaveFinished() && !gameOver) {
            gameOver = true;

            waveUI.SetActive(true);
            waveUI.GetComponent<Text>().text = "You Won The Whole Damn Game!";
        }
    }

    protected override bool BeginWave() {
        return randomWaves[waveCount].CanSpawn() && !randomWaves[waveCount].IsWaveFinished() && waveCount < randomWaves.Count;
    }

    protected override IEnumerator NewWave() {
        randomWaves.Add(CreateRandomWave());
        difficultyValue += difficultyIncreaseIncrement;
        beginSpawing = false;
        waveCount++;
        //lastWave = Time.time + waveCooldown;
        waveUI.SetActive(true);
        Camera.main.transform.position = Camera.main.GetComponent<CameraController>().initPos;
        if (randomWaves[waveCount].bossWave) {
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



    protected override IEnumerator Init() {
        randomWaves.Add(CreateRandomWave());
        randomWaves.Add(CreateRandomWave());
        return base.Init();
    }

    Wave CreateRandomWave() {
        Wave tmpWave = new Wave();
        tmpWave.waitForAllEnemies = true;
        int upgradeChance = Random.Range(0, 5);
        if (upgradeChance == 0)
            tmpWave.upgradeWave = true;

        if ((waveCount + 1) % 5 == 0)
            tmpWave.bossWave = true;

        tmpWave.spawns = DetermineEnemies(tmpWave);
        tmpWave.maxSpawns = DetermineMaxSpawns(tmpWave.spawns);
        tmpWave.spawnCooldown = spawnCooldown;
        tmpWave.spawnPoints = DetermineSpawnPoints();

        return tmpWave;
    }

    GameObject[] DetermineEnemies(Wave wave) {
        List<GameObject> tmpEnemies = new List<GameObject>();

        foreach (GameObject enemy in enemies) {
            Entity enemyScript = enemy.GetComponent<Entity>();

            if(enemyScript.difficultyValue <= difficultyValue && ((!enemy.GetComponent<Entity>().bossEntity && !wave.bossWave) || (enemy.GetComponent<Entity>().bossEntity && wave.bossWave == true))) {
                tmpEnemies.Add(enemy);
            }
        }
        

        List<GameObject> finalEnemyChoices = new List<GameObject>();
        for (int i = 0; i < numTypes; i++) {
            int randomEnemy = Random.Range(0, tmpEnemies.Count);
            finalEnemyChoices.Add(tmpEnemies[randomEnemy]);
        }

        return finalEnemyChoices.ToArray();
    }

    Transform[] DetermineSpawnPoints() {
        List<Transform> tmpPoints = new List<Transform>();

        for(int i = 0; i < 3; i++) {
            int randPoint = Random.Range(0, masterSpawnPointList.Count);
            tmpPoints.Add(masterSpawnPointList[randPoint]);
        }

        return tmpPoints.ToArray();
    }

    int DetermineMaxSpawns(GameObject[] spawns) {
        float totalDiff = 0;
        int count = 0;

        foreach (GameObject enemy in spawns) {
            Entity enemyScript = enemy.GetComponent<Entity>();
            count++;
            totalDiff += enemyScript.difficultyValue;
        }

        float avarageDiff = (totalDiff) / count;
        float modifiedDiffValue = difficultyValue / avarageDiff;

        spawnCooldown = (avarageDiff / difficultyValue) + 0.2f;


        return (int)(modifiedDiffValue);
    }

    //float DetermineSpawnRate(GameObject[] spawns) {
    //    float spawnRate = 1;
    //    float totalDiff = 0;

    //    foreach (GameObject enemy in spawns) {
    //        Entity enemyScript = enemy.GetComponent<Entity>();
    //        totalDiff += enemyScript.difficultyValue;
    //    }


    //    return spawnRate;
    //}
}