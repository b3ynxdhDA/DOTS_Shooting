using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム全体の状態を管理するクラス
/// </summary>
public class GameManager : MonoBehaviour
{
    // 変数宣言-----------------------------------------
    [SerializeField,Header("設定のUICanvas")]
    private GameObject _configCanvas = default;

    [HideInInspector]// SEマネージャー
    public SEManager _seManager = default;

    /// <summary>
    /// ゲームマネージャー自身を参照する変数
    /// </summary>
    public static GameManager instance { get; private set; }

    /// <summary>
    /// ボスキャラクターを管理するクラスの参照
    /// </summary>
    public BossManager BossManager { get; private set; }

    /// <summary>
    /// シーン遷移を管理するクラスの参照
    /// </summary>
    public SceneController SceneController { get; private set; }


    // 他のクラスから参照されるゲームステート
    public GameState game_State { get; set; } = GameState.Title;
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

    private void Awake()
    {
        // SEマネージャーを外部から参照しやすく
        _seManager = transform.GetComponent<SEManager>();

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

    }

    private void Update()
    {
        // seManagerでUpdateしないためここで呼び出す
        _seManager.CheckVolume();
    }

    /// <summary>
    /// ゲームのプレイ状態を初期化する
    /// </summary>
    public void InitializeGame()
    {
        // ボスキャラクターの初期化
        BossManager.BossInitialize();

        // ゲームの状態をゲーム開始前にする
        game_State = GameState.GameRedy;

        // スコアを初期化
        _nowScore = 0;
    }

    /// <summary>
    /// コンフィグキャンバスを表示
    /// </summary>
    public void CallConfigUI()
    {
        _configCanvas.SetActive(true);
        game_State = GameState.Config;
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
