### がっこーでやること。
	- UI作成
		- fine
		- money
		- charm
		- menberSize
			- 中の表示(おうち)
		- startButton
		- lovePoint
		- aura
	- カメラ
		- space
		- y
		- zoomUp/down
	- 演出
		- redEffect
			- フラグ
			- キャンセル
			- 色
			- 移動不可
			- receiveNG(おうち)
	
	- 関数
		- SetupLovePoint
			- CalcLovePoint
			- それの表示(おうち)
	- アバター追加
	- 背景
	- アニメ？
		- Opening
		- gameStart

### おうちでやること。
	- 同期
	- 演出
		- pinkEffect(おうち)
			- フラグ
			- 色
			- 移動不可
		- instantMaching
			- interfase
				- MatchingMove
				- MatchingEffect

	- steamworks



### 構造案
	- ①staticで一律管理　②DefaultPrayerPrefabに持たせる
	[Client]
		↓
	[ServerRpc]
		↓
	[(Hostの)GameManager]①
		↓
	[ClientRpc]
		↓
	[(対象Clientの)ClientManager]②
		↓
	[(対象Clientの)対象GameObject等]

		- ①
		[ServerRpc(RequireOwnership = false)]
		public void SubmitDataServerRpc(string playerName, int score)
		{
	    GameManager.Instance.
	
		～～～～

		public class GameManager : NetworkBehaviour
		{
			public static GameManager Instance;
	
		    private void Awake()
			{
				if (Instance == null) Instance = this;	
			}

		- ②




プレイ人数：10人前後の可変（オンライン）。
ゲームの流れ：
●ゲーム開始まで
①（タイトル画面）
②メニュー画面。画面左側でアバターやハンドルネームを設定し、画面右側のボタンでHostやClientとして接続。
③ロビー画面。参加者たちのアバターが表示され、ゲーム中に近い動作や操作が可能。Hostの[ゲーム開始]ボタンによってゲーム開始。

●ゲーム開始後
①（GameStart）適当な演出。
②（MatchingTurn）制限時間内に各プレイヤーが自由にペアを作る。
③（TurnEnd）制限時間の終了に伴いペアが確定する。
④（TrunResult）ペア毎にホテルに入っていく演出が行われ、ステータスの増減等が行われる。
⑤（TurnSetup）下記条件によりプレイヤーが1人以上脱落する。
　・奇数時：ペアを作れなかったプレイヤーが脱落。
　・偶数時：ステータス[元気]が最も低いプレイヤーが脱落。
⑥生存プレイヤーが4人になるまで②③④⑤の繰り返し。
⑦（LastTurn）最後にもう一度②③を行う。
⑧（GameResult）順位を判定し、順位に応じた演出とリザルト画面。

●各パラメーター：
　[好き好きポイント]：
　　●GameStart時に各プレイヤーと対象毎に、ランダムかつ公平（許容値は設定するが未定）に振り分けられる。
（例：
               a→b=80, a→c=10, a→d=60,
b→a=40,                b→c=90, b→d=20,
c→a=50, c→b=30,                 c→d=70,
d→a=60, d→b=40, d→c=50）
　　●TurnEnd時、ペア対象のこの値に応じて[元気]や[魅力]が増減する。
　　●TurnSetup時、（前回）ペア対象のこの値が半分になり、その差分を他のプレイヤーの[魅力]に応じて分配する。

　[お金]：
　　●MatchingTurn時、この値の譲渡をペア成立の交渉条件に組み込める。
　　●TurnEnd時、この値をリソースにして整形コマンド等で[魅力]の値の購入が可能。
　　●LastTurn時、この値によって1位から4位が決まる。
　　　　
　[元気]：
　　●TurnSetup時、この値に応じて[お金]を得る。
　　●TurnSetup時 && 生存人数が偶数時、この値の最も低いプレイヤーが脱落する。
　　●この値に応じて移動速度が増減する。

　[魅力]：
　　●上述の条件で上昇し、他プレイヤーからの[好き好きポイント]を集めやすくする。

●MatchingTurnでの操作（その他のフェイズでは基本的に操作しない）：
　[右クリック] LOL的なアバターの座標移動（実装済）。
　[左クリック] 諸々の選択。
　　→他のプレイヤーをクリックすると画面左上のtargetUIに反映される。
　　　targetUI：
　　　　対象の名前と各ボタンが表示される。予定コマンド：[告白][譲渡金][ブロック/解除]。
　　→他のプレイヤーに[告白]される度にLoveCallsUIが新しく生成され、表示される。
　　　LoveCallsUI：
　　　　対象の名前と各ボタンが表示される。予定コマンド：[OK][要求][NG][ブロック/解除]。