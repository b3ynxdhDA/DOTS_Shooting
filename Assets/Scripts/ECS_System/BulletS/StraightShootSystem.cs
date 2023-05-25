using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// �e�𐶐�����V�X�e��
/// </summary>
public class StraightShootSystem : SystemBase
{
    // �ϐ��錾------------------------------------------------------------------
    // ���s�^�C�~���O���Ǘ����Ă���V�X�e���O���[�v
    private BeginSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

    // �萔�錾--------------------------------------------------------
    // �ˌ������̊Ԋu
    const float _SHOOT_SPACE = 0.5f;

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
                        Entity instantiateEntity = comandBuffer.Instantiate(gunporttag._BulletEntity);

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

        // �w�肵��Job�������ECB�ɓo�^�������߂����s
        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}
