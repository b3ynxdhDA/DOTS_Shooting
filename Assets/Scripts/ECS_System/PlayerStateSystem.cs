using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

/// <summary>
/// プレイヤーの状態をみるシステム
/// </summary>
public class PlayerStateSystem : SystemBase
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
    /// システム実行停止時やOnDestroy()の前に呼ばれる処理
    /// </summary>
    protected override void OnStartRunning()
    {

    }

    /// <summary>
    /// システム有効時にフレーム毎に呼ばれる処理
    /// </summary>
    protected override void OnUpdate()
    {
        // コマンドバッファを取得
        EntityCommandBuffer commandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

        Entities
            .WithName("Player_State")
            .WithAll<PlayerTag>()
            .WithBurst()
            .ForEach((Entity entity, in PlayerTag playerTag) =>
            {
                // プレイヤーのHPが0以下なら消す
                if (playerTag._playerHp <= 0)
                {
                    commandBuffer.DestroyEntity(entity);
                }

            }).WithoutBurst().Run();// メインスレッド処理

        // 指定したJob完了後にECBに登録した命令を実行
        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}
