using UnityEngine;

namespace SlotGame.Data
{
    [CreateAssetMenu(fileName = "NewSymbolData", menuName = "Slot Machine/Symbol Data")]
    public class SymbolData : ScriptableObject
    {
        [Header("Symbol Configuration")]
        public string symbolID;          // e.g., "Seven", "Cherry"
        public Sprite symbolIcon;        // Drag slot-symbol1.png or slot-symbol2.png
        public int payoutMultiplier = 5; // Multiplier awarded on 3 matching symbols
    }
}