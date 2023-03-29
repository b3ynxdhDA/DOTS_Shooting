using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// �e�𐶐�����V�X�e��
/// </summary>
[AlwaysUpdateSystem]
public class BulletShootSystem : SystemBase
{
    // �ϐ��錾------------------------------------------------------------------
    // ���s�^�C�~���O���Ǘ����Ă���V�X�e���O���[�v
    private BeginSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

    // �e�𔭎˂��Ă���o�߂�������
    private float _shootCoolTime;

    // �萔�錾--------------------------------------------------------
    // �ˌ��̊Ԋu
    const float _SHOOT_INTERVAL = 0.15f;

    /// <summary>
    /// �V�X�e���쐬���ɌĂ΂�鏈��
    /// </summary>
    protected override void OnCreate()
    {
        // EntityCommandBuffer�̎擾
        _entityCommandBufferSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
    }

    /// <summary>
    /// �V�X�e���L�����Ƀt���[�����ɌĂ΂�鏈��
    /// </summary>
    protected override void OnUpdate()
    {
        // �R�}���h�o�b�t�@���擾
        EntityCommandBuffer comandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

        // �N�[���^�C�����C���^�[�o�����傫��������e�𔭎˂���
        if(_shootCoolTime > _SHOOT_INTERVAL)
        {
            Entities
                .WithName("Bullet_Shoot")
                .WithAll<GunPortTag>()
                .WithNone<AimGunPortTag>()
                .WithoutBurst()
                .ForEach((in GunPortTag gunporttag, in LocalToWorld localToWorld) =>
                {
                    // Prefab�ƂȂ�Entity����e�𕡐�����
                    Entity instantiateEntity = comandBuffer.Instantiate(gunporttag._prefabEntity);

                    // �ʒu�̏�����
                    comandBuffer.SetComponent(instantiateEntity, new Translation
                    {
                        Value = localToWorld.Position
                    });

                    // �e�̌�����������
                    comandBuffer.SetComponent(instantiateEntity, new Rotation
                    {
                        Value = localToWorld.Rotation
                    });

                }).Run();// ���C���X���b�h�Ŏ��s

            Entities
                .WithName("Aim_Shoot")
                .WithAll<GunPortTag, AimGunPortTag>()
                .WithoutBurst()
                .ForEach((in GunPortTag gunporttag, in AimGunPortTag aimgunporttag, in LocalToWorld localToWorld) =>
                {
                    // �^�[�Q�b�g�̃��[�J�����W���擾
                    //EntityQuery aimTargetEntityQuery = EntityManager.CreateEntityQuery(typeof(EnemyTag));
                    //Entity aimTargetEntity = aimTargetEntityQuery.GetSingletonEntity();
                    LocalToWorld aimTargetLocalToWorld = GetComponent<LocalToWorld>(aimgunporttag._targetEntity);

                    // ���ˑ䂩�猩���v���C���[�̌���
                    float3 direction = math.normalizesafe(aimTargetLocalToWorld.Position - localToWorld.Position);

                    // Prefab�ƂȂ�Entity����e�𕡐�����
                    Entity instantiateEntity = comandBuffer.Instantiate(gunporttag._prefabEntity);

                    // �ʒu�̏�����
                    comandBuffer.SetComponent(instantiateEntity, new Translation
                    {
                        Value = localToWorld.Position
                    });

                    // �e�̌�����������
                    comandBuffer.SetComponent(instantiateEntity, new Rotation
                    {
                        Value = quaternion.LookRotationSafe(direction, math.forward())
                    });

                    // �I�[�g�G�C���e�̐i�ފp�x��ݒ�
                    comandBuffer.SetComponent(instantiateEntity, new AutoAimBulletTag
                    {
                        _moveDirection = direction
                    });

                }).Run();// ���C���X���b�h�Ŏ��s

            // �w�肵��Job�������ECB�ɓo�^�������߂����s
            _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);

            // �N�[���^�C����0�ɂ���
            _shootCoolTime = 0;
        }

        // �N�[���^�C���Ɍo�ߎ��Ԃ𔽉f
        _shootCoolTime += Time.DeltaTime;
    }
}
