using UnityEngine;

[CreateAssetMenu(fileName = "NewSymbol", menuName = "Slot Machine/Symbol Data")]
// Defines a custom asset type in Unity that acts as a data container.
// This decouples symbol properties (art, ID, math) from the scene, making it easy to add new symbols
public class SymbolData : ScriptableObject
{
    public string symbolId;
    public Sprite icon;

    [Tooltip("Payout multiplier applied when matching 3 of this symbol.")]
    public int payoutMultiplier = 5;
}