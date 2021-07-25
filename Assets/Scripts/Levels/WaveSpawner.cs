using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {
    public enum SpawnState{ SPAWNING, WAITING, COUNTING };

        [System.Serializable]//This is so we can adjust the number of waves within unity
        public class Wave {
        public string name; 
        public Transform enemy;
        public int count;
        public float rate;
    }

    public Wave[] waves;//this array is what we control in unity
    private int nextWave = 0;

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f; // set to 5 seconds
    private float waveCountdown;

    private float searchCountdown = 1f;

    private SpawnState state = SpawnState.COUNTING;

    void Start(){
        waveCountdown = timeBetweenWaves;
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("Error no spawn points available");
        }
    }

    void Update(){
        if (state == SpawnState.WAITING){//this entire if statements checks to see if we killed everything in the curent wave
            if (!EnemyIsAlive()){
                WaveCompleted();                
            } else {
                return;
            }
        }

        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[nextWave])); // calls the wave spawning ienum
            }
        } else {
            waveCountdown -= Time.deltaTime;
        }
    }

    void WaveCompleted(){
        Debug.Log("Wave Completed");

        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if( nextWave + 1 > waves.Length - 1 ){
            nextWave = 0;
            Debug.Log("Completed all Waves, now looping to the start.");
        } else {
            nextWave++;
            }

        
    }

    bool EnemyIsAlive(){//checking if enemy is alive once a second 
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown =1f;
            //Method that waits a certain number of seconds before continuing
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
                {
                    return false;
                }
        }

        return true;
    }

    IEnumerator SpawnWave(Wave _wave){  //Method that waits a certain number of seconds before continuing
        Debug.Log("Spawning wave: "+ _wave.name);
        state = SpawnState.SPAWNING;//Start Spawning
         
         for (int i = 0; i< _wave.count; i++)
         {
             SpawnEnemy(_wave.enemy);
             yield return new WaitForSeconds(1/_wave.rate);
         }

        state = SpawnState.WAITING;//Stop Spawning
        yield break; //for some reason we HAVE to end this with this otherwise things get very very angry with me
    } 

    public void SpawnEnemy(Transform _enemy){ //instantiates an enemy to be spawned
        Debug.Log("Spawning Enemy: " + _enemy.name);
        
        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate (_enemy, _sp.position, _sp.rotation);
    }
}
