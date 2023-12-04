using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scary_Face : MonoBehaviour
{
public PlayerAttack playerAttack;
    public RangeVisual rangeVisual;
    public float range;

    void ScaryFaceEffect()
    {
        rangeVisual.CalculateScale(range);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, range);

        FindObjectOfType<AudioManager>().PlaySound("ScaryFace");

        if (hitEnemies != null && hitEnemies.Length > 0)
        {
            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
                    enemyStats.condition = EnemyCondition.slowed;

                    EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
                    enemyMovement.movementSpeed *= .75f;

                }
            }
        }

        
    }

    void EndAnimation()
    {
        playerAttack.EndStatusConditionAttack();
    }

}
