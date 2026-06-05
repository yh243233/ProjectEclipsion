# Project Eclipsion

## 概要

Project Eclipsion は、C#製のコンソールベース 2D リアルタイムシューティングゲームです。

現在は Console MVP を開発中であり、
将来的に以下の成長ルートを想定しています。

```txt
Console
↓
タイル描画
↓
ウィンドウ描画
↓
MonoGame
↓
独自エンジン
```

本プロジェクトは以下を重視しています。

- ゲームロジックと描画の分離
- テスト可能な設計
- AI（Codex）との共同開発
- 将来的な大規模化
- ナレッジ蓄積
- 長期運用

---

# ゲーム概要

## ジャンル

- 2D リアルタイムシューティング
- ローグライト
- ハクスラ
- ビルド構築型
- ソロプレイ中心

---

# 現在の開発フェーズ

## Phase 1 : Console MVP

現在実装対象:

- プレイヤー移動
- 敵
- 射撃
- 弾
- HP / Shield
- 当たり判定
- スコア
- Console描画

---

# 使用技術

| 技術    | 用途           |
| ------- | -------------- |
| C#      | メイン言語     |
| .NET    | 実行基盤       |
| Console | 初期描画       |
| xUnit   | テスト         |
| Codex   | AI共同開発     |
| Git     | バージョン管理 |

---

# フォルダ構成

```txt
ProjectEclipsion/
├─ docs/
├─ ai/
├─ src/
└─ tests/
```

---

## docs/

人間向けドキュメント。

| ファイル              | 内容             |
| --------------------- | ---------------- |
| 00\_概要.md           | プロジェクト概要 |
| 01\_仕様書.md         | ゲーム仕様       |
| 02\_アーキテクチャ.md | 設計思想         |
| 03\_タスク.md         | 実装タスク       |
| 04\_ロードマップ.md   | 長期計画         |
| 05\_用語集.md         | 用語説明         |

---

## ai/

AI共同開発用フォルダ。

| フォルダ  | 内容            |
| --------- | --------------- |
| prompts   | Codex用テンプレ |
| logs      | 作業履歴        |
| knowledge | ナレッジ蓄積    |

---

## src/

ゲーム本体コード。

```txt
src/
├─ ProjectEclipsion.App/
└─ ProjectEclipsion.Core/
```

### ProjectEclipsion.App

担当:

- Console描画
- 入力
- UI
- 起動処理

### ProjectEclipsion.Core

担当:

- ゲームロジック
- プレイヤー
- 敵
- 武器
- 弾
- スキル
- アイテム
- マップ
- セーブ

---

## tests/

テストコード。

```txt
tests/
└─ ProjectEclipsion.Core.Tests/
```

Coreロジックをテストする。

---

# 最重要設計

## ゲームロジックと描画を分離する

```txt
Game Logic
↓
Renderer
```

この設計を維持することで:

- Console
- MonoGame
- Unity
- Web

などへ移植しやすくなる。

---

# 開発ルール

## Core層では禁止

- Console使用
- MonoGame依存
- Thread.Sleep使用
- UI直接操作
- 巨大GodClass

---

## 推奨

- 小さな機能単位で実装
- テストを書く
- docs更新
- 作業ログ更新
- namespaceをフォルダ構成に合わせる

---

# Codex運用

## 作業前

以下を読む。

1. AGENTS.md
2. docs/01\_仕様書.md
3. docs/02\_アーキテクチャ.md
4. docs/03\_タスク.md

---

## 作業後

以下を更新する。

- docs/03\_タスク.md
- ai/logs/作業ログ.md
- ai/knowledge/設計判断.md

---

# 起動方法

## 実行

```bash
dotnet run --project src/ProjectEclipsion.App
```

---

## テスト

```bash
dotnet test
```

---

# 今後の予定

## Phase 2

- 武器追加
- アイテム
- ステージ生成
- セーブ
- UI強化

## Phase 3

- ボス
- AI高度化
- エフェクト
- ビルドシステム
- バランス調整

---

# 将来的な拡張

予定:

- マルチプレイ
- PvP
- Mod対応
- Dedicated Server
- Replay
- AI Director
- Lua Script
- ECS最適化

---

# 開発方針

まずは:

```txt
小さく作る
↓
動かす
↓
分離する
↓
テストする
↓
拡張する
```

を徹底する。

最初から完璧な大規模構成を作るのではなく、
MVPを積み上げて成長させる。
