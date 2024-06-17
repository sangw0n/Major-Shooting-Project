using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    // ----------- [ SerializeField Field ] -----------
    [SerializeField] private Vector3      detectorSize;                 // 탐지기 크기
    [SerializeField] private Vector3      detectorPosOffset;            // 탐지기 위치 오프셋 
    [SerializeField] private LayerMask    detectionLayer;               // 탐지할 특정 레이어 

    // ----------- [ Private Field ] -----------
    private GameObject                    closeEnemy;                   // 가까운 적
    private bool                          isDetecting;                  // 탐지중인지 검사 

    // ----------- [ Property Field ] -----------
    public  bool                          IsDetecting => isDetecting;   // isDetecting 프로퍼티 ( Get 기능 )
    public  GameObject                    CloseEnemy  => closeEnemy;    // closeEnemy  프로퍼티 ( Get 기능 )

    private void Update()
    {
        HandleDetectionInput();

        if( isDetecting ) 
        {
            closeEnemy = DetectCloseEnemy();
        }
    }

    ///<summary> 제일 가까운 적을 탐지하는 함수 </summary>
    private GameObject DetectCloseEnemy()
    {
        Collider2D[] enemyInField       = Physics2D.OverlapBoxAll(transform.position + detectorPosOffset, detectorSize, detectionLayer);
        float        targetDistance     = float.MaxValue;
        GameObject   result             = null;

        foreach(Collider2D enemy in enemyInField)
        {
            float  distanceToEnemy = Vector3.Distance(enemy.transform.position, transform.position);       // 플레이어와 적의 거리

            if( distanceToEnemy < targetDistance )                                                         // 플레이어와 적의 거리가 현재 타겟과의 거리보다 가까우면
            {
                targetDistance = distanceToEnemy;                                                          
                result         = enemy.gameObject;                                                        
            }
        }

        return result;
    }    

    ///<summary> 탐지 입력을 처리하는 함수 </summary>
    private void HandleDetectionInput()
    {
        if(Input.GetKey(KeyCode.C))
        {
            isDetecting = true;
        }
        else if(Input.GetKeyUp(KeyCode.C))
        {
            isDetecting = false;
            closeEnemy  = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + detectorPosOffset, detectorSize);
    }
}
