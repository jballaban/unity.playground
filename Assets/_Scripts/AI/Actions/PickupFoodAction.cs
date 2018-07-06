using UnityEngine;

public class PickupFoodAction : ActionBase
{
    void Start()
    {
        this.PreConditions["hungry"] = true;
        this.Effects["hungry"] = false;
    }

    public override bool Perform(GameObject actor)
    {
        throw new System.NotImplementedException();
    }
}