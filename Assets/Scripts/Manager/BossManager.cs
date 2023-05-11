/// <summary>
/// ボスキャラクターを管理する
/// </summary>
public class BossManager
{
    #region 変数宣言
    // 参照用のボスのHP
    private float _bossHP;

    // 第1段階の駒が設定されているか
    private bool _isBossInitialize = false;

    // ボスキャラクターの状態が何段階目か
    private int _bossPhaseCount;
    #endregion

    #region プロパティ
    /// <summary>
    /// 第1段階のボスの駒データ
    /// </summary>
    public KomaData BossKomaData1 { get; private set; }
    /// <summary>
    /// 第2段階のボスの駒データ
    /// </summary>
    public KomaData BossKomaData2 { get; private set; }
    /// <summary>
    /// 第3段階のボスの駒データ
    /// </summary>
    public KomaData BossKomaData3 { get; private set; }

    public float GetBossHP { get { return _bossHP; } set { _bossHP = value; } }

    /// <summary>
    /// ボスキャラクターの状態が何段階目か
    /// </summary>
    public int BossPhaseCount { get { return _bossPhaseCount; }}

    /// <summary>
    /// 第1段階の駒が設定されているか
    /// </summary>
    public bool IsBossInitialize { get { return _isBossInitialize; } set { _isBossInitialize = value; } }

    #endregion

    #region 公開メソッド
    /// <summary>
    /// ゲーム開始時にGameManagerから呼ばれるBossManagerの参照メソッド
    /// </summary>
    /// <param name="komaData1">第1段階のボスの駒データ</param>
    /// <param name="komaData2">第2段階のボスの駒データ</param>
    /// <param name="komaData3">第3段階のボスの駒データ</param>
    public BossManager(KomaData komaData1, KomaData komaData2, KomaData komaData3)
    {
        BossKomaData1 = komaData1;
        BossKomaData2 = komaData2;
        BossKomaData3 = komaData3;
    }

    /// <summary>
    /// ボスの初期化をするメソッド
    /// </summary>
    public void BossInitialize()
    {
        _bossPhaseCount = 1;
        _isBossInitialize = false;
    }

    /// <summary>
    /// ボスの攻撃段階を上げるメソッド
    /// </summary>
    public void UpdateBossCount()
    {
        _bossPhaseCount++;
    }

    #endregion
}