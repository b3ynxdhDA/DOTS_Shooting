using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// �e�𐶐�����V�X�e��
/// </summary>
public class AimShootSystem : SystemBase
{
    // �ϐ��錾------------------------------------------------------------------
    // ���s�^�C�~���O���Ǘ����Ă���V�X�e���O���[�v
    private BeginSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

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
                    Entity instantiateEntity = comandBuffer.Instantiate(gunporttag._BulletEntity);

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
