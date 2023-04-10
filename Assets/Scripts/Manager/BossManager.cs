
/// <summary>
/// ボスキャラクターを管理する
/// </summary>
public class BossManager 
{
    #region 変数宣言
    /// <summary>
    /// ボスキャラクターの状態が何段階目か
    /// </summary>
    public int BossPhaseCount { get; private set; }

    #endregion

    #region メソッド
    public void BossInitialize()
    {
        BossPhaseCount = 0;

    }

    #endregion
}
