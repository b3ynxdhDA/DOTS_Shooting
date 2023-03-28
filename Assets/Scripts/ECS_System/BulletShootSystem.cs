using Unity.Entities;
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
                .WithAll<GunPortTag>()
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

                    // ��]�̏�����
                    comandBuffer.SetComponent(instantiateEntity, new Rotation
                    {
                        Value = localToWorld.Rotation
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
