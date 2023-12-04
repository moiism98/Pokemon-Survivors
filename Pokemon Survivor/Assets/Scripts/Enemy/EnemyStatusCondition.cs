using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusCondition : MonoBehaviour
{
    public List<RuntimeAnimatorController> controllers = new List<RuntimeAnimatorController>();
    public SpriteRenderer sprite;
    public Animator animator;
    public EnemyStats stats;

    void Update()
    {
        if(stats != null)
        {
            switch (stats.condition)
            {
                case EnemyCondition.healthy:

                    animator.enabled = false;
                    sprite.enabled = false;

                break;

                case EnemyCondition.slowed: ApplyStatusCondition(0); break;
                case EnemyCondition.damageUp: ApplyStatusCondition(1); break;
                case EnemyCondition.speedUp: ApplyStatusCondition(2); break;
                case EnemyCondition.defenseUp: ApplyStatusCondition(3); break;
            }
        }
    }

    void ApplyStatusCondition(int animatorController)
    {
        animator.enabled = true;
        animator.runtimeAnimatorController = controllers[animatorController];
        sprite.enabled = true;
    }
}
