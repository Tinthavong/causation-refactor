using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    Animator animator;
    EnemyDetection enemyDetection;
    EnemyStateManager enemyStateManager;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        enemyDetection = GetComponent<EnemyDetection>();
        enemyStateManager = GetComponent<EnemyStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
