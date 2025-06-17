using UnityEngine;

public class UnitAnimationHandler : MonoBehaviour
{
   public static void Trigger(string trigger, Animator animator)
    {
        animator.SetTrigger(trigger);
    

    }
}
