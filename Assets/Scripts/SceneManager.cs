using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;

    public Player Player;
    public List<Enemie> Enemies;
    public List<MiniGoblins> MiniGoblins;
    public List<DoubleGobl> DoubleGobls;

    public GameObject Lose;
    public GameObject Win;
    public Text CountWave;

    public GameObject DoubleGoblPrefab;
    public GameObject miniGoblinPrefab;
    public GameObject enemiePrefab;

    private int currWave = 0;
    [SerializeField] private LevelConfig Config;

    private int totalEnemiesToKill;
    private int currentKills;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SpawnWave();
    }

    private void Update()
    {
        Debug.Log(Enemies.Count + " " + MiniGoblins.Count);
        CheckForNextWave();
    }

    public void AddEnemie(Enemie enemie)
    {
        if (!Enemies.Contains(enemie))
        {
            Enemies.Add(enemie);
        }
    }

    public void AddDoubleGobl(DoubleGobl doublegobl)
    {
  
            DoubleGobls.Add(doublegobl);
        

        Vector3 pos = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));

        for (int i = 0; i < 2; i++)
        {
            Vector3 offset = new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
            Instantiate(DoubleGoblPrefab, pos + offset, Quaternion.identity);
        }
    }

    public void RemoveDoubleGobl(DoubleGobl doublegobl)
    {
        currentKills++;
   
            DoubleGobls.Remove(doublegobl);
            Destroy(doublegobl.gameObject);
            
            CheckForNextWave();
        
    }

    public void RemoveEnemie(Enemie enemie)
    {
        currentKills++;
      
            Enemies.Remove(enemie);
            Destroy(enemie.gameObject);
            
           CheckForNextWave();
        
    }

    public void AddMiniGoblins(MiniGoblins miniGoblin)
    {
        if (!MiniGoblins.Contains(miniGoblin))
        {
            MiniGoblins.Add(miniGoblin);
        }
    }

    public void RemoveMiniGoblins(MiniGoblins miniGoblin, Vector3 posDeath)
    {
      
            MiniGoblins.Remove(miniGoblin);
            Destroy(miniGoblin.gameObject);
            Player.Hp += 100;
            currentKills++;
            CheckForNextWave();
        
    }

    public void GameOver()
    {
        Lose.SetActive(true);
    }

    private void SpawnWave()
    {
        if (currWave >= Config.Waves.Length)
        {
            Win.SetActive(true);
            return;
        }

        CountWave.text = "Wave: " + (currWave + 1) + " / " + Config.Waves.Length;
        var wave = Config.Waves[currWave];

        totalEnemiesToKill = 0;
        currentKills = 0;

        foreach (var character in wave.Characters)
        {
            Vector3 pos = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));

            GameObject newEnemyObj;
            if (character.GetComponent<MiniGoblins>() != null)
            {
                newEnemyObj = Instantiate(miniGoblinPrefab, pos, Quaternion.identity);
                MiniGoblins miniGoblin = newEnemyObj.GetComponent<MiniGoblins>();
                AddMiniGoblins(miniGoblin);
            }
            else
            {
                newEnemyObj = Instantiate(enemiePrefab, pos, Quaternion.identity);
                Enemie enemie = newEnemyObj.GetComponent<Enemie>();
                AddEnemie(enemie);
            }
        }
        totalEnemiesToKill = Enemies.Count + MiniGoblins.Count+ MiniGoblins.Count*2;
        Debug.Log(totalEnemiesToKill);
        currWave++;
    }

    private void CheckForNextWave()
    {
        Debug.Log(currentKills);
        if (totalEnemiesToKill == currentKills)
        {
            SpawnWave();
        }
    }

    public void Reset()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
