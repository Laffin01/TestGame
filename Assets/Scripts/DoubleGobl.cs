using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleGobl : Enemie
{
 

    public void Spawn ()
        {
        SceneManager.Instance.AddDoubleGobl(this);
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
            // Agent.isStopped = true;
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
        SceneManager.Instance.RemoveDoubleGobl(this);
        isDead = true;
        AnimatorController.SetTrigger("Die");
    }

}


