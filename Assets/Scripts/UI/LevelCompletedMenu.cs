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

    private IEnumerator ShowMenu(float time) {
        yield return new WaitForSeconds(time);
        canvas.gameObject.SetActive(true);
        collectedMoneyText.text = $"Collected: ${GameState.instance.playerCollectedMoney}";
        Time.timeScale = 0f;
    }

    private void Awake() {
        canvas.gameObject.SetActive(false);
        goBackButton.onClick.AddListener(() => {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        });
        SaveFile.MarkLevelCompleted(SceneManager.GetActiveScene().name,GameState.instance.playerCollectedMoney);
        StartCoroutine(ShowMenu(1.5f));
    }
}