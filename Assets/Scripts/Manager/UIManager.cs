using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ステージやそのUIを管理するクラス
/// </summary>
public class UIManager : MonoBehaviour
{
    // 変数宣言--------------------------
    private bool _isCallGameOver = false;
    // タイマー
    private float _timerCount = 0;

    // テキストオブジェクト---------------------------
    // ゲームスタートのカウント
    [SerializeField] private Text _startCountText = default;

    // ゲームオーバーテキスト
    [SerializeField] private GameObject _gameOverText = default;

    // リザルトテキスト
    [SerializeField] private GameObject _resultUI = default;

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

    private void Start()
    {
        // ゲームの状態をゲーム中に
        GameManager.instance.gameState = GameManager.GameState.GameRedy;

        // ゲームスタートのカウントダウンを開始
        StartCoroutine("CountdownCoroutine");

        // タイムスケールの初期化
        Time.timeScale = _SEFAULT_TIMESCALE;
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

            // ゲームステートがゲームオーバーで、ゲームオーバーコルーチンを呼んでいないなら
            if (GameManager.instance.gameState == GameManager.GameState.GameOver && !_isCallGameOver)
            {
                StartCoroutine("GameOver");
                _isCallGameOver = true;
            }
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
    /// ゲームオーバーしてからリザルトまでの処理
    /// </summary>
    /// <returns></returns>
    IEnumerator GameOver()
    {
        // ゲームステートをGameOverに
        GameManager.instance.gameState = GameManager.GameState.GameOver;

        _gameOverText.SetActive(true);

        // ゲームオーバーテキストのポジションが0より大きい間
        while (_GAMEOVER_TEXT_POSITION_Y < _gameOverText.transform.localPosition.y)
        {
            // ゲームオーバーテキストのポジションを下げる
            _gameOverText.transform.localPosition += Vector3.down * 10;
            yield return new WaitForSeconds(0.001f);
        }

        yield return new WaitForSeconds(3f);

        // ゲームステートをResultに
        GameManager.instance.gameState = GameManager.GameState.Result;

        _resultUI.SetActive(true);
    }
}