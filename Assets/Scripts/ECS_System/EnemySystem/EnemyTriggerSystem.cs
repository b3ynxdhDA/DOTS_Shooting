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
        enemyTriggerJob.HPComponent = GetComponentDataFromEntity<HPTag>(true);
        enemyTriggerJob.EnemyEntity = GetComponentDataFromEntity<EnemyTag>(true);
        enemyTriggerJob.PlayerBulletsEntity = GetComponentDataFromEntity<PlayerBulletTag>(true);
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
        // エンティティのHPコンポーネントを取得
        [ReadOnly] public ComponentDataFromEntity<HPTag> HPComponent;
        // 敵のエンティティを取得
        [ReadOnly] public ComponentDataFromEntity<EnemyTag> EnemyEntity;
        // プレイヤーの弾のエンティティを取得
        [ReadOnly] public ComponentDataFromEntity<PlayerBulletTag> PlayerBulletsEntity;
        // プレイヤーのエンティティを取得
        [ReadOnly] public ComponentDataFromEntity<PlayerTag> PlayerEntity;

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
                entityCommandBuffer.SetComponent(entityA, new HPTag
                {
                    _hp = HPComponent[entityA]._hp - HIT_DAMAGE
                });
            }
            else if (PlayerBulletsEntity.HasComponent(entityA) && EnemyEntity.HasComponent(entityB))
            {
                // プレイヤーの弾を消す
                entityCommandBuffer.DestroyEntity(entityA);
                // 敵のエンティティのhpを減らす
                entityCommandBuffer.SetComponent(entityB, new HPTag
                {
                    _hp = HPComponent[entityB]._hp - HIT_DAMAGE
                });
            }
            // 敵とプレイヤーが接触した場合の処理はPlayerTriggerSystemにある
            else if (EnemyEntity.HasComponent(entityA) && PlayerEntity.HasComponent(entityB))
            {
            }
            else if (PlayerEntity.HasComponent(entityA) && EnemyEntity.HasComponent(entityB))
            {
            }
        }
    }
}
