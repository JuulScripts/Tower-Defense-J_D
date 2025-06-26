using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class AnimationHandler : MonoBehaviour
{
    public static void Trigger(string boolName, Animator animator) // Triggers an animation parameter by name.
    {
        animator.SetTrigger(boolName);
    }

    private static IEnumerator ResetBoolAfterAnimation(Animator animator, string boolName) // Waits for a specific animation to finish, then resets its bool parameter.
    {
        float waitTime = GetWaitTimeSpecifick(animator, boolName);
        yield return new WaitForSeconds(0.1f);

        animator.SetBool(boolName, false);
    }

    public static void SetTrueBool(string value, Animator animator) // Sets a boolean parameter to true in the animator.
    {
        animator.SetBool(value, true);
    }
    public static float GetWaitTime(Animator animator) //Returns the length of the current animation state.
    {
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }
    public static float GetWaitTimeSpecifick(Animator animator, string animationName) // Returns the length of a specific animation clip by name.
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
    public static void ResetBoolAnimation(string value, Animator animator) // Sets an animation boolean parameter to false.
    {
        animator.SetBool(value, false);
    }
}