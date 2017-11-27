using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Spawner : MonoBehaviour {

    public GameObject[] spawns;
    public float spawnCooldown;
    public float waveCooldown;

    public GameObject waveUI;
    
    public int maxSpawn;
    public int maxWaves;

    private int waveCount;
    private int spawnCount;
    private int spawnAlt = 0;

    public Transform[] spawnPoints;

    private float lastSpawn;
    private float lastWave;
    private bool beginSpawing = false;

    void Start()
    {
        StartCoroutine(Init());
    }



    void FixedUpdate()
    {
        if(CanSpawn() && spawnCount < maxSpawn && beginSpawing)
        {
            AlternateSpawnPoints();
        }

        if(Time.time > lastWave && waveCount < maxWaves && spawnCount == maxSpawn)
        {
            StartCoroutine(NewWave());
        }
    }

    bool CanSpawn()
    {
        return Time.time > lastSpawn;
    }



    void AlternateSpawnPoints()
    {
        if (spawnAlt == 0)
        {
            spawnAlt = 1;
            Spawn(spawnAlt);
        } else if
            (spawnAlt == 1)
        {
            spawnAlt = 0;
            Spawn(spawnAlt);
        }
    }

    public void Spawn(int loc)
    {
        GameObject activeSpawn = Instantiate(spawns[waveCount], spawnPoints[loc].position, spawnPoints[loc].rotation) as GameObject;
        if (loc == 1 && activeSpawn.GetComponent<GalaMovement>() != null)
            activeSpawn.GetComponent<GalaMovement>().SwapRotation();
       
        spawnCount++;
        if (spawnCount == maxSpawn - 1)
        {
            lastWave = Time.time + waveCooldown;
        }

        lastSpawn = Time.time + spawnCooldown;
    }


    IEnumerator NewWave()
    {
        waveCount++;
        lastWave = Time.time + waveCooldown;
        waveUI.SetActive(true);
        waveUI.GetComponent<Text>().text = "Wave " + (waveCount + 1);
        yield return new WaitForSeconds(2f);
        waveUI.SetActive(false);
        spawnCount = 0; 
    }

    IEnumerator Init()
    {
        waveUI.SetActive(true);
        waveUI.GetComponent<Text>().text = "Wave " + (waveCount + 1);
        yield return new WaitForSeconds(2f);
        waveUI.SetActive(false);
        beginSpawing = true;
    }

    //IEnumerator SpawnWaves()
    //{

    //    while(waveCount < maxWaves)
    //    {
    //        for(int i = 0; i < maxSpawn; i++)
    //        {

    //            if (i % 2 == 0)
    //                spawnAlt = 0;
    //            if (i % 2 == 1)
    //                spawnAlt = 1;

    //            GameObject activeSpawn = Instantiate(spawns, spawnPoints[spawnAlt].position, spawnPoints[spawnAlt].rotation) as GameObject;

    //            if (spawnAlt == 1)
    //                activeSpawn.GetComponent<GalaMob>().SwapRotation();

                
    //            yield return new WaitForSeconds(spawnCooldown);
    //        }
    //        waveCount++;
    //        yield return new WaitForSeconds(waveCooldown);
    //    }
    //}
}
