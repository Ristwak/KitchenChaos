using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Awake() {
        playButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.GameScene);
        });
        quitButton.onClick.AddListener(() => {
            Application.Quit();
        });

        // Game me pause krne ke bad vapas main menu vala button dbane pe game restart to ho jata h but koi bhi animation aur function kam nhi krta to usko kaam krnane ke liye yeniche vala code likha h
        Time.timeScale = 1f;
    }
}
