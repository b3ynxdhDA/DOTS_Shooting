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
        PlayerTriggerJob playerTriggerJob = new PlayerTriggerJob();
        playerTriggerJob.AllEnemyBulletEntity = GetComponentDataFromEntity<EnemyBulletTag>(true);
        playerTriggerJob.AllEnemyEntity = GetComponentDataFromEntity<EnemyTag>(true);
        playerTriggerJob.PlayerEntity = GetComponentDataFromEntity<PlayerTag>(true);
        playerTriggerJob.entityCommandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

        // �W���u�̎��s
        Dependency = playerTriggerJob.Schedule(
            _stepPhysicsWorld.Simulation,
            ref _buildPhysicsWorld.PhysicsWorld,
            Dependency);

        // �������ׂ��W���u���R�}���h�o�b�t�@�V�X�e���ɒǉ�
        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }

    [BurstCompile]
    struct PlayerTriggerJob : ITriggerEventsJob
    {
        // �G�̒e�̃G���e�B�e�B���擾
        [ReadOnly] public ComponentDataFromEntity<EnemyBulletTag> AllEnemyBulletEntity;
        // �G�̃G���e�B�e�B���擾
        [ReadOnly] public ComponentDataFromEntity<EnemyTag> AllEnemyEntity;
        // �v���C���[�̃G���e�B�e�B���擾
        [ReadOnly] public ComponentDataFromEntity<PlayerTag> PlayerEntity;

        public EntityCommandBuffer entityCommandBuffer;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            // �e�������������̃_���[�W
            const float HIT_DAMAGE = 1f;

            // �v���C���[���m���ڐG�����ꍇ
            if (PlayerEntity.HasComponent(entityA) && PlayerEntity.HasComponent(entityB))
            {
                return;
            }

            // �G�̒e�ƃv���C���[���ڐG�����ꍇ�͓G�̒e�������ăv���C���[��HP�����炷
            if (AllEnemyBulletEntity.HasComponent(entityA) && PlayerEntity.HasComponent(entityB))
            {
                // �G�̒e������
                entityCommandBuffer.DestroyEntity(entityA);
                // �v���C���[��hp�����炷
                entityCommandBuffer.SetComponent(entityB, new PlayerTag
                {
                    _playerHp = PlayerEntity[entityB]._playerHp - HIT_DAMAGE
                });
            }
            else if (PlayerEntity.HasComponent(entityA) && AllEnemyBulletEntity.HasComponent(entityB))
            {
                // �G�̒e������
                entityCommandBuffer.DestroyEntity(entityB);
                // �v���C���[��hp�����炷
                entityCommandBuffer.SetComponent(entityA, new PlayerTag
                {
                    _playerHp = PlayerEntity[entityA]._playerHp - HIT_DAMAGE
                });
            }
            // �G�ƃv���C���[���ڐG�����ꍇ�v���C���[��HP�����炷
            else if (AllEnemyEntity.HasComponent(entityA) && PlayerEntity.HasComponent(entityB))
            {
                // �v���C���[��hp�����炷
                entityCommandBuffer.SetComponent(entityB, new PlayerTag
                {
                    _playerHp = PlayerEntity[entityB]._playerHp - HIT_DAMAGE
                });
            }
            else if (PlayerEntity.HasComponent(entityA) && AllEnemyEntity.HasComponent(entityB))
            {
                // �v���C���[��hp�����炷
                entityCommandBuffer.SetComponent(entityA, new PlayerTag
                {
                    _playerHp = PlayerEntity[entityA]._playerHp - HIT_DAMAGE
                });
            }
        }
    }
}
