using System;
using System.IO;
using ProjectEclipsion.Core.Gameplay.Skills;
using ProjectEclipsion.Core.Gameplay.StatusEffects;

namespace ProjectEclipsion.App.Input;

public sealed class KeyboardInput
{
    // ここの箇所はReadDirectionは戻り値の型ではなくメソッド名である。
    // int DirectionX, int DirectionY, bool ShouldExitの箇所のintなどは引数の型宣言ではなく返り値の型宣言である。
    public (int DirectionX, int DirectionY, bool ShouldExit, bool ShouldDamagePlayer, bool ShouldFireBullet, int WeaponNumber, bool ShouldPickUpItem, bool ShouldEquipItem, StatusEffectType? StatusEffectType, SkillTreeType? SkillTreeType) ReadDirection()
    {
        // ブレークポイントでのデバッグがうまくいかないので調べる。
        // todo:Console.IsInputRedirectedはコンソールの入力がリダイレクトされているかどうかを示すプロパティで、trueの場合はリダイレクトされていることを意味する。
        // true = コンソール（パイプやファイルなどキーボード以外からの入力） ← リダイレクトされている。
        // false = キーボードからの直接入力 ← リダイレクトされていない＝通常の状態。
        // リダイレクト = 流れを別の場所に向ける。入力元を切り替える。
        // 元々は「方向を変える」という意味です。
        // つまり、本来の入力元（プレイヤーの操作）を別の入力元（テストデータ）に置き換えるということです。
        // リダイレクトされている場合は、キーボードではなくパイプやファイルなど別の入力元から来ています。
        // デバッグ用の入力を提供するために、リダイレクトされた入力を処理する必要があります。
        if (Console.IsInputRedirected)
        {
            // Console.In.Read();
            // リダイレクトされた入力を1文字分読み取ります。
            // 読める文字がない場合は -1 が返ります。
            var input = Console.In.Read();

            // input < 0 の場合は、入力が終わった／ないと判断して ShouldExit = true を返します。
            // そうでなければ、その文字を ToDirection に渡して移動方向や終了フラグを返します。
            return input < 0
                ? (0, 0, true, false, false, 0, false, false, null, null)
            // ToDirectionは下の箇所で定義されているメソッドで、char型の引数を受け取り、移動方向と終了フラグを返すものです。
                : ToDirection((char)input);
            // つまりまとめると、リダイレクトされた入力がある場合は、キーボードからの入力を待たずに、リダイレクトされた入力を処理してゲームを終了するか、移動方向を決定することになる。
        }
        // デバッグ用
        // Console.WriteLine(Console.IsInputRedirected);
        // リダイレクトされていない場合、ユーザーがキーボードを押すのを待ちます。
        // intercept: true により、押したキーは画面に表示されません。

        // Console.ReadKey(...) は、ユーザーがキーを押すのを待って、押されたキーに関する情報を返すメソッドです。
        // intercept: true を指定すると、押されたキーがコンソール画面に表示されません。
        // 通常 ReadKey() は入力した文字をそのまま画面に表示します。
        // intercept: true にすると「入力を隠す」動作になります。パスワード入力などで使われる設定です。

        // まとめ
        // Console.ReadKey(intercept: true)：キー入力を受け取りつつ、画面には表示しない
        // .Key：押されたキーの種類だけを取り出す
        var key = Console.ReadKey(intercept: true).Key;

        return key switch
        {
          // ConsoleKey.W => (0, -1, false)

          // W キーなら、上方向に移動を指示
          // x=0, y=-1, cancel=false
          // ConsoleKey.S => (0, 1, false)

          // S キーなら、下方向に移動
          // x=0, y=1, cancel=false
          // ConsoleKey.A => (-1, 0, false)

          // A キーなら、左方向に移動
          // x=-1, y=0, cancel=false
          // ConsoleKey.D => (1, 0, false)

          // D キーなら、右方向に移動
          // x=1, y=0, cancel=false
          // ConsoleKey.Q => (0, 0, true)

          // Q キーなら移動しないが、キャンセルや終了を示す true
          // x=0, y=0, cancel=true
          // ConsoleKey.Escape => (0, 0, true)

          // Escape キーも移動せずに取消・終了扱い
          // x=0, y=0, cancel=true
          // _ => (0, 0, false)

          // ここの戻り値がint DirectionX,
          // int DirectionY,
          // bool ShouldExit,
          // bool ShouldDamagePlayer,
          // bool ShouldFireBullet
          // の内容になっている。

            ConsoleKey.W => (0, -1, false, false, false, 0, false, false, null, null),
            ConsoleKey.S => (0, 1, false, false, false, 0, false, false, null, null),
            ConsoleKey.A => (-1, 0, false, false, false, 0, false, false, null, null),
            ConsoleKey.D => (1, 0, false, false, false, 0, false, false, null, null),
            ConsoleKey.T => (0, 0, false, true, false, 0, false, false, null, null),
            ConsoleKey.G => (0, 0, false, false, false, 0, true, false, null, null),
            ConsoleKey.E => (0, 0, false, false, false, 0, false, true, null, null),
            ConsoleKey.B => (0, 0, false, false, false, 0, false, false, StatusEffectType.Burn, null),
            ConsoleKey.F => (0, 0, false, false, false, 0, false, false, StatusEffectType.Freeze, null),
            ConsoleKey.H => (0, 0, false, false, false, 0, false, false, StatusEffectType.Shock, null),
            ConsoleKey.C => (0, 0, false, false, false, 0, false, false, StatusEffectType.Corrosion, null),
            ConsoleKey.V => (0, 0, false, false, false, 0, false, false, StatusEffectType.Virus, null),
            ConsoleKey.Spacebar => (0, 0, false, false, true, 0, false, false, null, null),
            ConsoleKey.D1 => (0, 0, false, false, false, 1, false, false, null, null),
            ConsoleKey.D2 => (0, 0, false, false, false, 2, false, false, null, null),
            ConsoleKey.D3 => (0, 0, false, false, false, 3, false, false, null, null),
            ConsoleKey.D4 => (0, 0, false, false, false, 4, false, false, null, null),
            ConsoleKey.D5 => (0, 0, false, false, false, 5, false, false, null, null),
            ConsoleKey.D6 => (0, 0, false, false, false, 6, false, false, null, null),
            ConsoleKey.D7 => (0, 0, false, false, false, 0, false, false, null, SkillTreeType.Combat),
            ConsoleKey.D8 => (0, 0, false, false, false, 0, false, false, null, SkillTreeType.Tech),
            ConsoleKey.D9 => (0, 0, false, false, false, 0, false, false, null, SkillTreeType.Survival),
            ConsoleKey.NumPad1 => (0, 0, false, false, false, 1, false, false, null, null),
            ConsoleKey.NumPad2 => (0, 0, false, false, false, 2, false, false, null, null),
            ConsoleKey.NumPad3 => (0, 0, false, false, false, 3, false, false, null, null),
            ConsoleKey.NumPad4 => (0, 0, false, false, false, 4, false, false, null, null),
            ConsoleKey.NumPad5 => (0, 0, false, false, false, 5, false, false, null, null),
            ConsoleKey.NumPad6 => (0, 0, false, false, false, 6, false, false, null, null),
            ConsoleKey.NumPad7 => (0, 0, false, false, false, 0, false, false, null, SkillTreeType.Combat),
            ConsoleKey.NumPad8 => (0, 0, false, false, false, 0, false, false, null, SkillTreeType.Tech),
            ConsoleKey.NumPad9 => (0, 0, false, false, false, 0, false, false, null, SkillTreeType.Survival),
            ConsoleKey.Q => (0, 0, true, false, false, 0, false, false, null, null),
            ConsoleKey.Escape => (0, 0, true, false, false, 0, false, false, null, null),
            // ここはそれ以外のキーが押された場合のデフォルトの動作を定義している。今回は移動なし、終了なしの状態を返す。
            _ => (0, 0, false, false, false, 0, false, false, null, null),
        };
    }

    // こっちはデバッグ用
    // using System;
    // todo:pace ProjectEclipsion.App.Input;の二つしかネームスペース宣言をしていないのになぜリターン時に
    // char.ToUpperInvariantが使えているのかわからない。
    private static (int DirectionX, int DirectionY, bool ShouldExit, bool ShouldDamagePlayer, bool ShouldFireBullet, int WeaponNumber, bool ShouldPickUpItem, bool ShouldEquipItem, StatusEffectType? StatusEffectType, SkillTreeType? SkillTreeType) ToDirection(char input)
    {
        return char.ToUpperInvariant(input) switch
        {
            'W' => (0, -1, false, false, false, 0, false, false, null, null),
            'S' => (0, 1, false, false, false, 0, false, false, null, null),
            'A' => (-1, 0, false, false, false, 0, false, false, null, null),
            'D' => (1, 0, false, false, false, 0, false, false, null, null),
            'T' => (0, 0, false, true, false, 0, false, false, null, null),
            'G' => (0, 0, false, false, false, 0, true, false, null, null),
            'E' => (0, 0, false, false, false, 0, false, true, null, null),
            'B' => (0, 0, false, false, false, 0, false, false, StatusEffectType.Burn, null),
            'F' => (0, 0, false, false, false, 0, false, false, StatusEffectType.Freeze, null),
            'H' => (0, 0, false, false, false, 0, false, false, StatusEffectType.Shock, null),
            'C' => (0, 0, false, false, false, 0, false, false, StatusEffectType.Corrosion, null),
            'V' => (0, 0, false, false, false, 0, false, false, StatusEffectType.Virus, null),
            ' ' => (0, 0, false, false, true, 0, false, false, null, null),
            '1' => (0, 0, false, false, false, 1, false, false, null, null),
            '2' => (0, 0, false, false, false, 2, false, false, null, null),
            '3' => (0, 0, false, false, false, 3, false, false, null, null),
            '4' => (0, 0, false, false, false, 4, false, false, null, null),
            '5' => (0, 0, false, false, false, 5, false, false, null, null),
            '6' => (0, 0, false, false, false, 6, false, false, null, null),
            '7' => (0, 0, false, false, false, 0, false, false, null, SkillTreeType.Combat),
            '8' => (0, 0, false, false, false, 0, false, false, null, SkillTreeType.Tech),
            '9' => (0, 0, false, false, false, 0, false, false, null, SkillTreeType.Survival),
            'Q' => (0, 0, true, false, false, 0, false, false, null, null),
            _ => (0, 0, false, false, false, 0, false, false, null, null),
        };
    }
}
