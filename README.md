# 🃏 MemoCards Demo

A classic memory card matching game built with C# and Windows Forms.

## How to Play

1. Hit **New Game** — cards briefly flash face-up so you can peek, then flip back down
2. Click any card to reveal it
3. Click a second card — if they match, they disappear. If not, they flip back
4. Match all 6 pairs before time runs out or you hit the move limit

## Rules

| Constraint | Value |
|---|---|
| Cards | 12 (6 pairs) |
| Time limit | 1:40 |
| Move limit | 30 |
| Score per match | +100 |

## Getting Started

Open `MemoCardsDemo/MemoCardsDemo.sln` in Visual Studio and hit **F5**.

Requires .NET Framework 4.8.

## Project Structure

```
MemoCardsDemo/
├── Form1.cs              # Game logic
├── Form1.Designer.cs     # UI layout
├── Resources/            # Card images (0–5.jpg, back.png)
└── Program.cs            # Entry point
```
