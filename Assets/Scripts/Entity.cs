using UnityEngine;
using UnityEngine.Audio;

public class Entity : MonoBehaviour
{
    protected StateMachine stateMachine;
    protected float moveSpeedMultiplier = 1;

    [Header("General")]
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected string footStepName = "-footstep";

    protected virtual void Awake()
    {
        stateMachine = new StateMachine();
    }

    protected virtual void Update()
    {
        if (GameManager.Instance.IsGameEnded) return;

        stateMachine.CallUpdateCurrentState();
    }

    protected virtual void FixedUpdate() => stateMachine.CallFixedUpdateCurrentState();

    public void ResetMoveSpeedMultiplier() => moveSpeedMultiplier = 1;
    public void SetMoveSpeedMultiplier(float newMultiplier) => moveSpeedMultiplier = newMultiplier;
    public void PlayFootStepSound() => AudioManager.Instance.PlaySFX(footStepName, audioSource, true);

}
