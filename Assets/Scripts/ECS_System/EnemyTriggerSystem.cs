using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Physics;
using Unity.Physics.Systems;

/// <summary>
/// 敵の当たり判定を制御するシステム
/// </summary>
[AlwaysUpdateSystem]
public class EnemyTriggerSystem : SystemBase
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
        enemyTriggerJob.EnemyEntity = GetComponentDataFromEntity<EnemyTag>(true);
        enemyTriggerJob.PlayerBulletsEntity = GetComponentDataFromEntity<PlayerBulletTag>(true);
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
        // 敵のエンティティを取得
        [ReadOnly] public ComponentDataFromEntity<EnemyTag> EnemyEntity;
        // プレイヤーの弾のエンティティを取得
        [ReadOnly] public ComponentDataFromEntity<PlayerBulletTag> PlayerBulletsEntity;

        public EntityCommandBuffer entityCommandBuffer;


        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            // 弾が当たった時のダメージ
            const float HIT_DAMAGE = 1f;


            // 敵同士が接触した場合
            if(EnemyEntity.HasComponent(entityA) && EnemyEntity.HasComponent(entityB))
            {
                return;
            }

            // 敵とプレイヤーの弾が接触した場合
            if (EnemyEntity.HasComponent(entityA) && PlayerBulletsEntity.HasComponent(entityB))
            {
                // プレイヤーの弾を消す
                entityCommandBuffer.DestroyEntity(entityB);
                // 敵のエンティティのhpを減らす
                entityCommandBuffer.SetComponent(entityA, new EnemyTag
                {
                    _enemyHp = EnemyEntity[entityA]._enemyHp - HIT_DAMAGE
                });
            }
            else if (PlayerBulletsEntity.HasComponent(entityA) && EnemyEntity.HasComponent(entityB))
            {
                // プレイヤーの弾を消す
                entityCommandBuffer.DestroyEntity(entityA);
                // 敵のエンティティのhpを減らす
                entityCommandBuffer.SetComponent(entityB, new EnemyTag
                {
                    _enemyHp = EnemyEntity[entityB]._enemyHp - HIT_DAMAGE
                });
            }
        }
    }
}
