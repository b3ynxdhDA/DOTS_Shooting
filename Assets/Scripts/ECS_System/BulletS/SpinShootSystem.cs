using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// ��]����e�𐶐�����V�X�e��
/// </summary>
[AlwaysUpdateSystem]
public class SpinShootSystem : SystemBase
{
    // �ϐ��錾------------------------------------------------------------------
    // ���s�^�C�~���O���Ǘ����Ă���V�X�e���O���[�v
    private BeginSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

    private int _spinRad = 1;

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
        // ���O�̃t���[������̌o�ߎ���
        float deltaTime = Time.DeltaTime;

        Entities
            .WithName("Spin_Shoot")
            .WithoutBurst()
            .WithAll<GunPortTag, SpinGunPortTag>()
            .ForEach((ref GunPortTag gunPortTag, in SpinGunPortTag spinGunPortTag, in LocalToWorld localToWorld) =>
            {
                // ��]���̖@�����v�Z����
                //quaternion normalizedRotation = math.normalizesafe(rotation.Value);
                // ��]������p�x���v�Z����
                //quaternion angleToRotate = quaternion.AxisAngle(math.forward(), spinGunPortTag._spinSpeed * deltaTime);

                //rotation.Value = math.mul(normalizedRotation, angleToRotate);
                if (_spinRad >= int.MaxValue)
                {
                    _spinRad = 0;
                    
                }

                // �Q�[���̃X�e�[�g���Q�[�����ȊO�Ȃ珈�����Ȃ�
                if (GameManager.instance.gameState != GameManager.GameState.GameNow)
                {
                    return;
                }

                // �R�}���h�o�b�t�@���擾
                EntityCommandBuffer comandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

                // �N�[���^�C����0��菬����������e�𔭎˂���
                if (gunPortTag._shootInterval < 0)
                {
                    // Prefab�ƂȂ�Entity����e�𕡐�����
                    Entity instantiateEntity = comandBuffer.Instantiate(gunPortTag._BulletEntity);

                    // ���˒n�_�̃��[�J�����W
                    float3 localPos = localToWorld.Position;

                    // ���ˊp�x     �ʓx�@:(_PI / 12) = �x���@:15�x�A�ʓx�@:(_PI / 24) = �x���@:7.5�x
                    float angle = _spinRad * (_PI / 12) * (_PI / 24) + (_PI / 24);

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
                        _bulletSpeed = gunPortTag._bulletSpeed
                    });

                    // ���ˊp�����炷
                    _spinRad += spinGunPortTag._spinSpeed;

                    // �C���^�[�o�����Z�b�g����
                    gunPortTag._shootInterval = gunPortTag._shootCoolTime;
                }
                // �C���^�[�o������o�ߎ��Ԃ����炷
                gunPortTag._shootInterval -= Time.DeltaTime;

            }).Run();// ���C���X���b�h�Ŏ��s

    }
}
