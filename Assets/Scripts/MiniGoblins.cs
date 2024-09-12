using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MiniGoblins : Enemie
{
    public float ReducedHp;
    public float ReducedDamage ;
    public float ReducedAttackSpeed ;
        DoubleGobl d ;


    private void Start()
    {

        SceneManager.Instance.AddMiniGoblins(this);
        d = new DoubleGobl();
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        if (Hp <= 0)
        {

            Die();
          //  Agent.isStopped = true;
            return;
        }

        var distance = Vector3.Distance(transform.position, SceneManager.Instance.Player.transform.position);

        if (distance <= AttackRange)
        {
            Agent.isStopped = true;
            if (Time.time - lastAttackTime > AtackSpeed)
            {
                lastAttackTime = Time.time;
                SceneManager.Instance.Player.Hp -= Damage;
                AnimatorController.SetTrigger("Attack");
            }
        }
        else
        {
            Agent.isStopped = false;
            Agent.SetDestination(SceneManager.Instance.Player.transform.position);
        }
        AnimatorController.SetFloat("SpeedCharacter", Agent.speed);
        Debug.Log(Agent.speed);
    }

    private void Die()
    {

       
        Vector3 posDeath = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        SceneManager.Instance.RemoveMiniGoblins(this, posDeath);
        isDead = true;
        AnimatorController.SetTrigger("Die");
    d.Spawn();
    }
}
