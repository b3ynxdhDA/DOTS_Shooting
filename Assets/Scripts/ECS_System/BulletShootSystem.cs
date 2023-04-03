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

    // �ˌ��̊Ԋu
    private float _ShootInterval = 0.15f;

    // �萔�錾--------------------------------------------------------
    // �ˌ������̊Ԋu
    const float _SHOOT_SPACE = 0.5f;

    // �ˌ�����p�x�̊Ԋu
    const float _SHOOT_RAD = 1.5f;
    // �~����
    const float _PI = math.PI;

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
        if(_shootCoolTime > _ShootInterval)
        {
            Entities
                .WithName("Straight_Shoot")
                .WithAll<GunPortTag, StraightGunPortTag>()
                .WithoutBurst()
                .ForEach((in GunPortTag gunporttag, in StraightGunPortTag straightTag, in LocalToWorld localToWorld) =>
                {
                    // ���˂���e�̗�̐��������[�v
                    for (int i = 0; i < straightTag._lines; i++)
                    {
                        // Prefab�ƂȂ�Entity����e�𕡐�����
                        Entity instantiateEntity = comandBuffer.Instantiate(gunporttag._straightBulletEntity);

                        // ���˂̗񂲂Ƃ̈ʒu���v�Z����
                        float3 pos = new float3(localToWorld.Position.x + i - _SHOOT_SPACE * straightTag._lines + _SHOOT_SPACE, localToWorld.Position.y, 0f);
                        
                        // �ʒu�̏�����
                        comandBuffer.SetComponent(instantiateEntity, new Translation
                        {
                            Value = pos
                        });

                        // �e�̌�����������
                        comandBuffer.SetComponent(instantiateEntity, new Rotation
                        {
                            Value = localToWorld.Rotation
                        });

                    }
                }).Run();// ���C���X���b�h�Ŏ��s

            Entities
                .WithName("Wide_Shoot")
                .WithAll<GunPortTag, WideGunPortTag>()
                .WithoutBurst()
                .ForEach((in GunPortTag gunporttag, in WideGunPortTag WideTag, in LocalToWorld localToWorld) =>
                {
                    // ���˂���e�̗�̐��������[�v
                    for (int i = 0; i < WideTag._lines; i++)
                    {
                        // Prefab�ƂȂ�Entity����e�𕡐�����
                        Entity instantiateEntity = comandBuffer.Instantiate(gunporttag._straightBulletEntity);

                        // ���˒n�_�̃��[�J�����W
                        float3 localPos = localToWorld.Position;

                        // ���ˊp�x     �ʓx�@:(_PI / 12) = �x���@:15�x�A�ʓx�@:(_PI / 24) = �x���@:7.5�x
                        float angle = i * (_PI / 12) - WideTag._lines * (_PI / 24) + (_PI / 24);

                        // ���ˊp�����߂邽�߂̍��W
                        float3 position = new float3(localPos.x + _SHOOT_RAD * math.cos(angle), localPos.y + _SHOOT_RAD * math.sin(angle), localPos.z);
                        // ���ˌ�����݂����W�̌���
                        float3 diff = math.normalizesafe(position - localToWorld.Position);

                        // �ʒu�̏�����
                        comandBuffer.SetComponent(instantiateEntity, new Translation
                        {
                            Value = localPos
                        });

                        // �e�̌�����������
                        comandBuffer.SetComponent(instantiateEntity, new Rotation
                        {
                            Value = quaternion.LookRotationSafe(diff, math.up())
                        });

                    }
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
                    Entity instantiateEntity = comandBuffer.Instantiate(gunporttag._straightBulletEntity);

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
