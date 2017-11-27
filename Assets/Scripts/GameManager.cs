using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    [Header("Misc Stats")]
    public float gameOverDelay;
    [Header("Screen Overlays")]
    public GameObject gameOverScreen;
    public GameObject pauseScreen;
    public GameObject upgradeScreen;
    [Header("Main Header Text")]
    public GameObject setMainText;
    public float textDuration;
    [Header("Player")]
    public GameObject playerPrefab;

    public static float score;
    public static float missiles;
    private static GameObject mainText;
    private static GameManager _instance;

    private Transform startLocation;
    private EntityPlayer player;
    private bool isPaused;
    private float textTimer = 0f;

    void Awake() {
        _instance = this;

        mainText = setMainText;

        if (startLocation == null) {
            startLocation = GameObject.FindGameObjectWithTag("StartLocation").transform;
        }
        player = FindObjectOfType<EntityPlayer>();
        if (player == null) {
            Instantiate(playerPrefab, startLocation.position, startLocation.rotation);
            player = FindObjectOfType<EntityPlayer>();
        }
        else {
            player.transform.position = startLocation.position;
        }
    }

    public IEnumerator GameOver() {
        yield return new WaitForSeconds(2f);
        gameOverScreen.SetActive(true);
        yield return new WaitForSeconds(3f);
        score = 0f;
        SoundManager.RestartMusic("Title");
        SoundManager.SwapMusic("Title");
        SceneManager.LoadScene("MainMenu");
    }

    void Update() {
        //TODO: Remove this when done testing. Open the shop without a keycode
        if (Input.GetKeyDown(KeyCode.J)) {
            ToggleUpgradeScreen();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused && !upgradeScreen.activeSelf) {
            SoundManager.SwapMusic("Pause", 0f);
            isPaused = !isPaused;
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);

        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused) {
            SoundManager.SwapMusic("", 1, true);
            isPaused = !isPaused;
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);

        }

        if (mainText.activeSelf) {
            textTimer += Time.deltaTime;
            if (textTimer >= textDuration) {
                mainText.SetActive(false);
            }
        }
    }//End of Update

    //Upgrade Screen
    public static void ToggleUpgradeScreen() {
        _instance.upgradeScreen.SetActive(true);
        CameraController.shakeAmount = 0f;
        //Time.timeScale = 0f;
    }

    //UI Text
    public static void SetUIText(string entry) {
        mainText.SetActive(true);
        mainText.GetComponent<Text>().text = entry;
        _instance.textTimer = 0f;
    }

    //Load Scenes or Quit
    public void ReturnToMainMenu() {
        Time.timeScale = 1f;
        SoundManager.SwapMusic("Title");
        SoundManager.RestartMusic("Title");
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame() {
        Application.Quit();
    }
}