using UnityEngine;

[CreateAssetMenu(fileName = "NewSymbol", menuName = "Slot Machine/Symbol Data")]
public class SymbolData : ScriptableObject
{
    public string symbolId;
    public Sprite icon;

    [Tooltip("Payout multiplier applied when matching 3 of this symbol.")]
    public int payoutMultiplier = 5;
}