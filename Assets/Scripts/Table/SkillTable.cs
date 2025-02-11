using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SkillData
{
    public int SkillAtk_ID { get; set; }
    public float SkillDmgMul { get; set; }
    public float Area {  get; set; }
    public int Enemy_Speed { get; set; }
    public int Duration { get; set; }
    public string Pjt {  get; set; }

}
public class SkillTable : DataTable
{
    private readonly Dictionary<int, SkillData> dictionary = new Dictionary<int, SkillData>();

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<SkillData>(textAsset.text);
        dictionary.Clear();
        

        foreach (var skill in list)
        {
            if (!dictionary.ContainsKey(skill.SkillAtk_ID))
            {
                dictionary.Add(skill.SkillAtk_ID, skill);
            }
            else
            {
                Debug.LogError($"Å° Áßº¹: {skill.SkillAtk_ID}");
            }
        }
    }
    public SkillData Get(int id)
    {
        if (!dictionary.ContainsKey(id))
        {
            return null;
        }
        return dictionary[id];
    }
    public EffectState GetSkillEffectState(float speed)
    {
        if(speed == 0)
        {
            return EffectState.Stun;
        }
        else if(speed == 100)
        {
            return EffectState.StrongAttack;
        }
        else
        {
            return EffectState.Slow;
        }
    }
    public AttackType GetAttackType(float area)
    {
        if(area == 0)
        {
            return AttackType.Single;
        }
        else
        {
            return AttackType.Multiple;
        }
    }
    public Dictionary<int, SkillData> GetAll()
    {
        return dictionary;
    }
}
