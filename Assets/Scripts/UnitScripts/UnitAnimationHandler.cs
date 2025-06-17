using UnityEngine;

public class AnimationHandler: MonoBehaviour
{
   public static void Trigger(string trigger, Animator animator)
    {
        animator.SetTrigger(trigger);
    }

    public static void SetTrueBool(string value, Animator animator)
    {
        animator.SetBool(value, true);
    }
    public static float GetWaitTime(Animator animator)
    {
       return animator.GetCurrentAnimatorStateInfo(0).length;
    }
}
