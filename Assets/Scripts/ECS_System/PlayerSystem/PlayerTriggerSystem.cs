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
        PlayerTriggerJob playerTriggerJob = new PlayerTriggerJob();
        playerTriggerJob.AllEnemyBulletEntity = GetComponentDataFromEntity<EnemyBulletTag>(true);
        playerTriggerJob.AllEnemyEntity = GetComponentDataFromEntity<EnemyTag>(true);
        playerTriggerJob.PlayerEntity = GetComponentDataFromEntity<PlayerTag>(true);
        playerTriggerJob.entityCommandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

        // ジョブの実行
        Dependency = playerTriggerJob.Schedule(
            _stepPhysicsWorld.Simulation,
            ref _buildPhysicsWorld.PhysicsWorld,
            Dependency);

        // 完了すべきジョブをコマンドバッファシステムに追加
        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }

    [BurstCompile]
    struct PlayerTriggerJob : ITriggerEventsJob
    {
        // 敵の弾のエンティティを取得
        [ReadOnly] public ComponentDataFromEntity<EnemyBulletTag> AllEnemyBulletEntity;
        // 敵のエンティティを取得
        [ReadOnly] public ComponentDataFromEntity<EnemyTag> AllEnemyEntity;
        // プレイヤーのエンティティを取得
        [ReadOnly] public ComponentDataFromEntity<PlayerTag> PlayerEntity;

        public EntityCommandBuffer entityCommandBuffer;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            // 弾が当たった時のダメージ
            const float HIT_DAMAGE = 1f;

            // プレイヤー同士が接触した場合
            if (PlayerEntity.HasComponent(entityA) && PlayerEntity.HasComponent(entityB))
            {
                return;
            }

            // 敵の弾とプレイヤーが接触した場合は敵の弾を消してプレイヤーのHPを減らす
            if (AllEnemyBulletEntity.HasComponent(entityA) && PlayerEntity.HasComponent(entityB))
            {
                // 敵の弾を消す
                entityCommandBuffer.DestroyEntity(entityA);
                // プレイヤーのhpを減らす
                entityCommandBuffer.SetComponent(entityB, new PlayerTag
                {
                    _playerHp = PlayerEntity[entityB]._playerHp - HIT_DAMAGE
                });
            }
            else if (PlayerEntity.HasComponent(entityA) && AllEnemyBulletEntity.HasComponent(entityB))
            {
                // 敵の弾を消す
                entityCommandBuffer.DestroyEntity(entityB);
                // プレイヤーのhpを減らす
                entityCommandBuffer.SetComponent(entityA, new PlayerTag
                {
                    _playerHp = PlayerEntity[entityA]._playerHp - HIT_DAMAGE
                });
            }
            // 敵とプレイヤーが接触した場合プレイヤーのHPを減らす
            else if (AllEnemyEntity.HasComponent(entityA) && PlayerEntity.HasComponent(entityB))
            {
                // プレイヤーのhpを減らす
                entityCommandBuffer.SetComponent(entityB, new PlayerTag
                {
                    _playerHp = PlayerEntity[entityB]._playerHp - HIT_DAMAGE
                });
            }
            else if (PlayerEntity.HasComponent(entityA) && AllEnemyEntity.HasComponent(entityB))
            {
                // プレイヤーのhpを減らす
                entityCommandBuffer.SetComponent(entityA, new PlayerTag
                {
                    _playerHp = PlayerEntity[entityA]._playerHp - HIT_DAMAGE
                });
            }
        }
    }
}
