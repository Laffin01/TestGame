using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{

    public float Hp;
    public float Damage;
    public float SuperAttackDamage = 5;
    public Button atackButton;
    public Button SuperAtackButton;
    public float AtackSpeed;
    public float AttackRange = 2;
    private float SpeedCharacter;
    private float lastAttackTime = 0;
    private bool isDead = false;
    public Animator AnimatorController;
    private float superAttackCooldown = 2f; 
    private float superAttackCooldownTimer = 0f;

    private void Start()
    {
        atackButton.onClick.AddListener(() => Attack(false)); 
        SuperAtackButton.onClick.AddListener(() => Attack(true));  
    }

    private void Update()
    {
        if (isDead) return;

        if (Hp <= 0)
        {
            Die();
            return;
        }

        bool nesrenemies = CheckforEnemies();
        SuperAtackButton.interactable = nesrenemies;
      
        Movement();
  if (superAttackCooldownTimer > 0)
        {
            superAttackCooldownTimer -= Time.deltaTime;

            
            SuperAtackButton.GetComponentInChildren<Text>().text = Mathf.Ceil(superAttackCooldownTimer).ToString();

            SuperAtackButton.interactable = false;

            if (superAttackCooldownTimer <= 0)
            { 
                SuperAtackButton.GetComponentInChildren<Text>().text = "Super Attack";
                SuperAtackButton.interactable = true;
            }
        }
     

    }
    private bool CheckforEnemies()
    {
        bool enemy = false;
        var enemies = SceneManager.Instance.MiniGoblins;
        var enemies1 = SceneManager.Instance.Enemies;

        foreach (var enemyObject in enemies)
        {
            if (enemyObject == null) continue; 

            var distance = Vector3.Distance(transform.position, enemyObject.transform.position);
            if (AttackRange >= distance)
            {
                enemy = true;
                break; 
            }
        }

        foreach (var enemyObject in enemies1)
        {
            if (enemyObject == null) continue;

            var distance = Vector3.Distance(transform.position, enemyObject.transform.position);
            if (AttackRange >= distance)
            {
                enemy = true;
                break; 
            }
        }
        return enemy;
    }

    private void Movement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 MoveDirection = new Vector3(moveX, 0, moveZ).normalized;

        if (MoveDirection.magnitude >= 0.1f)
        {

            transform.Translate(MoveDirection * SpeedCharacter * Time.deltaTime, Space.World);

            Quaternion targetRotation = Quaternion.LookRotation(MoveDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            SpeedCharacter = 5f;
            AnimatorController.SetFloat("SpeedCharacter", Mathf.Abs(SpeedCharacter));
          

        }
        else
        {
            SpeedCharacter = 0f;
            AnimatorController.SetFloat("SpeedCharacter", Mathf.Abs(SpeedCharacter));
        }
    }

    private void Attack(bool isSuperAttack)
    {
        float currentDamage = isSuperAttack ? SuperAttackDamage : Damage;
        if (isSuperAttack)
        {
            AnimatorController.SetTrigger("superattack");
            superAttackCooldownTimer = superAttackCooldown;
            SuperAtackButton.interactable = false;
        }

        if (Time.time - lastAttackTime > AtackSpeed)
        {
            if (!isSuperAttack)
            {
                AnimatorController.SetTrigger("Attack");
            }
            lastAttackTime = Time.time;

            var enemies = SceneManager.Instance.Enemies;
            var miniGoblins = SceneManager.Instance.MiniGoblins;
            GameObject closestEnemy = null;
            float closestDistance = Mathf.Infinity;

            foreach (var enemy in enemies)
            {
                if (enemy == null) continue;

                var distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy.gameObject;
                }
            }

            foreach (var miniGoblin in miniGoblins)
            {
                if (miniGoblin == null) continue;

                var distance = Vector3.Distance(transform.position, miniGoblin.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = miniGoblin.gameObject;
                }
            }

            if (closestEnemy != null && closestDistance <= AttackRange)
            {
                transform.rotation = Quaternion.LookRotation(closestEnemy.transform.position - transform.position);
                var enemyComponent = closestEnemy.GetComponent<Enemie>();
                if (enemyComponent != null)
                {
                    enemyComponent.Hp -= currentDamage;
                }
                else
                {
                    var miniGoblinComponent = closestEnemy.GetComponent<MiniGoblins>();
                    if (miniGoblinComponent != null)
                    {
                        miniGoblinComponent.Hp -= currentDamage;
                    }
                }
            }
        }
    }


    private void Die()
    {
        isDead = true;
        AnimatorController.SetTrigger("Die");
        SceneManager.Instance.GameOver();
    }
}
