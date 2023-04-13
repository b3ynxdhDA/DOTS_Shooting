using Unity.Entities;
using UnityEngine;

/// <summary>
/// ボスキャラクターを管理する
/// </summary>
public class BossManager
{
    #region 変数宣言
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

    /// <summary>
    /// ボスキャラクターの状態が何段階目か
    /// </summary>
    public int BossPhaseCount { get; private set; }

    #endregion

    #region 公開メソッド
    /// <summary>
    /// ゲーム開始時にGameManagerから呼ばれるBossManagerの初期化メソッド
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
    public void BossInitialize()
    {
        BossPhaseCount = 1;

    }

    /// <summary>
    /// ボスの攻撃段階を上げるメソッド
    /// </summary>
    public void UpdateBossCount()
    {
        BossPhaseCount++;
        Debug.Log(BossPhaseCount);
    }

    #endregion
}