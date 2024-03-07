using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class CharaterDatabase : ScriptableObject
{
    // Start is called before the first frame update
    public Character[] characters;

    public int CharaterCount
    {
        get
        {
            return characters.Length;
        }
    }

    public Character GetCharacter(int index)
    {
        return characters[index];
    }
}
