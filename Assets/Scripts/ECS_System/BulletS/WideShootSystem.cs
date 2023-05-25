using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// �e�𐶐�����V�X�e��
/// </summary>
public class WideShootSystem : SystemBase
{
    // �ϐ��錾------------------------------------------------------------------
    // ���s�^�C�~���O���Ǘ����Ă���V�X�e���O���[�v
    private BeginSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

    // �萔�錾--------------------------------------------------------
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
                        Entity instantiateEntity = comandBuffer.Instantiate(gunporttag._BulletEntity);

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

        // �w�肵��Job�������ECB�ɓo�^�������߂����s
        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }

}
