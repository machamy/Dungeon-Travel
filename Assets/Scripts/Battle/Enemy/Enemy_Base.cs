using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Base : MonoBehaviour
{
    public int EnemyLevel; // �� ����
    public int EnemyDamage; // �� ���ݷ�
    public int EnemyMaxHealth; // �� �ִ�ü��
    public int EnemyCurrentHealth; // �� ����ü��
    public int EnemyAgility; // �� ��ø            
    public int EnemyMana; // �� ����                    // �� �������� ��ġ ����Ƽ �ν�����â���� ����

    public enum AttackType
    {
        Damage, // Ÿ��
        Penetrate, // ����
        Slash, // ����
    }
    public virtual void EnemyAttack(AttackType type) // �������̵�
    {
        switch (type) // ����Ÿ�Կ� ���� �и� //�������̵����� ���� ���ɼ� ����
        {
            case AttackType.Damage:
                Damaged();
                break;
            case AttackType.Penetrate:
                Penetrate();
                break;
            case AttackType.Slash:
                Slash();
                break;
        }
        //�ٸ� �ΰ�ȿ�� ����
    }
    public void Damaged()
    {

    }
    public void Penetrate()
    {

    }
    public void Slash()
    {

    }
    public void Skill1()
    {

    }
}
