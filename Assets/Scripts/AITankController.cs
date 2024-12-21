using UnityEngine;

public class AITankController : DrivableTank {
    [SerializeField]
    private HealthBar healthBar;

    protected override void Awake() {
        base.Awake();
    }

    private void Update() {
        healthBar.SetValue((float)health / maxHealth);
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
    }
}