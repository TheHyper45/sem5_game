using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelCompletedMenu : MonoBehaviour {
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private TMP_Text collectedMoneyText;
    [SerializeField]
    private Button goBackButton;

    private bool levelHasEnded = false;
    private int collectedMoneyAmount = 0;

    private void Awake() {
        canvas.gameObject.SetActive(false);
        goBackButton.onClick.AddListener(() => {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        });
    }

    private void FixedUpdate() {
        if(levelHasEnded) return;
        if(SpawnBase.spawnBases.Count > 0) return;
        collectedMoneyAmount = PlayerTank.instance ? PlayerTank.instance.collectedMoney : 0;
        if(GameState.instance.IsGameSaveDataLoaded()) {
            GameState.instance.MarkLevelCompleted(SceneManager.GetActiveScene().name,collectedMoneyAmount);
        }
        StartCoroutine(ShowMenu(3f));
        levelHasEnded = true;
    }

    private IEnumerator ShowMenu(float time) {
        yield return new WaitForSeconds(time);
        canvas.gameObject.SetActive(true);
        collectedMoneyText.text = $"Money: {collectedMoneyAmount}";
        Time.timeScale = 0f;
    }
}