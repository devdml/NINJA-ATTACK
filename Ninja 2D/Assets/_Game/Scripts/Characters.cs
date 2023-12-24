using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characters : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] protected HealthBar healthBar;
    [SerializeField] protected CombatText CombatTextPrefab;
    private float hp;
    public bool isDead => hp <= 0;
    private string currentAnimName;

    private void Start()
    {
        OnInit();
    }
    public virtual void OnInit()
    {
        hp = 100;
        healthBar.OnInit(100, transform);
    }
    public virtual void OnDespawn()
    {

    }
    public void OnHit(float damage)
    {
        if (!isDead)
        {
            hp -= damage;
            if (isDead)
            {
                hp = 0;
                OnDeath();
            }
            healthBar.SetNewHp(hp);
            Instantiate(CombatTextPrefab, transform.position + Vector3.up, Quaternion.identity).OnInit(damage);
        }
    }
    protected void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName); 
        }
    }
    protected virtual void OnDeath()
    {
        ChangeAnim("die");
        Invoke(nameof(OnDespawn), 2f);
    }

}
