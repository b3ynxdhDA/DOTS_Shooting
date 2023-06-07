/// <summary>
/// ボスキャラクターを管理する
/// </summary>
public class PlayerManager
{
    #region 変数宣言
    // 参照用プレイヤーのHP
    private float _playerHP;

    // プレイヤーの駒が初期化されているか
    private bool _isPlayerInitialize = false;

    #endregion

    #region プロパティ
    /// <summary>
    /// 初期化時のプレイヤーの駒データ
    /// </summary>
    public KomaData PlayerKomaData { get; private set; }

    public bool IsPlayerInitialize { get { return _isPlayerInitialize; } set { _isPlayerInitialize = value; } }

    public float GetPlayerHP { get { return _playerHP; } set { _playerHP = value; } }

    #endregion

    #region 公開メソッド

    /// <summary>
    /// ゲーム開始時にGameManagerから呼ばれるPlayerManagerの参照メソッド
    /// </summary>
    /// <param name="komaData1">第1段階のボスの駒データ</param>
    public PlayerManager(KomaData komaData)
    {
        PlayerKomaData = komaData;
    }
    
    public void PlayerInitialize()
    {
        _isPlayerInitialize = false;
    }

    #endregion
}