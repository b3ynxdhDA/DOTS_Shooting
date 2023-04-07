using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Physics;
using Unity.Physics.Systems;

/// <summary>
/// �G�̓����蔻��𐧌䂷��V�X�e��
/// </summary>
[AlwaysUpdateSystem]
public class EnemyTriggerSystem : SystemBase
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
        enemyTriggerJob.EnemyEntity = GetComponentDataFromEntity<EnemyTag>(true);
        enemyTriggerJob.PlayerBulletsEntity = GetComponentDataFromEntity<PlayerBulletTag>(true);
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
        // �G�̃G���e�B�e�B���擾
        [ReadOnly] public ComponentDataFromEntity<EnemyTag> EnemyEntity;
        // �v���C���[�̒e�̃G���e�B�e�B���擾
        [ReadOnly] public ComponentDataFromEntity<PlayerBulletTag> PlayerBulletsEntity;

        public EntityCommandBuffer entityCommandBuffer;


        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            // �e�������������̃_���[�W
            const float HIT_DAMAGE = 1f;


            // �G���m���ڐG�����ꍇ
            if(EnemyEntity.HasComponent(entityA) && EnemyEntity.HasComponent(entityB))
            {
                return;
            }

            // �G�ƃv���C���[�̒e���ڐG�����ꍇ
            if (EnemyEntity.HasComponent(entityA) && PlayerBulletsEntity.HasComponent(entityB))
            {
                // �v���C���[�̒e������
                entityCommandBuffer.DestroyEntity(entityB);
                // �G�̃G���e�B�e�B��hp�����炷
                entityCommandBuffer.SetComponent(entityA, new EnemyTag
                {
                    _enemyHp = EnemyEntity[entityA]._enemyHp - HIT_DAMAGE
                });
            }
            else if (PlayerBulletsEntity.HasComponent(entityA) && EnemyEntity.HasComponent(entityB))
            {
                // �v���C���[�̒e������
                entityCommandBuffer.DestroyEntity(entityA);
                // �G�̃G���e�B�e�B��hp�����炷
                entityCommandBuffer.SetComponent(entityB, new EnemyTag
                {
                    _enemyHp = EnemyEntity[entityB]._enemyHp - HIT_DAMAGE
                });
            }
        }
    }
}
