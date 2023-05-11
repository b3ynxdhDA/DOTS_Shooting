using UnityEngine;
using Unity.Entities;

/// <summary>
/// ゲーム全体の状態を管理するクラス
/// </summary>
public class GameManager : MonoBehaviour
{
    #region 公開変数
    [SerializeField, Header("設定のUICanvas")]
    private GameObject _configCanvas = default;

    [SerializeField, Header("駒のメッシュ")] 
    public Mesh Quad;

    /// <summary>
    /// ゲームマネージャー自身を参照する変数
    /// </summary>
    public static GameManager instance { get; private set; }

    /// <summary>
    /// 駒に駒データを設定するクラスの参照
    /// </summary>
    public KomaManager KomaManager { get; private set; }

    /// <summary>
    /// 雑魚敵を管理するクラスの参照
    /// </summary>
    public NormalEnemyManager NormalEnemyManager { get; private set; }

    /// <summary>
    /// プレイヤーを管理するクラスの参照
    /// </summary>
    public PlayerManager PlayerManager { get; private set; }

    /// <summary>
    /// ボスキャラクターを管理するクラスの参照
    /// </summary>
    public BossManager BossManager { get; private set; }

    /// <summary>
    /// SEManagerクラスの参照
    /// </summary>
    public SEManager SEManager { get; private set; }

    /// <summary>
    /// UIManagerクラスの参照
    /// </summary>
    public UIManager UIManager { get; private set; }

    /// <summary>
    /// ゲームステートの参照
    /// </summary>
    public GameState gameState { get; set; } = GameState.Title;


    /// <summary>
    /// ゲームの状態
    /// Title:タイトル
    /// GameRedy:ゲーム開始前
    /// GameNow:ゲーム中
    /// GameFinish:ゲーム終了
    /// Result:リザルト
    /// Pause:ポーズ
    /// Config:設定
    /// GameFinish:ゲーム終了からリザルト表示までの間
    /// </summary>
    public enum GameState
    {
        Title,
        GameRedy,
        GameNow,
        GameFinish,
        Result,
        Pause,
        Config
    };

    [HideInInspector]// スコアの変数
    public int _nowScore = 0;

    #endregion

    #region プライベート変数
    [SerializeField, Header("プレイヤーの駒データ")]
    private KomaData _playerKomadate;

    [SerializeField, Header("雑魚敵の駒データ")]
    private KomaData _normalEnemyKomadate;

    [SerializeField, Header("ボスの第1段階の駒データ")]
    private KomaData _bossKomaDate1;

    [SerializeField, Header("ボスの第2段階の駒データ")]
    private KomaData _bossKomaDate2;

    [SerializeField, Header("ボスの第3段階の駒データ")]
    private KomaData _bossKomaDate3;

    #endregion
    private void Awake()
    {
        // GameManagerをシングルトンにする
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        // ConfigCanvasを生成
        _configCanvas = Instantiate(_configCanvas);

        // SEマネージャーを外部から参照しやすく
        SEManager = transform.GetComponent<SEManager>();

        // KomaManagerを参照できるようにする
        KomaManager = new KomaManager();

        // NormalEnemyManagerに雑魚敵の駒データを渡す
        NormalEnemyManager = new NormalEnemyManager(_normalEnemyKomadate);

        // BossManagerにボスの駒データを渡す
        BossManager = new BossManager(_bossKomaDate1, _bossKomaDate2, _bossKomaDate3);

        // PlayerManagerにプレイヤーの駒データを渡す
        PlayerManager = new PlayerManager(_playerKomadate);

    }

    private void Update()
    {
        // seManagerでUpdateしないためここで呼び出す
        SEManager.CheckVolume();
        print(gameState);
    }

    /// <summary>
    /// ゲームのプレイ状態を初期化する
    /// </summary>
    public void InitializeGame()
    {
        // メインゲームシーンのUIManagerを取得
        // DOTSによってGameObjectが少ないのでFindを使用
        UIManager = GameObject.Find("GameCanvas").GetComponent<UIManager>();

        // 雑魚敵の初期化
        NormalEnemyManager.NormalEnemyInitialize();

        // ボスキャラクターの初期化
        BossManager.BossInitialize();

        // プレイヤーの初期化
        PlayerManager.PlayerInitialize();

        // ゲームの状態をゲーム開始前にする
        gameState = GameState.GameRedy;

        // スコアを初期化
        _nowScore = 0;
    }

    /// <summary>
    /// エンティティを全て削除する
    /// </summary>
    public void EntityIntialize()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        entityManager.DestroyEntity(entityManager.GetAllEntities());
            
    }

    /// <summary>
    /// コンフィグキャンバスを表示
    /// </summary>
    public void CallConfigUI()
    {
        _configCanvas.SetActive(true);
        gameState = GameState.Config;
    }

    /// <summary>
    /// ゲームの終了
    /// </summary>
    public void OnExit()
    {
#if UNITY_EDITOR
        //エディターの時は再生をやめる
        UnityEditor.EditorApplication.isPlaying = false;
#else
            //アプリケーションを終了する
            Application.Quit();
#endif
    }
}