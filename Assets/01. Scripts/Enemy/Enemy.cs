using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    // ----------- [ SerializeField Field ] -----------
    [SerializeField] private int maxHp;

    // ----------- [ Private Field ] -----------
    private int curHp;
    
    private void Start()
    {
        curHp = maxHp;
    }

    public void TakeDamage(int damage)
    {
        curHp -= damage;

        if(curHp <= 0)
        {
            Die();
        }
    }

    public abstract void Attack();

    private void Die()
    {
        WaveManager.Instance.RemoveActiveEnemy(this);
        WaveManager.Instance.CheckRemainingMonstersForNextWave();

        Destroy(gameObject);
    }
}
