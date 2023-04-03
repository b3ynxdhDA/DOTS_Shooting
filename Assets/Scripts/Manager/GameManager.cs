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

    // ゲームマネージャーインスタンス
    public static GameManager instance { get; private set; }

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

    #region ゲームステート切り替え時の処理メソッド
    /// <summary>
    /// ゲームステートがTitleになったとき
    /// </summary>
    private void ChangeStateToTitle()
    {
        // マウスカーソルを表示する
        Cursor.visible = true;
    }
    /// <summary>
    /// ゲームステートがGameRedyになったとき
    /// </summary>
    private void ChangeStateToGameRedy()
    {
        // スコアを初期化
        _nowScore = 0;

        // カーソルをゲーム画面内でのみ動かせるようにする
        Cursor.lockState = CursorLockMode.Confined;

        // マウスカーソルを非表示にする
        Cursor.visible = false;
    }
    /// <summary>
    /// ゲームステートがGameNowになったとき
    /// </summary>
    private void ChangeStateToGameNow()
    {
        // マウスカーソルを非表示にする
        Cursor.visible = false;
    }
    /// <summary>
    /// ゲームステートがGameOverになったとき
    /// </summary>
    private void ChangeStateToGameOver()
    {

    }
    /// <summary>
    /// ゲームステートがPauseになったとき
    /// </summary>
    private void ChangeStateToPause()
    {
        // マウスカーソルを表示する
        Cursor.visible = true;
    }
    /// <summary>
    /// ゲームステートがConfigになったとき
    /// </summary>
    private void ChangeStateToConfig()
    {

    }
    /// <summary>
    /// ゲームステートがResultになったとき
    /// </summary>
    private void ChangeStateToResult()
    {
        // マウスカーソルを表示する
        Cursor.visible = true;
        // カーソルロックを解除する
        Cursor.lockState = CursorLockMode.None;
    }

    #endregion
}
