using UnityEngine;
using System.Collections;

public class TalkingAnimationController : MonoBehaviour
{
    public Animator animator;
    public bool isTalking = false;

    public void StartTalkingDelayed(float delay)
    {
        StartCoroutine(StartTalkingAfterDelay(delay));
    }

    private IEnumerator StartTalkingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isTalking = true;
        animator.SetBool("isTalking", true);
    }

    public void StopTalking()
    {
        isTalking = false;
        animator.SetBool("isTalking", false);
    }
}
