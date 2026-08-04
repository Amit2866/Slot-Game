using UnityEngine;

namespace SlotGame.Data
{
    // Create new symbols directly
    [CreateAssetMenu(fileName = "New Symbol", menuName = "Slot Game/Symbol Data")]
    public class SymbolData : ScriptableObject
    {
        public string id;                // Unique name for checking matches (e.g., "Cherry", "Seven")
        public Sprite icon;              
        public int payoutMultiplier = 1; // How much this specific symbol multiplies your bet
        public bool isWild = false;      // If true, this symbol can substitute for others
    }
}