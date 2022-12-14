using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

//Dictionary<int, string> SceneIndexToPlanetName = new Dictionary<int, string>();
//SceneIndexToPlanetName.Add(2, "Earth");
//SceneIndexToPlanetName.Add(3, "Moon");
//SceneIndexToPlanetName.Add(4, "Mars");
//SceneIndexToPlanetName.Add(4, "Venus");

public class LevelManager : MonoBehaviour {

    Dictionary<int, string> SceneIndexToPlanetName = new Dictionary<int, string>()
    {
        { 2,"Earth" }, {3,"Moon"}, {4,"Mars"}, {5,"Venus"}
    };

    public float MaxLevelTimeSec = 100f;
    private float currentLevelTime;
    private bool isLevelOver = false;
    [SerializeField] private bool TimeConstraintActive = true;
    private int CurrentLivingEnemies = 0;
    [SerializeField] private int MaxNumberOfEnemies = 10;
    public UnityEvent onLevelOver;
    public UnityEvent onLevelStart;
    public GameObject Enemyspawner;
    public GameObject Player;
    public GameObject LevelOverScreen;
    public GameObject HUD;

    [SerializeField] List<Objective> MainObjectives;
    [SerializeField] List<Objective> SecondaryObjectives;

    private void Start()
    {
        EnemySpawner.MaxEnemies = MaxNumberOfEnemies;
    }
    void Update () {

        currentLevelTime += Time.deltaTime;
        if (TimeConstraintActive && currentLevelTime >= MaxLevelTimeSec ) {
            currentLevelTime = 0;
            GameOver ();
            // onLevelOver.Invoke ();
            // SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
        }

    }

    public void SetTimeConstraintOFF()
    {
        TimeConstraintActive = false;
    }

    public string GetLevelTimeString () {
        int remaningTime = (int) (MaxLevelTimeSec - currentLevelTime);
        int minutes = (int) (remaningTime / 60);
        int seconds = (int) (remaningTime % 60);
        return string.Format ("{0:00}:{1:00}", minutes, seconds);
    }

    public void DecreaseLivingEnemies () {
        CurrentLivingEnemies--;
        (Enemyspawner.GetComponent<EnemySpawner> ()).DecreaseCounter ();
        UpdateandCheckEnemies();
    }

    public void GameOver () {
        // isLevelOver = true;
        Debug.Log("Lol");
        Player.SetActive (false);
        Player.GetComponent<PlayerController>().enabled = false;
        LevelOverScreen.SetActive (true);
        HUD.SetActive (false);
        onLevelOver.Invoke ();
        Debug.Log("LolfelA5er");
    }
    public void Retry () {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
    }
    public void BackToMainMenu () {
        SceneManager.LoadScene (0);
    }

    public void UpdateandCheckFuel(float value)
    {
        for (int i = 0; i < MainObjectives.Count; i++)
        {
            if (MainObjectives[i].Type == ObjectiveType.Fuel)
            {
                MainObjectives[i].CurrentCount += value;
                Debug.Log($"Current Fuel is {MainObjectives[i].CurrentCount}");
                if (MainObjectives[i].CheckComplete())
                {
                    Debug.Log("Completed Fuel");
                    Destroy(MainObjectives[i]);
                    MainObjectives.RemoveAt(i);

                }
            }

        }
        CheckLevelDone();
        for (int i = 0; i < SecondaryObjectives.Count; i++)
        {
            if (SecondaryObjectives[i].Type == ObjectiveType.Fuel)
            {
                SecondaryObjectives[i].CurrentCount += value;
                if (SecondaryObjectives[i].CheckComplete())
                {
                    Destroy(SecondaryObjectives[i]);
                    SecondaryObjectives.RemoveAt(i);

                }
            }

        }

    }
    void UpdateandCheckEnemies()
    {
        for (int i = 0; i < MainObjectives.Count; i++)
        {
            if (MainObjectives[i].Type == ObjectiveType.Enemy)
            {
                MainObjectives[i].CurrentCount += 1;
                if (MainObjectives[i].CheckComplete())
                {
                    Destroy(MainObjectives[i]);
                    MainObjectives.RemoveAt(i);

                }
            }

        }
        CheckLevelDone();
        for (int i = 0; i < SecondaryObjectives.Count; i++)
        {
            if (SecondaryObjectives[i].Type == ObjectiveType.Enemy)
            {
            SecondaryObjectives[i].CurrentCount += 1;
                if (SecondaryObjectives[i].CheckComplete())
                {
                    Destroy(SecondaryObjectives[i]);
                    SecondaryObjectives.RemoveAt(i);

                }
            }

        }
    }

    public void UpdateandCheckStarshipKey(float value)
    {
        for (int i = 0; i < MainObjectives.Count; i++)
        {
            if (MainObjectives[i].Type == ObjectiveType.StarshipKey)
            {
                MainObjectives[i].CurrentCount += value;
                if (MainObjectives[i].CheckComplete())
                {
                    Destroy(MainObjectives[i]);
                    MainObjectives.RemoveAt(i);

                }
            }

        }
        CheckLevelDone();
    }

    public void  CheckLevelDone()
    {
        //Note that non active missions return false;
        /*Debug.Log("Checking Done")*/;
                //Debug.Log("Checking....");
        for(int i=0;i < MainObjectives.Count; i++)
        {
            //Debug.Log("Now Checking"+ MainObjectives[i].Type);
            if (MainObjectives[i].CheckComplete()) {
                //Debug.Log("Truly done in the name of god");
                Destroy(MainObjectives[i]);
                MainObjectives.RemoveAt(i);
            }
        }
        if (MainObjectives.Count == 0)
        {
            //LevelDone();
            LevelDone();
        }

    }

    void LevelDone()
    {
        //You need to first check you aren't at the end of the game 
        string planetName = SceneIndexToPlanetName[SceneManager.GetActiveScene().buildIndex+1];
        PlayerPrefs.SetInt(planetName, 1);
       SceneManager.LoadScene(1);
      
    }
}