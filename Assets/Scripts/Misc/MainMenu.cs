using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public string mouseOverSound;
    public string selectSound;

    public Loot[] drops;
    public float spawnDelay;
    public Transform spawnPoint;

    private int numDrops = 250;

    void Awake() {
        Screen.SetResolution(600, 900, true);
    }

    void Start() {
        drops = GetComponents<Loot>();
        StartCoroutine(BlockRain());
    }

    public void MouseOver() {
        SoundManager.PlaySound(mouseOverSound, 1f, false);
    }

    public void ArcadeMode() {
        SoundManager.PlaySound(selectSound, 1f, false);
        SoundManager.SwapMusic("Level");
        SoundManager.RestartMusic("Level");
        SceneManager.LoadScene("Main");
    }

    public void EndlessMode() {
        SoundManager.PlaySound(selectSound, 1f, false);
        SoundManager.SwapMusic("Level");
        SoundManager.RestartMusic("Level");
        SceneManager.LoadScene("Endless");
    }

    public void QuitGame() {
        SoundManager.PlaySound(selectSound, 1f, false);
        Application.Quit();
    }

    public void TeachMe() {
        SoundManager.PlaySound(selectSound, 1f, false);
        SceneManager.LoadScene("Tutorial");
    }



    //public IEnumerator SwapScene(string scene) {
    //    yield return new WaitForSeconds(0.2f);
    //    SceneManager.LoadScene(scene);
    //}


    public IEnumerator BlockRain() {
        for (int i = 0; i < numDrops; i++) {
            int dropIndex = Random.Range(0, drops.Length);
            drops[dropIndex].DropLoot(spawnPoint.position, spawnPoint.rotation);
            numDrops++;
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}