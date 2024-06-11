using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition health;
    public Condition hunger;
    public Condition stamina;
    public Condition thirst;
    public Condition warmth;

    void Start()
    {
        CharacterManager.Instance.Player.Condition.uiCondition = this;
    }
}
