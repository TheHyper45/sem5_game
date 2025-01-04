using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelFailedMenu : MonoBehaviour {
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private TMP_Text collectedMoneyText;
    [SerializeField]
    private Button restartButton;
    [SerializeField]
    private Button goBackButton;

    private IEnumerator ShowMenu(float time) {
        yield return new WaitForSeconds(time);
        canvas.gameObject.SetActive(true);
        collectedMoneyText.text = $"Collected: ${GameState.instance.playerCollectedMoney}";
        Time.timeScale = 0f;
    }

    private void Awake() {
        canvas.gameObject.SetActive(false);
        restartButton.onClick.AddListener(() => {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
        goBackButton.onClick.AddListener(() => {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        });
        SaveFile.AddMoney(GameState.instance.playerCollectedMoney);
        StartCoroutine(ShowMenu(1.5f));
    }
}