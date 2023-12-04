using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAniamtorTrigger : MonoBehaviour
{
    public Animator evoAnimator;
    public Animator shinyAnimator;
    public void ResetEvoTrigger()
    {
        evoAnimator.ResetTrigger("Start");
        evoAnimator.gameObject.SetActive(false);
    }

    public void ResetShinyTrigger()
    {
        shinyAnimator.ResetTrigger("Start");
        shinyAnimator.gameObject.SetActive(false);
    }
}
