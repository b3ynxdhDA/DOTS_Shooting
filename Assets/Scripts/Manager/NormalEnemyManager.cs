using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Rendering;

/// <summary>
/// 駒を管理するクラス
/// </summary>
public class NormalEnemyManager : MonoBehaviour
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

    /// <summary>
    /// 雑魚敵の初期化
    /// </summary>
    public void NormalEnemyInitialize()
    {

    }
}