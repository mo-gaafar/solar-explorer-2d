using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Level {
    public string LevelName;
    public int LevelSceneIndex;
    public int LevelNumber;
    public bool IsUnlocked;
    public GameObject LevelObject;

    [TextArea (3, 10)]
    public string LevelDescription;
}

public class LevelPicker : MonoBehaviour {

    // public UnityEvent OnNextLevel;
    // public UnityEvent OnPreviousLevel;
    // public UnityEvent OnLevelSelect;
    [SerializeField] CinemachineVirtualCamera LevelCamera;
    [SerializeField] public List<Level> Levels;
    [SerializeField] TextMeshProUGUI LevelNameText;
    [SerializeField] TextMeshProUGUI LevelDescriptionText;
    [SerializeField] GameObject Lock;
    [SerializeField] GameObject PlayButton;

    public int CurrentLevelIndex = 0;

    private void Start () {

        for (int i = 0; i < Levels.Count; i++)
        {
           
            if (PlayerPrefs.HasKey(Levels[i].LevelName) && PlayerPrefs.GetInt(Levels[i].LevelName) != 0)
            {
                Levels[i].IsUnlocked = true;
            }
        }
        GotoLevel (CurrentLevelIndex);

    }

    public void NextLevel () {
        if (CurrentLevelIndex < Levels.Count - 1) {
            CurrentLevelIndex++;
            GotoLevel (CurrentLevelIndex);
        }
    }
    public void PreviousLevel () {
        if (CurrentLevelIndex > 0) {
            CurrentLevelIndex--;
            GotoLevel (CurrentLevelIndex);
        }
    }
    public void GotoLevel (int index) {
        if (index >= 0 && index < Levels.Count) {
            CurrentLevelIndex = index;
            LevelCamera.Follow = Levels[CurrentLevelIndex].LevelObject.transform;
            LevelCamera.LookAt = Levels[CurrentLevelIndex].LevelObject.transform;
            UpdateDesc ();
        }
    }
    public void PlayLevel () {
        if (Levels[CurrentLevelIndex].LevelNumber<= (SceneManager.sceneCountInBuildSettings-2))
        {
            if (CurrentLevelIndex >= 0 && CurrentLevelIndex < Levels.Count)
            {
                if (Levels[CurrentLevelIndex].IsUnlocked)
                {

                    //Destroy Music 
                    GameObject mus = GameObject.FindGameObjectWithTag("Game BGMusic");
                    Destroy(mus);

                    UnityEngine.SceneManagement.SceneManager.LoadScene(Levels[CurrentLevelIndex].LevelSceneIndex);
                }
                else
                {
                    Debug.Log("Level is locked");
                }
            }
        }
        else
        {
            Debug.Log("Not Implemented yet");
        }
    }
    //TODO implement level progression in playerprefs
    // public void RefreshLocks () {
    //     //gets level number from player prefs
    //     //loops if level number is less than level count then unlock level
    //     // for (int i = 0; i < Levels.Count; i++) {
    //     //     if (PlayerPrefs.GetInt ("Level" + i) == 1) {
    //     //         Levels[i].IsUnlocked = true;
    //     //     }
    //     // }
    // }
    public void UpdateDesc () {
        if (Levels[CurrentLevelIndex].IsUnlocked) {
            if (Levels[CurrentLevelIndex].LevelNumber > (SceneManager.sceneCountInBuildSettings - 2))
            {
                Lock.SetActive(true);
                PlayButton.SetActive(false);
                LevelNameText.text = "Coming Soon";
                LevelDescriptionText.text = "We Shall Deliver";
            }
            else
            {
                Lock.SetActive (false);
                PlayButton.SetActive (true);
                LevelNameText.text = $"Level {Levels[CurrentLevelIndex].LevelNumber}: {Levels[CurrentLevelIndex].LevelName}";
                LevelDescriptionText.text = Levels[CurrentLevelIndex].LevelDescription;
            }
        } else {
            Lock.SetActive (true);
            PlayButton.SetActive (false);
            LevelNameText.text = "Locked";
            LevelDescriptionText.text = "";
        }
    }
    public void BackToMenu () {
        UnityEngine.SceneManagement.SceneManager.LoadScene (0);
    }
}