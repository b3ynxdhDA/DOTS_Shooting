using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ステージのUIを管理するクラス
/// </summary>
public class UIManager : MonoBehaviour
{
    // 変数宣言--------------------------
    // タイマー
    private float _timerCount = 0;

    // テキストオブジェクト---------------------------
    // リザルトテキスト
    [SerializeField] private GameObject _resultUI = default;
    
    // ボスの体力バー
    [SerializeField] private Slider _bossHpSlider = default;

    // ゲームオーバーテキスト
    [SerializeField] private GameObject _gameFinishText = default;

    // ゲームスタートのカウント
    [SerializeField] private Text _startCountText = default;

    // ハイスコアテキスト
    [SerializeField] private Text _scoreCountText = default;

    // タイマーテキスト
    [SerializeField] private Text _timerCountText = default;

    // 定数宣言---------------------
    // 1分間の秒数
    const int _ONE_MINUTES = 60;

    // デフォルトのタイムスケール
    const int _SEFAULT_TIMESCALE = 1;

    // ゲームオーバーテキストの移動後のY座標
    const float _GAMEOVER_TEXT_POSITION_Y = 0;

    // ゲームクリア時に表示するテキスト
    const string _CLEAR_TEXT = "!! GAME CLEAR !!";

    // ゲームオーバー時に表示するテキスト
    const string _DEFEAT_TEXT = "GAME OVER";

    private void Start()
    {
        // ゲームスタートのカウントダウンを開始
        StartCoroutine("CountdownCoroutine");

        // タイムスケールの初期化
        Time.timeScale = _SEFAULT_TIMESCALE;

        GameManager.instance.InitializeGame();
    }
    private void Update()
    {
        // ハイスコアの表示を更新
        _scoreCountText.text = "" + GameManager.instance._nowScore;

        // ゲームステートがゲーム中の時
        if (GameManager.instance.gameState == GameManager.GameState.GameNow)
        {
            // タイマーの更新(増加)
            _timerCount += Time.deltaTime;
            _timerCountText.text = "" + ((int)_timerCount / _ONE_MINUTES).ToString("00") + " : " + ((int)_timerCount % _ONE_MINUTES).ToString("00");

            // タイマーの更新(減少)
            //_timerCount -= Time.deltaTime;
            //_timerCountText.text = "" + ((int)_timerCount / _ONE_MINUTES).ToString("00") + " : " + ((int)_timerCount % _ONE_MINUTES).ToString("00");

        }
    }

    /// <summary>
    /// ゲーム終了時のコルーチンを外部から呼ぶメソッド
    /// </summary>
    /// <param name="clear">true:ゲームクリア,false:ゲームオーバー</param>
    public void CallGameFinish(bool clear)
    {
        // ゲーム中からしか遷移しない
        if (GameManager.instance.gameState == GameManager.GameState.GameNow)
        {
            StartCoroutine("GameFinish", clear);
        }
    }

    /// <summary>
    /// ゲーム開始の３カウントダウンのコルーチン
    /// </summary>
    IEnumerator CountdownCoroutine()
    {
        _startCountText.gameObject.SetActive(true);

        _startCountText.text = "3";
        GameManager.instance.SEManager.OnStartCount3_SE();
        yield return new WaitForSeconds(1f);

        _startCountText.text = "2";
        GameManager.instance.SEManager.OnStartCount3_SE();
        yield return new WaitForSeconds(1f);

        _startCountText.text = "1";
        GameManager.instance.SEManager.OnStartCount3_SE();
        yield return new WaitForSeconds(1f);

        _startCountText.text = "GO!";
        GameManager.instance.SEManager.OnStartCountGo_SE();
        yield return new WaitForSeconds(1f);

        _startCountText.text = "";
        _startCountText.gameObject.SetActive(false);
        GameManager.instance.gameState = GameManager.GameState.GameNow;
    }

    /// <summary>
    /// ゲーム終了時のテキストを呼ぶコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator GameFinish(bool isClear)
    {
        // ゲームステートをGameOverに
        GameManager.instance.gameState = GameManager.GameState.GameFinish;

        _gameFinishText.SetActive(true);

        // クリアによる遷移か敗北による遷移か
        if (isClear)
        {
            _gameFinishText.GetComponent<Text>().text = _CLEAR_TEXT;
        }
        else
        {
            _gameFinishText.GetComponent<Text>().text = _DEFEAT_TEXT;
        }

        // ゲームオーバーテキストのポジションが0より大きい間
        while (_GAMEOVER_TEXT_POSITION_Y < _gameFinishText.transform.localPosition.y)
        {
            // ゲームオーバーテキストのポジションを下げる
            _gameFinishText.transform.localPosition += Vector3.down * 10;
            yield return new WaitForSeconds(0.001f);
        }

        yield return new WaitForSeconds(3f);

        // ゲームステートをResultに
        GameManager.instance.gameState = GameManager.GameState.Result;


        _gameFinishText.SetActive(false);
        _resultUI.SetActive(true);
        
    }
}