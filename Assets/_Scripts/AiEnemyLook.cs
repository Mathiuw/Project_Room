using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AiEnemyLook : MonoBehaviour
{
    [SerializeField] Transform IKTarget;   
    [Range(0f, 1f)][SerializeField] float IkBodyWeight;
    [SerializeField] MultiAimConstraint IKBody;
    [Range(0f, 1f)][SerializeField] float IkHeadWeight;
    [SerializeField] MultiAimConstraint IKHead;
    EnemyAi enemyAi;

    void Awake()
    {
        enemyAi = GetComponent<EnemyAi>();
        IKBody.weight = 0;
        IKHead.weight = 0;     
    }

    void Update()
    {
        LookAtTarget();
    }

    void LookAtTarget() 
    {
        if (enemyAi.canSeeTarget)
        {
            //Estabelece os weights do IK
            IKBody.weight = IkBodyWeight;
            IKHead.weight = IkHeadWeight;

            //Posiciona o target do IK no alvo do inimigo
            IKTarget.position = enemyAi.target.position;
        }
        else 
        {
            IKBody.weight = 0;
            IKHead.weight = 0;
        } 
    }
}
