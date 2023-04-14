using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム全体の状態を管理するクラス
/// </summary>
public class GameManager : MonoBehaviour
{
    #region 公開変数
    [SerializeField, Header("設定のUICanvas")]
    private GameObject _configCanvas = default;

    /// <summary>
    /// ゲームマネージャー自身を参照する変数
    /// </summary>
    public static GameManager instance { get; private set; }

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
    /// シーン遷移を管理するクラスの参照
    /// </summary>
    public SceneController SceneController { get; private set; }


    /// <summary>
    /// ゲームステートの参照
    /// </summary>
    public GameState gameState { get; set; } = GameState.Title;

    /// <summary>
    /// ゲームの状態
    /// Title:タイトル
    /// GameRedy:ゲーム開始前
    /// GameNow:ゲーム中
    /// GameOver:死亡後
    /// Result:リザルト
    /// Pause:ポーズ
    /// Config:設定
    /// </summary>
    public enum GameState
    {
        Title,
        GameRedy,
        GameNow,
        GameOver,
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

        // SEマネージャーを外部から参照しやすく
        SEManager = transform.GetComponent<SEManager>();

        BossManager = new BossManager(_bossKomaDate1, _bossKomaDate2, _bossKomaDate3);

        PlayerManager = new PlayerManager(_playerKomadate);
        //@
        InitializeGame();

        
    }

    private void Update()
    {
        // seManagerでUpdateしないためここで呼び出す
        //@SEManager.CheckVolume();
    }

    /// <summary>
    /// ゲームのプレイ状態を初期化する
    /// </summary>
    public void InitializeGame()
    {
        // ボスキャラクターの初期化
        BossManager.BossInitialize();

        // ゲームの状態をゲーム開始前にする
        gameState = GameState.GameRedy;

        // スコアを初期化
        _nowScore = 0;
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