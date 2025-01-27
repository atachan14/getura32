
### 未修正バグ
	- redEffect , pinkEffectのSOがときどきヘン。

### 優先順
	- matchingAnime & Move
		- png
		- animator
		- move

### がっこーでやること。
	- InputManager
		- OwnerPlayer,Tentacleと合わせてリファクタリング
	- TargetInfo
		- targetNameの色(free, send, receive, other, pair)
		- tributeButton
			- [-]
			- [1000]
			- [5000]
			- [10000]
			- TMP
		- PleaseTalkAura
		- CantTalkAura
	- PlayerStatus
		- MatchingColor
	- RobbyMangager
		- timeSetting
		- stageSelect
	- GameManager
		- SetupLovePoint
			- CalcLovePoint
	- turaa
		- eyeMove
		- 触手分割SortingOrder
	- アバター追加
	- 背景
	- アニメ？
		- LoveCallTentacle
		- MatchingEffect
		- ReceiveNG
		- Title
		- Opening

### おうちでやること。
# 優先順
	-

# リスト
	- steamworks

### 構造案
	- 同期構造
	[Client]
		↓
	[ServerRpc]
		↓
	（[(Hostの)GameManager]）①
		↓
	[ClientRpc]
		↓
	[(対象Clientの)ClientManager]②
		↓
	[(対象Clientの)対象GameObject等]

		- ① 一元管理したい情報をServerだけで保持する。
		public class GameManager : NetworkBehaviour
		{
			public static GameManager SI; //ServerInstance
	
		    private void Awake()
			{
				if (IsServer && SI == null) SI = this;
			}
		～～～～

		[ServerRpc(RequireOwnership = false)]
		public void xxxServerRpc(xxx)
		{
	    GameManager.SI.～～


		- ② ClientRpcを一元で受け、ローカルの各GameObjectへ処理を送信。


	

### ゲーム概要
	プレイ人数：10人前後の可変（オンライン）。
	ゲームの流れ：
	●ゲーム開始まで
	①（Title）
	②（Menu）画面左側でTuraa（DefaultPlayerPrefab（アバター））のカスタマイズやハンドルネームの設定を行い、画面右側のボタンでHostやClientとして接続。
	③（Robby）参加者たちのTuraaが表示され、MatchingTurnに近い動作や操作が可能。Hostの[Start]ボタンによってゲーム開始。

	●ゲーム開始後
	①（Opening）適当な演出。
	②（MatchingTurn）制限時間内に各プレイヤーが自由にペアを作る。このフェイズ以外には殆ど操作がない。
	③（TurnEnd）制限時間の終了に伴いペアが確定する。
	④（TurnResult）ペア毎にホテルに入っていき、ステータスの増減等が行われる。
	⑤（TurnSetup）下記条件によりプレイヤーが1人以上脱落する。
	　・奇数時：ペアを作れなかったプレイヤーが脱落。
	　・偶数時：ステータス[元気]が最も低いプレイヤーが脱落。
	⑥生存プレイヤーが4人になるまで②③④⑤の繰り返し。	
	⑦（LastTurn）最後にもう一度②③を行う。
	⑧（GameResult）順位を判定し、順位に応じた演出とリザルト画面。

	●各パラメーター：
	　[好き好きポイント]：
	　　●Opening時に各プレイヤーと対象毎に、ランダムかつ公平（許容値は設定するが未定）に振り分けられる。
	（送信値の合計と受信値の合計が各プレイヤー間で同じくらい。
	例：
		／　a　 b　 c　 d
		a→ ／　80　10　60　
		b→ 40　／　90　20
		c→ 50　30　／　70
		d→ 60　40　50　／
	）
	　　●TurnEnd時、ペア対象のこの値に応じて[元気]や[魅力]が増減する。
	　　●TurnSetup時、（前回）ペア対象のこの値が半分になり、その差分を他のプレイヤーの[魅力]に応じて分配する。

	　[お金]：
	　　●MatchingTurn時、この値の譲渡をペア成立の交渉条件に組み込める。
		●TurnEnd時、この値をリソースにして整形コマンド等で[魅力]の値の購入が可能。
		●GameResult時、この値によって1位から4位が決まる。

	　[元気]：
	　　●TurnSetup時、この値に応じて[お金]を得る。
	　　●TurnSetup時 && 生存人数が偶数時、この値の最も低いプレイヤーが脱落する。
	　　●この値に応じて移動速度が増減する。

	　[魅力]：
	　　●上述の条件で上昇し、他プレイヤーからの[好き好きポイント]を集めやすくする。

	●Robby/MatchingTurnでの操作（その他のフェイズでは殆ど操作しない）：
	　[右クリック] LOL的なTuraaの座標移動（実装済）。
	　[左クリック] targetの選択やButtonのクリック。

	- Robby/MatchingTurnの主なオブジェクト
			- Turaa（DefaultPlayerPrefab（アバター））
			- MatchingSet
				- MatchingUI
					- targetInfo（左上 , 他ClientのTuraaを選択するとtargetとして反映される）
						- targetName
						- LoveCall（告白ボタン）
						- TributeBer（貢ぐ額を設定する , デフォル値は0）
						- Block/Cansell（targetからのLoveCallを受けなくする）
					- Aura（左中 , モード切替トグル）
						- CantTalkAura（対象指定不可モード）
						- PleaseTalkAura（目立つエフェクトが付くだけのモード）
					- LoveCalls（左下 , LoveCallを受けるとLoveCallListに追加され、古い順に最大4件までLovePopupとして反映される）
						- LovePopup
							- SenderName
							- Tribute
							- OK（暫定的にMatchingが成立、LoveCallListをClear）
							- Neg（tributeの交渉UIを表示）
							- NG
							- Block/Cansell
				- InputManager
				- QolEffectManager
				- MatchingManager（状態に応じたEffectを管理）
					（状態：Effect
					　LoveCall送信中：Red
					　LoveCall受信中：Pink
					　Matching成立中：Matching
					　CantTalkAura：Gray
					　PleaseTalkAura：Orange
					　Default:None）
			- BaseUI
				- Status（中下）
					- Money
					- Fine
					- Charm
				- Chat（右下）
			- RobbyCanvas
				- RobbySize（右上 , 現在の参加人数をUIに反映）
				- StartButton（その下 , Hostにだけ表示）
				- StageSelect（その下 ,Hostにだけ表示）
				- TimeSetting（中上）
			- GameManager？（MatchingTurnの処理？）
				- GameSize（右上 , 生存人数/参加人数）
				- Time（中上 , 制限時間）
				- 未定