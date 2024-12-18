using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Data;
using Scripts.Entity;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "ScriptableObjects/TempPlayerData", order = 1)]
public class TempPlayerData : ScriptableObject
{
    [SerializeField] private ClassType _classType; // 직업 유형
    [SerializeField] private int _level;
    [SerializeField] private int _exp;
    [SerializeField] private float _maxHP;
    [SerializeField] private float _maxMP;
    [SerializeField] private float _currentHP;
    [SerializeField] private float _currentMP;
    [SerializeField] private int _atk;
    [SerializeField] private int _def;
    [SerializeField] private int _mdef;
    [SerializeField] private int _agi;
    [SerializeField] private int _acc; // 적중률
    [SerializeField] private int _avo; // 회피율
    [SerializeField] private int _cri; // 크리티컬 확률
    [SerializeField] private int _str_cor; // 근력 보정
    [SerializeField] private int _mag_cor; // 마법 보정
    [SerializeField] private int _position; // 파티 내의 위치  6 이면 파티에 없음

    // 값에 접근할 때 사용하는 메서드나 프로퍼티 (소문자)
    public ClassType classType => _classType;
    public int level => _level;
    public int exp => _exp;
    public float maxHP => _maxHP;
    public float maxMP => _maxMP;
    public float currentHP => _currentHP;
    public float currentMP => _currentMP;
    public int atk => _atk;
    public int def => _def;
    public int mdef => _mdef;
    public float agi => _agi;
    public int acc => _acc;
    public int avo => _avo;
    public int cri => _cri;
    public int str_cor => _str_cor;
    public int mag_cor => _mag_cor;
    public int position => _position;
}
