using Unity.Entities;

/// <summary>
/// ボスキャラクターを管理する
/// </summary>
public class PlayerManager
{
    #region 変数宣言
    /// <summary>
    /// 初期化時のプレイヤーの駒データ
    /// </summary>
    public KomaData PlayerKomaData { get; private set; }

    #endregion

    #region 公開メソッド
    /// <summary>
    /// ゲーム開始時にGameManagerから呼ばれるPlayerManagerの初期化メソッド
    /// </summary>
    /// <param name="komaData1">第1段階のボスの駒データ</param>
    public PlayerManager(KomaData komaData)
    {
        PlayerKomaData = komaData;
    }

    #endregion
}