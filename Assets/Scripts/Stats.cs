using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats {
    //hp
    public int Hp;
    //points attack and defense 
    public int AttackPoints;
    public int DefensePoints;
    // value for attack and defense
    public List<int> PowerZoneAttacks;
    public List<bool> PowerZoneDefense;
    
}
