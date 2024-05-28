using System.Collections.Generic;

[System.Serializable]
public class Stat
{
    public int baseValue;

    public List<int> modifiers = new List<int>();
    public int GetValue()
    {
        int finalValue = baseValue;
        modifiers.ForEach(x => finalValue += x);
        return finalValue;
    }

    public void AddModifier(int modifier)
    {
        modifiers.Add(modifier);
    }

    public void RemoveModifier(int modifier)
    {
        modifiers.Remove(modifier);
    }

}
