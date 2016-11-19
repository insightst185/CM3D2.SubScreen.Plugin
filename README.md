﻿# CM3D2.SubScreen.Plugin
画面上にサブカメラおよびスクリーンを配置し、サブカメラの映像をスクリーンに投影するUnityInjectorプラグインです。

## 導入方法
#### 前提条件  
UnityInjectorが導入済みであること。

#### インストール  
Download ZIPボタンを押してzipファイルをダウンロードします。  
zipファイルの中にあるUnityInjectorフォルダを、CM3D2フォルダにコピーしてください。

配布するDLLはChu-B Lip版に対応していません。  
配布するソースはChu-B Lip版に対応してます。  
お手数ですがChu-B Lip版使用の場合は、各自コンパイルしてください  

<span style="color:red;font-weight:bold;">注意</span>  
0.3.9.15で機能追加した関係で、SubScreenParam.xmlに要素・属性が追加されています。  
以前のバージョンを利用されている方は、SubScreenParam.xmlを新しいものに入れ替えて利用ください  
SubScreenPreset.xmlはそのまま利用できますにゃあ  

0.3.9.0で機能追加した関係で、SubScreenParam.xml, SubScreenPreset.xmlとも要素・属性が追加されています。  
以前のバージョンを利用されている方は、各xmlを削除してからご利用ください。  

#### 自分でコンパイルする場合  
自分でコンパイルする場合、UnityInjectorフォルダにソース（**CM3D2.SubScreenPlugin.cs**）をコピーした後、コマンドプロンプト等で  
`cd [UnityInjectorフォルダ]`
`C:\Windows\Microsoft.NET\Framework\v3.5\csc /t:library /lib:..\CM3D2x64_Data\Managed /r:UnityEngine.dll /r:UnityInjector.dll /r:Assembly-CSharp.dll /r:Assembly-CSharp-firstpass.dll /r:ExIni.dll CM3D2.SubScreenPlugin.cs`  
を実行してください。

## 機能概要
#### 対象シーン
* エディット
* 夜伽
* ダンス
* ADVパート

で動作します。

#### 使用方法
###### <span style="color:red">new!</span>プリセット自動適用
UnityInjector/Config/SubScreenParam.xmlの**autoPreset="false"**の**false**を**true**に変更すると、メニューに「現在のシーンにプリセットを割り当て」ボタンが追加されます。プリセット登録後、こちらでプリセットを選択・割り当てることで、次回以降、該当シーン・夜伽プレイ開始時にプリセットが適用されます。

###### メニュー操作
Pauseキーでメニューが開きます。  
キーアサインを変更したい場合は、UnityInjector/Config/SubScreenParam.xmlの**toggleKey="Pause"**の**Pause**を好きなキーに変更してください。  
指定可能なキーは、[Unityスクリプトリファレンス](http://docs.unity3d.com/jp/current/ScriptReference/KeyCode.html)
を参照してください。

![GUI](http://i.imgur.com/ZLytYBB.png  "GUI")

* 有効  
    サブスクリーンを有効にします。

* サブカメラを常にメイドに向ける  
    サブカメラを移動しても、常にメイドの方を向きます。

* サブカメラをメイドに向ける  
    サブカメラをメイドに向けます。

* サブカメラを常にメイドの顔の前に  
    サブカメラを自動的にメイドの顔の前に配置します。
    - 上下  
        上下位置を調整します。
    - 左右  
        左右位置を調整します。
    - 前後  
        前後位置を調整します。

* サブカメラをメインカメラに移動  
    サブカメラの位置をメインカメラに合わせます。

* スクリーンを中心後方に移動  
    スクリーンの位置をメインカメラ中心点のZ軸後方に移動します。

* サブカメラに切り替え  
    スクリーンに投影せず、サブカメラとして表示します。
    - X  
        サブカメラのX軸方向位置を調整します。
    - Y  
        サブカメラのY軸方向位置を調整します。
    - サイズ  
        サブカメラのサイズを調整します。1で全画面表示となります。

* メインライト設定  
メインライトの明るさ・色を調整します。
    - 明るさ、赤、緑、青  
        各値を調整します。

* サブライトON  
    サブライト（サブカメラ前方へのスポットライト）を点灯します。
    - 範囲、明るさ、赤、緑、青  
        各値を調整します。

* カメラの色  
    サブカメラモデルの色を調整します。  
    - 明るさ、赤、緑、青、アルファ  
    各値を調整します。
	- <span style="color:red">new!</span> 視野角  
		視野角を変更します。

* スクリーンの倍率    
    スクリーンの大きさを調整します。

* スクリーンの向き  
    スクリーンの向きを調整します。

* スクリーンの色  
    スクリーンの明るさ・色を調整します。
    - 照明  
    スクリーンに対し照明を当てます。    
	0だとメインライトに従います。  
    0より大きくすると、メインライトの影響をはずし、正面からの照明が照射されます。  
    - 明るさ、赤、緑、青、アルファ  
    各値を調整します。

* スクリーンフィルター  
    スクリーンの前にConfig/SubSCreenFilter.pngを表示します。  
    - 明るさ、赤、緑、青、アルファ  
    各値を調整します。

* プリセットの呼び出し  
	プリセットを選択し、設定値を読み込みます。
	
* <span style="color:red">new!</span> プリセットの削除  
	プリセットを選択し、削除します。  
	**※クリックで即削除されるのでご注意。**
	
* 現在値をプリセットとして保存  
	現在の各設定値をプリセットとして保存します。  
	同じプリセット名は保存できません。  
	呼び出しやすいよう、わかりやすいプリセット名をつけるとよいでしょう。  
	**※プリセットは、UnityInjector/Config/SubScreenPreset.xmlに保存されます。**

* <span style="color:red">new!</span> 現在のシーンにプリセットを割り当て  
	現在のシーン・夜伽プレイ開始時に、選択したプリセットが適用されるようにします。  
	- 割り当てを解除  
		現在のシーンに割り当てられているプリセットを解除します。

###### カメラ操作
**W. S. A. D, Q, E** カメラの向きに対し、前、後、左、右、上、下に移動します。

**Ctrl + W, S, A, D, Q, E** 向きに関係なく、座標軸方向に移動します。

**Shift + (Ctrl +) W, S, A, D, Q, E** 移動速度をあげます。

**Alt + W, S, A, D, Q, E** 回転します。

**VR版ではW, S, A, D, Q, Eは
**        I, K, J, L, U, Oです

##更新履歴
####0.3.9.16
* unity5対応したつもり  

####0.3.9.15
* メインカメラの視野角変更スライダーを追加  
* ダンス中はZキーどON/OFF切り替え可  

####0.3.9.14
* happy!happy!スキャンダル豪華版，Can Know Two Closeに対応 (Chu-B Lip版は未対応）


####0.3.9.13
* happy!happy!スキャンダルに対応 (Chu-B Lip版は未対応）
* Vキーでサブカメラ／サブスクリーンをトグルして、それぞれ移動／回転可能に
* バグをいくつか修正

####0.3.9.12
* with Chu-B Lip版で夜伽スキル毎のプリセ切替が正しく動いていなかったのを修正 プラグイン名を修正

####0.3.9.11
* with Chu-B Lip版およびVR版に対応

####0.3.9.10
* rhythmix to youに対応
* 0.3.9.9の修正をもとにもどしてサブカメラの位置を引き継ぎしないように修正
* サブカメラの位置を引き継ぎたい場合は、SubScreenParam.xmlのsub_positionに"new"を指定してください
* サブスクリーンの機能とは全く関係ない目線移動の実験機能を追加

####0.3.9.9
* 0.3.9.8で追加した要素がプリセ入力時に反映されるように修正

####0.3.9.8
* サブカメラに切り替え時にサブカメラを移動できるスライダーを追加

####0.3.9.7
* 同名ファイルでプリセットとして保存するときの不具合修正（上書き保存に仕様変更）
* プリセットとして保存するときのテキストボックスをなんとなく変更

####0.3.9.6
* スライダーで値を変更する項目にテキストボックス追加
  (スライダーで十分調整できるケースが多いと思うけどね)

####0.3.9.5
* stellar my tearsに対応

####0.3.9.4
* scarlet leapに対応

####0.3.9.3
?????

####0.3.9.2
* メニューを一度開くと閉じない不具合修正
* 自動プリセット適用時にNullReferenceExceptionになる不具合修正

####0.3.9.1
* ダンスで機能しなかった不具合修正

####0.3.9.0
* サブカメラの視野角を変更する機能追加
* スクリーンを常にメインカメラ前に表示する機能追加
* シーン・夜伽プレイごとに自動的にプリセットを適用する機能追加
* スクリーンの色 - 照明の機能変更（照明を0より大きくすると、メインライトの影響を無効に）

####0.3.0.2
* サブライト、スクリーンフィルターを有効にした状態でサブスクリーンを無効にしても残ってしまう不具合修正

####0.3.0.1
* メニューを開くキーをパラメータで変更できるよう変更

####0.3.0.0
* プリセット保存・読み込み機能追加
* 起動ボタンをPauseに変更

####0.2.1.0
* サブライトの範囲が変わらない不具合修正
* サブスクリーン前にフィルター画像配置する機能追加

####0.2.0.0
* 初版
