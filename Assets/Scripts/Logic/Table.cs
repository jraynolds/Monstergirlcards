using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Table : MonoBehaviour 
{
    public List<CreatureLogic> monstersOnTable = new List<CreatureLogic>();

    public void PlaceCreatureAt(int index, CreatureLogic creature)
    {
        monstersOnTable.Insert(index, creature);
    }
        
}
