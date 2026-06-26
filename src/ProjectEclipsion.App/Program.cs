using ProjectEclipsion.App.Input;
using ProjectEclipsion.App.Rendering;
using ProjectEclipsion.Core;
using ProjectEclipsion.Core.Gameplay.Player;
using ProjectEclipsion.Core.Gameplay.Weapons;
using System;

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

// 2026-06-06
// ステップ1-3

// このコードの流れは以下の通りになる
// while (true) で無限ループ

// ゲームを続ける限り同じ処理を繰り返します。
// break が実行されるとループを抜けます。
// if (!Console.IsInputRedirected && !Console.IsOutputRedirected) { Console.Clear(); }

// 入出力がリダイレクトされていない通常のコンソール環境なら、画面をクリアします。
// これにより毎フレーム、古い表示を消して新しい画面が見やすくなります。
// gameLoop.RunOnce();

// 1フレーム分のゲーム更新と描画処理を実行します。
// Console.WriteLine();

// 空行を入れて見た目を整えます。
// Console.WriteLine("WASDで移動 / QまたはEscで終了");

// 操作方法の説明を画面に表示します。
// var direction = keyboardInput.ReadDirection();

// キーボードから入力を受け取り、移動方向や終了要求を取得します。
// if (direction.ShouldExit) { break; }

// Q か Esc が押されたらループを抜けてゲームを終了します。
// gameState.MovePlayer(direction.DirectionX, direction.DirectionY);

// 入力に応じてプレイヤーを移動させます。
// まとめ
// ループごとに画面を更新し、
// キー入力を受け取って、
// 終了命令なら抜け、
// それ以外ならプレイヤーを移動する。
// つまり「描画→操作入力→終了判定→移動」を繰り返す、ゲームのメインループです。

// 2026-06-08
// ステップ1-4

while (true)
{
  if (!Console.IsInputRedirected && !Console.IsOutputRedirected)
  {
    Console.Clear();
  }

  gameLoop.RunOnce();
  Console.WriteLine();
  Console.WriteLine("WASDで移動 / IJKLで部屋移動 / Mでミニマップ / 1-6で武器切り替え / 7-9でスキル解放 / Spaceで発射 / Gで取得 / Eで装備 / B/F/H/C/Vで状態異常 / Tで10ダメージ / QまたはEscで終了");

  var direction = keyboardInput.ReadDirection();
  if (direction.ShouldExit)
  {
    break;
  }

  if (direction.ShouldDamagePlayer)
  {
    gameState.DamagePlayer(10);
  }

  if (direction.ShouldFireBullet)
  {
    gameState.FireCurrentWeapon();
  }

  if (direction.WeaponNumber > 0)
  {
    gameState.SwitchWeapon(direction.WeaponNumber);
  }

  if (direction.ShouldPickUpItem)
  {
    gameState.PickUpFirstDroppedItem();
  }

  if (direction.ShouldEquipItem)
  {
    gameState.EquipFirstInventoryItem();
  }

  if (direction.StatusEffectType.HasValue)
  {
    gameState.ApplyStatusEffectToFirstEnemy(direction.StatusEffectType.Value);
  }

  if (direction.SkillTreeType.HasValue)
  {
    gameState.UnlockFirstSkill(direction.SkillTreeType.Value);
  }

  if (direction.RoomDirection.HasValue)
  {
    gameState.MoveRoom(direction.RoomDirection.Value);
  }

  if (direction.ShouldToggleMiniMap)
  {
    gameState.ToggleMiniMap();
  }

  //  public void FireCurrentWeapon()
  //{
  //  var bullet = CurrentWeapon.Fire(Player.X, Player.Y, directionX: 1, directionY: 0);
  //  Bullets.Add(bullet);
  //}

  gameState.MovePlayer(direction.DirectionX, direction.DirectionY);
  gameState.Update();
}
