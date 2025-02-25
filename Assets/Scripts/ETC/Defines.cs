public static class DataTableIds
{
    public static readonly string Enemy = "EnemyTable";
    public static readonly string Wave = "WaveTable";
    public static readonly string Tower = "TowerTable";
    public static readonly string Upgrade = "UpgradeTable";
    public static readonly string Skill = "SkillTable";
    public static readonly string Quest = "QuestTable";
    public static readonly string String = "StringTable";
}
public enum TowerRarity
{
    Common = 1,
    Rare,
    Hero,
    Legendary,
    God
}
public enum TowerType
{ 
    Human = 1,
    Machine,
    Monster
}
public enum ResourceType
{
    None = 0,
    Terazin,
    Mineral,
    Gas
}
public enum EnemyType
{
    Common,
    Boss,
    MissionBoss
}
public enum EffectState 
{ 
    None = -1, 
    StrongAttack,
    Slow, 
    Stun 
}
public enum AttackType
{
    Single,
    Multiple
}