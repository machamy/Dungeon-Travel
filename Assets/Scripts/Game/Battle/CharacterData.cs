using Scripts.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBattleStat", menuName = "CreateBattleStat")]
public class CharacterData : ScriptableObject
{
    [SerializeField] private ClassType _classType; // 직업 유형
    [SerializeField] private string _unitName;
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
    [SerializeField] private int _position; // 파티 내의 위치  -1 이면 파티에 없음
    [SerializeField] private bool _isDie;
    [SerializeField] private BattleSkill[] _skill = new BattleSkill[4];

    // 값에 접근할 때 사용
    public ClassType classType => _classType;
    public string unitName => _unitName;
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
    public bool isDie => _isDie;
    public BattleSkill[] skill => _skill;
    public void SetStatus(float hp, float mp)
    {
        this._currentHP = hp;
        this._currentMP = mp;
    }
}
