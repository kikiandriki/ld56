using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneGuy : MonoBehaviour {

    public string gameScene = "";
    public string menuScene = "";
    public string loseScene = "";
    public string winScene = "";

    void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadGame() {
        SceneManager.LoadScene(gameScene);
    }

    public void LoadMenu() {
        SceneManager.LoadScene(menuScene);
    }

    public void LoadLose() {
        SceneManager.LoadScene(loseScene);
    }

    public void LoadWin() {
        SceneManager.LoadScene(winScene);
    }

    public void LoadQuit() {
        Application.Quit();
    }
}
