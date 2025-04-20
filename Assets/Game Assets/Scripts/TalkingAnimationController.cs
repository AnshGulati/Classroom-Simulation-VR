using UnityEngine;
using System.Collections;

public class TalkingAnimationController : MonoBehaviour
{
    public Animator animator;
    public bool isTalking = false;

    public void StartTalkingDelayed(float delay, bool enable)
    {
        StartCoroutine(StartTalkingAfterDelay(delay, enable));
    }

    private IEnumerator StartTalkingAfterDelay(float delay, bool enable)
    {
        yield return new WaitForSeconds(delay);
        isTalking = enable;
        animator.SetBool("isTalking", enable);
    }

    //public void StopTalking()
    //{
    //    isTalking = false;
    //    animator.SetBool("isTalking", false);
    //}
}
