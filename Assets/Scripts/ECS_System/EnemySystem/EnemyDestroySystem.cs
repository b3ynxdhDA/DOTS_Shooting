using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

/// <summary>
/// hpが0になった敵を削除するシステム
/// </summary>
public class EnemyDestroySystem : SystemBase
{
    // 変数宣言------------------------------------------------------------------
    // 実行タイミングを管理しているシステムグループ
    private EndSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

    // 定数宣言--------------------------------------------------------


    /// <summary>
    /// システム作成時に呼ばれる処理
    /// </summary>
    protected override void OnCreate()
    {
        // EntityCommandBufferの取得
        _entityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    /// <summary>
    /// システム有効時にフレーム毎に呼ばれる処理
    /// </summary>
    protected override void OnUpdate()
    {
        // コマンドバッファを取得
        EntityCommandBuffer commandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

        Entities
            .WithName("Enemy_Destroy")
            .WithAll<EnemyTag>()
            .WithNone<BossEnemyTag>()
            .WithBurst()
            .ForEach((Entity entity, in EnemyTag enemyTag) =>
            {
                // 敵のHPが0以下になったら消す
                if (enemyTag._enemyHp <= 0)
                {
                    commandBuffer.DestroyEntity(entity);
                }

            }).WithoutBurst().Run();// メインスレッド処理

        // 指定したJob完了後にECBに登録した命令を実行
        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}
