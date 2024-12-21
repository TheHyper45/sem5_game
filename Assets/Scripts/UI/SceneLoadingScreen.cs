using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoadingScreen : MonoBehaviour {
    [SerializeField]
    private Slider progressBar;
    [SerializeField]
    private TMP_Text label; 
    
    public void SetLevelName(string name) {
        label.text = $"Loading \"{name}\" ...";
    }

    public void SetProgress(float progress) {
        progressBar.value = progress;
    }
}