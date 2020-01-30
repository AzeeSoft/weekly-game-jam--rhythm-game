using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Data", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    public string levelName => name;

    public string levelKey;
    public string sceneName;
}
