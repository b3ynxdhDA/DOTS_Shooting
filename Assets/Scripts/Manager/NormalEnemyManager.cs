/// <summary>
/// 駒を管理するクラス
/// </summary>
public class NormalEnemyManager
{

    #region 変数宣言

    /// <summary>
    /// 初期化時のプレイヤーの駒データ
    /// </summary>
    public KomaData NormalEnemyKomaData { get; private set; }

    #endregion
    /// <summary>
    /// ゲーム開始時にGameManagerから呼ばれるKomaManagerの参照メソッド
    /// </summary>
    public NormalEnemyManager(KomaData komaData)
    {
        NormalEnemyKomaData = komaData;
    }

    public void NormalEnemyInitialize()
    {

    }

}