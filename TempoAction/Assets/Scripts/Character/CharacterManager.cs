using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static List<GameObject> characters = new List<GameObject>();

    public static List<GameObject> GetCharacter(int layerMask)
    {
       List<GameObject> list = new List<GameObject>();

        list = characters.Where((obj) => (obj.layer & layerMask) != 0).ToList();
        
        return list;
    }
}
