using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Physics;
using Unity.Physics.Systems;

/// <summary>
/// �v���C���[�̓����蔻��𐧌䂷��V�X�e��
/// </summary>
[AlwaysUpdateSystem]
public class PlayerTriggerSystem : SystemBase
{
    // �ϐ��錾------------------------------------------------------------------
    // ���s�^�C�~���O���Ǘ����Ă���V�X�e���O���[�v
    private EndSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

    // ���[���h
    private BuildPhysicsWorld _buildPhysicsWorld;
    // 
    private StepPhysicsWorld _stepPhysicsWorld;

    /// <summary>
    /// �V�X�e���쐬���ɌĂ΂�鏈��
    /// </summary>
    protected override void OnCreate()
    {
        // BuildPhysicsWorld��StepPhysicsWorld���擾
        _buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        _stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();

        // EntityCommandBuffer�̎擾
        _entityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    /// <summary>
    /// �V�X�e���L�����Ƀt���[�����ɌĂ΂�鏈��
    /// </summary>
    protected override void OnUpdate()
    {
        // �W���u�̐���
        EnemyTriggerJob enemyTriggerJob = new EnemyTriggerJob();
        enemyTriggerJob.allEnemyBulletEntity = GetComponentDataFromEntity<EnemyBulletTag>(true);
        enemyTriggerJob.PlayerEntity = GetComponentDataFromEntity<PlayerTag>(true);
        enemyTriggerJob.entityCommandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

        // �W���u�̎��s
        Dependency = enemyTriggerJob.Schedule(
            _stepPhysicsWorld.Simulation,
            ref _buildPhysicsWorld.PhysicsWorld,
            Dependency);

        // �������ׂ��W���u���R�}���h�o�b�t�@�V�X�e���ɒǉ�
        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }

    [BurstCompile]
    struct EnemyTriggerJob : ITriggerEventsJob
    {
        // �G�̒e�̃G���e�B�e�B���擾
        [ReadOnly] public ComponentDataFromEntity<EnemyBulletTag> allEnemyBulletEntity;
        // �v���C���[�̃G���e�B�e�B���擾
        [ReadOnly] public ComponentDataFromEntity<PlayerTag> PlayerEntity;

        public EntityCommandBuffer entityCommandBuffer;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            // �G�̒e���m���ڐG�����ꍇ
            if(allEnemyBulletEntity.HasComponent(entityA) && allEnemyBulletEntity.HasComponent(entityB))
            {
                return;
            }

            // �G�̒e�ƃv���C���[���ڐG�����ꍇ
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
