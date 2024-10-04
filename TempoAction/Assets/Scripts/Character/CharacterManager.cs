using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterManager : Singleton<CharacterManager>
{
    public List<GameObject> characters = new List<GameObject>();

    public List<GameObject> GetCharacter(int layerMask)
    {
       List<GameObject> list = new List<GameObject>();

        list = characters.Where((obj) => (1 << obj.layer & layerMask) != 0).ToList();
        
        return list;
    }
}
