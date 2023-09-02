using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [System.Serializable]
public class Dialogue
{
    public string name;
    
    [TextArea(3,10)]
    public string[] sentences;

    [Space(10)]
    public string quest;

    [Space(10)]
    public bool isQuest;
    public bool isScreenText;
    public bool doesEndCutscene;
}
