using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Physics;
using Unity.Physics.Systems;

/// <summary>
/// プレイヤーの当たり判定を制御するシステム
/// </summary>
[AlwaysUpdateSystem]
public class PlayerTriggerSystem : SystemBase
{
    // 変数宣言------------------------------------------------------------------
    // 実行タイミングを管理しているシステムグループ
    private EndSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

    // ワールド
    private BuildPhysicsWorld _buildPhysicsWorld;
    // 
    private StepPhysicsWorld _stepPhysicsWorld;

    /// <summary>
    /// システム作成時に呼ばれる処理
    /// </summary>
    protected override void OnCreate()
    {
        // BuildPhysicsWorldとStepPhysicsWorldを取得
        _buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        _stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();

        // EntityCommandBufferの取得
        _entityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    /// <summary>
    /// システム有効時にフレーム毎に呼ばれる処理
    /// </summary>
    protected override void OnUpdate()
    {
        // ジョブの生成
        EnemyTriggerJob enemyTriggerJob = new EnemyTriggerJob();
        enemyTriggerJob.allEnemyBulletEntity = GetComponentDataFromEntity<EnemyBulletTag>(true);
        enemyTriggerJob.PlayerEntity = GetComponentDataFromEntity<PlayerTag>(true);
        enemyTriggerJob.entityCommandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

        // ジョブの実行
        Dependency = enemyTriggerJob.Schedule(
            _stepPhysicsWorld.Simulation,
            ref _buildPhysicsWorld.PhysicsWorld,
            Dependency);

        // 完了すべきジョブをコマンドバッファシステムに追加
        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }

    [BurstCompile]
    struct EnemyTriggerJob : ITriggerEventsJob
    {
        // 敵の弾のエンティティを取得
        [ReadOnly] public ComponentDataFromEntity<EnemyBulletTag> allEnemyBulletEntity;
        // プレイヤーのエンティティを取得
        [ReadOnly] public ComponentDataFromEntity<PlayerTag> PlayerEntity;

        public EntityCommandBuffer entityCommandBuffer;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            // 敵の弾同士が接触した場合
            if(allEnemyBulletEntity.HasComponent(entityA) && allEnemyBulletEntity.HasComponent(entityB))
            {
                return;
            }

            // 敵の弾とプレイヤーが接触した場合
            if (allEnemyBulletEntity.HasComponent(entityA) && PlayerEntity.HasComponent(entityB))
            {
                entityCommandBuffer.DestroyEntity(entityA);
                //entityCommandBuffer.DestroyEntity(entityB);
            }
            else if (PlayerEntity.HasComponent(entityA) && allEnemyBulletEntity.HasComponent(entityB))
            {
                entityCommandBuffer.DestroyEntity(entityB);
                //entityCommandBuffer.DestroyEntity(entityA);
            }
        }
    }
}
