using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyStatData
{
    /// <summary>
    /// �� �̸�
    /// </summary>
    public string name;
    /// <summary>
    /// �⺻ ü��
    /// </summary>
    public float hp;
    /// <summary>
    /// �⺻ ���ݷ�
    /// </summary>
    public float atk;
    /// <summary>
    /// �⺻ ����
    /// </summary>
    public float def;
    /// <summary>
    /// �⺻ �Ӽ������
    /// </summary>
    public float mdef;

    /// <summary>
    /// ���߷�
    /// </summary>
    public float accuracy;
    /// <summary>
    /// ȸ�Ƿ�
    /// </summary>
    public float dodge;
    /// <summary>
    /// ũ��Ȯ��
    /// </summary>
    public float critical;
    /// <summary>
    /// �������׷�
    /// </summary>
    public float strcret;
    ///<summary>
    ///�������׷�
    ///</summary>
    public float magcret;

    /// <summary>
    /// �⺻ �ٷ�
    /// </summary>
    public float str;
    /// <summary>
    /// �⺻ �����
    /// </summary>
    public float vit;
    /// <summary>
    /// �⺻ ������
    /// </summary>
    public float mag;
    /// <summary>
    /// �⺻ ��ø
    /// </summary>
    public float agi;
    /// <summary>
    /// �⺻ ��
    /// </summary>
    public float luk;
}
