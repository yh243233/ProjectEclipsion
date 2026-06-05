using System;
using ProjectEclipsion.App.Input;
using ProjectEclipsion.App.Rendering;
using ProjectEclipsion.Core;

// namespaceの宣言はプロジェクト/フォルダの順になる

// ステップ1－1
// 2026-05-29
// このゲームの基本的な流れは以下の通りになる
// Program.cs
//   ↓
// GameState を作る
//   ↓
// ConsoleRenderer を作る
//   ↓
// GameLoop に渡す
//   ↓
// GameLoop が Renderer に描画をお願いする
//   ↓
// Console にタイトルが表示される
// 入口は Program.cs (line 5) です。

// 2026-06-05
// ステップ1-2
// 基本的な流れはステップ1－1と同じ。
// 今回はステータス部分に更新がかかり、最大HP、現在のHP、最大シールド、現在のシールド、移動速度を管理するPlayerStatsクラスが追加されている。
// 具体的にはゲームステータスのインスタンスを生成するタイミングでプレイヤークラスのインスタンスも生成して管理する事となっている。
var gameState = new GameState();
var renderer = new ConsoleRenderer();
var gameLoop = new GameLoop(gameState, renderer);
var keyboardInput = new KeyboardInput();

while (true)
{
    if (!Console.IsInputRedirected && !Console.IsOutputRedirected)
    {
        Console.Clear();
    }

    gameLoop.RunOnce();
    Console.WriteLine();
    Console.WriteLine("WASDで移動 / QまたはEscで終了");

    var direction = keyboardInput.ReadDirection();
    if (direction.ShouldExit)
    {
        break;
    }

    gameState.MovePlayer(direction.DirectionX, direction.DirectionY);
}
