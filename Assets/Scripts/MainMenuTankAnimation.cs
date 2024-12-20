using UnityEngine;

public class MainMenuTankAnimation : MonoBehaviour {
    [SerializeField]
    private Animator rightTreadAnimation,leftTreadAnimation;

    private void Awake() {
        rightTreadAnimation.SetFloat("MoveSpeed",1f);
        leftTreadAnimation.SetFloat("MoveSpeed",1f);
    }
}