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

    /// <summary>
    /// 雑魚敵を生成する
    /// </summary>
    private void NormalEnemyInstantiate()
    {
        KomaManager komaManager = GameManager.instance.KomaManager;

        // エンティティの生成
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        // 弾のエンティティのアーキタイプ
        EntityArchetype bulletArchetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(LocalToWorld),
            typeof(BulletTag),
            typeof(BulletMoveDirectionTag),
            typeof(EnemyBulletTag)
            );

        // 弾のエンティティを生成
        Entity bulletEntity = entityManager.CreateEntity(bulletArchetype);

        // 雑魚敵のエンティティのアーキタイプ
        EntityArchetype enemyArchetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(LocalToWorld),
            typeof(HPTag),
            typeof(EnemyTag),
            typeof(GunPortTag)
            );

        // 雑魚敵のエンティティを生成
        Entity enemyEntity = entityManager.CreateEntity(enemyArchetype);



        // Translationコンポーネントのデータの指定
        entityManager.AddComponentData(enemyEntity, new Translation
        {
            Value = new float3(0f, 0f, 0f)
        });
    }

}