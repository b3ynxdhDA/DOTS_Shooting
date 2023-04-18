using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// �e�𐶐�����V�X�e��
/// </summary>
public class BulletShootSystem : SystemBase
{
    // �ϐ��錾------------------------------------------------------------------
    // ���s�^�C�~���O���Ǘ����Ă���V�X�e���O���[�v
    private BeginSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

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
        // �Q�[���̃X�e�[�g���Q�[�����ȊO�Ȃ珈�����Ȃ�
        if (GameManager.instance.gameState != GameManager.GameState.GameNow)
        {
            return;
        }

        // �R�}���h�o�b�t�@���擾
        EntityCommandBuffer comandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

        Entities
            .WithName("Straight_Shoot")
            .WithAll<GunPortTag, StraightGunPortTag>()
            .WithoutBurst()
            .ForEach((ref GunPortTag gunporttag, in StraightGunPortTag straightTag, in LocalToWorld localToWorld) =>
            {
                // �N�[���^�C����0��菬����������e�𔭎˂���
                if (gunporttag._shootInterval < 0)
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

                        // �e�̐i�ފp�x��ݒ�
                        comandBuffer.SetComponent(instantiateEntity, new BulletMoveDirectionTag
                        {
                            _moveDirection = localToWorld.Up
                        });

                        // �e�̐i�ޑ��x��ݒ�
                        comandBuffer.SetComponent(instantiateEntity, new BulletTag
                        {
                            _bulletSpeed = gunporttag._bulletSpeed
                        });
                    }
                    // �C���^�[�o�����Z�b�g����
                    gunporttag._shootInterval = gunporttag._shootCoolTime;
                }
                // �C���^�[�o������o�ߎ��Ԃ����炷
                gunporttag._shootInterval -= Time.DeltaTime;
            }).Run();// ���C���X���b�h�Ŏ��s

        Entities
            .WithName("Wide_Shoot")
            .WithAll<GunPortTag, WideGunPortTag>()
            .WithoutBurst()
            .ForEach((ref GunPortTag gunporttag, in WideGunPortTag WideTag, in LocalToWorld localToWorld) =>
            {
                // �N�[���^�C����0��菬����������e�𔭎˂���
                if (gunporttag._shootInterval < 0)
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
                            Value = localToWorld.Position
                        });

                        // �e�̌�����������
                        comandBuffer.SetComponent(instantiateEntity, new Rotation
                        {
                            Value = quaternion.LookRotationSafe(diff, math.up())
                        });

                        // �e�̐i�ފp�x��ݒ�
                        comandBuffer.SetComponent(instantiateEntity, new BulletMoveDirectionTag
                        {
                            // �������ɐi��ł����̂�diff��x��y���t�ɂ��āAy�ɋ�Ƃ�localToWorld��y���|���Č����𒲐�
                            _moveDirection = new float3(diff.y, diff.x * math.sign(localToWorld.Up.y), diff.z)
                        });

                        // �e�̐i�ޑ��x��ݒ�
                        comandBuffer.SetComponent(instantiateEntity, new BulletTag
                        {
                            _bulletSpeed = gunporttag._bulletSpeed
                        });
                    }
                    // �C���^�[�o�����Z�b�g����
                    gunporttag._shootInterval = gunporttag._shootCoolTime;
                }
                // �C���^�[�o������o�ߎ��Ԃ����炷
                gunporttag._shootInterval -= Time.DeltaTime;
            }).Run();// ���C���X���b�h�Ŏ��s

        Entities
            .WithName("Aim_Shoot")
            .WithAll<GunPortTag, AimGunPortTag>()
            .WithoutBurst()
            .ForEach((ref GunPortTag gunporttag, in AimGunPortTag aimgunporttag, in LocalToWorld localToWorld) =>
            {
                // �N�[���^�C����0��菬����������e�𔭎˂���
                if (gunporttag._shootInterval < 0)
                {
                    // ���ˑ䂩�猩���^�[�Q�b�g�̌�����������ɂ��ď�����
                    float3 direction = localToWorld.Up;

                    // �^�[�Q�b�g�̃G���e�B�e�B�����݂���Ƃ�
                    if (aimgunporttag._targetEntity != Entity.Null)
                    {
                        // �^�[�Q�b�g�̃��[�J�����W���擾
                        ComponentDataFromEntity<Translation> translationComponentData = GetComponentDataFromEntity<Translation>(true);
                        float3 targetPosition = translationComponentData[aimgunporttag._targetEntity].Value;

                        // ���ˑ䂩�猩���^�[�Q�b�g�̌������v�Z
                        direction = math.normalizesafe(targetPosition - localToWorld.Position);
                    }
                    // Prefab��Entity����e�𕡐�����
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

                    // �e�̐i�ފp�x��ݒ�
                    comandBuffer.SetComponent(instantiateEntity, new BulletMoveDirectionTag
                    {
                        _moveDirection = direction
                    });

                    // �e�̐i�ޑ��x��ݒ�
                    comandBuffer.SetComponent(instantiateEntity, new BulletTag
                    {
                        _bulletSpeed = gunporttag._bulletSpeed
                    });
                    // �C���^�[�o�����Z�b�g����
                    gunporttag._shootInterval = gunporttag._shootCoolTime;
                }
                // �C���^�[�o������o�ߎ��Ԃ����炷
                gunporttag._shootInterval -= Time.DeltaTime;
            }).Run();// ���C���X���b�h�Ŏ��s

        // �w�肵��Job�������ECB�ɓo�^�������߂����s
        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }

}
