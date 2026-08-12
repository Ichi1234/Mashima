using System;
using UnityEngine;

public class PursuerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [Header("Parameters")]
    [SerializeField] private string isIdleParam = "idle";
    [SerializeField] private string isRunningParam = "run";
    [SerializeField] private string isWalkingParam = "walk";
    [SerializeField] private string walkHeadDir = "walkHeadDir";
    [SerializeField] private string attackParam = "attack";
    [SerializeField] private string nextAttackParam = "nextAttack";
    [SerializeField] private string roar = "roar";

    public Action OnAnimationFinished;
    public Action OnFootSteped;

    public void SetIdle(bool value)
    {
        animator.SetBool(isIdleParam, value);
    }

    public void SetRunning(bool value)
    {
        animator.SetBool(isRunningParam, value);
    }

    public void SetWalking(bool value)
    {
        animator.SetBool(isWalkingParam, value);
    }

    public void SetWalkHeadDirection(float value)
    {
        animator.SetFloat(walkHeadDir, value);
    }

    private void OnFootStep() => OnFootSteped?.Invoke();
    private void OnRoarFinished() => OnAnimationFinished?.Invoke();

    public void PlayAttack()
    {
        animator.SetTrigger(attackParam);
    }

    public void PlayNextAttack()
    {
        animator.SetTrigger(nextAttackParam);
    }

    public void PlayRoar()
    {
        animator.SetTrigger(roar);
    }

    public void ResetAllAnimation()
    {
        animator.SetBool(isIdleParam, false);
        animator.SetBool(isRunningParam, false);
        animator.SetBool(isWalkingParam, false);
        animator.SetFloat(walkHeadDir, 0.5f);
        animator.ResetTrigger(attackParam);
        animator.ResetTrigger(nextAttackParam);
        animator.ResetTrigger(roar);
    }
    
}