using UnityEngine;
using System.Collections;

public class AnimationHandler: MonoBehaviour
{
    public static void Trigger(string boolName, Animator animator, MonoBehaviour coroutineRunner)
    {
        animator.SetBool(boolName, true);
       coroutineRunner.StartCoroutine(ResetBoolAfterAnimation(animator, boolName));
       
    }

    private static IEnumerator ResetBoolAfterAnimation(Animator animator, string boolName)
    {
        // Wait for the length of the current animation
        float waitTime = GetWaitTimeSpecifick(animator, boolName);
        yield return new WaitForSeconds(0.5f);

        animator.SetBool(boolName, false);
    }

    public static void SetTrueBool(string value, Animator animator)
    {
        animator.SetBool(value, true);
    }
    public static float GetWaitTime(Animator animator)
    {
       return animator.GetCurrentAnimatorStateInfo(0).length;
    }
    public static float GetWaitTimeSpecifick(Animator animator, string animationName)
    {
   
        RuntimeAnimatorController rac = animator.runtimeAnimatorController;

        foreach (AnimationClip clip in rac.animationClips)
        {
            if (clip.name == animationName)
            {
                return clip.length; 
            }
        }

 
        return 0f;
    }
    public static void ResetBoolAnimation(string value, Animator animator)
    {
        animator.SetBool(value, false);
    }
}
