using Unity.Entities;
/// <summary>
/// 駒を管理するクラス
/// </summary>
public class NormalEnemyManager
{
    #region 変数宣言
    // フィールドにいるボス以外の敵の数
    private int _enemyCount;

    /// <summary>
    /// 初期化時のプレイヤーの駒データ
    /// </summary>
    public Entity EnemyBasePrefab { get; private set; }

    /// <summary>
    /// 初期化時のプレイヤーの駒データ
    /// </summary>
    public KomaData[] NormalEnemyKomaData { get; private set; }

    #endregion
    /// <summary>
    /// ゲーム開始時にGameManagerから呼ばれるKomaManagerの参照メソッド
    /// </summary>
    public NormalEnemyManager(KomaData[] komaData)
    {
        NormalEnemyKomaData = komaData;
    }

    /// <summary>
    /// 雑魚敵の初期化
    /// </summary>
    public void NormalEnemyInitialize()
    {

    }
}