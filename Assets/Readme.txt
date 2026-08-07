Game Overview

Unity Slot Machine Game

This project is a fully functional 3-reel slot machine developed in Unity 6 version - 6000.3.21f1

Key Features:

ScriptableObject Symbols: Decoupled symbol assets (SymbolData) that allow easy creation of custom icons, IDs, and unique payout multipliers.

Interactive Bet Panel: Automatically opens at game launch, locks and hides during spins, and re-opens as soon as reels finish stopping.

Dynamic Economy & Balance Tracking: Players start with 1000 Gold and can choose between different bet increments.

Out-of-Gold & Restart System: Automatically detects when a player's balance drops below the minimum required bet, triggering a dedicated restart screen and WebGL session reset handler.

Flexible Controls: Players can spin by clicking an interactive UI lever handle.

Smooth Animations: Features smooth continuous vertical scrolling combined with elastic spring overshoot bounce physics on reel stops.

------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
# Instructions to Play

## Windows

1. Open **Command Prompt** or **PowerShell**.
2. Navigate to your Unity project's **Build** folder (the folder containing `index.html` and the `Build` directory):
   
    cmd-
    
    cd "C:\SlotGame\Build"
   
3. Start a local web server:
   
      cmd-
   
      py -m http.server
   
4. Open your web browser and go to:
   
   http://localhost:8000
   
5. The WebGL game will load in your browser.

> **Note:** Keep the Command Prompt or PowerShell window open while playing. Press **Ctrl + C** to stop the server when you're finished.