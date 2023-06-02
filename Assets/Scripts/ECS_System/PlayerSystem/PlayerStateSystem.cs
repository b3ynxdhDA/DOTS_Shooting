using Unity.Entities;

/// <summary>
/// �v���C���[�̏�Ԃ��݂�V�X�e��
/// </summary>
public class PlayerStateSystem : SystemBase
{
    // �ϐ��錾------------------------------------------------------------------
    // ���s�^�C�~���O���Ǘ����Ă���V�X�e���O���[�v
    private EndSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

    /// <summary>
    /// �V�X�e���쐬���ɌĂ΂�鏈��
    /// </summary>
    protected override void OnCreate()
    {
        // EntityCommandBuffer�̎擾
        _entityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    /// <summary>
    /// �V�X�e���L�����Ƀt���[�����ɌĂ΂�鏈��
    /// </summary>
    protected override void OnUpdate()
    {
        // �eManager���擾
        GameManager gameManager = GameManager.instance;
        PlayerManager playerManager = gameManager.PlayerManager;

        // �R�}���h�o�b�t�@���擾
        EntityCommandBuffer commandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

        Entities
            .WithName("Player_State")
            .WithAll<PlayerTag>()
            .WithoutBurst()
            .ForEach((Entity entity, ref GunPortTag gunPortTag, in HPTag hpTag) =>
            {
                // �t�B�[���h��OnCreate�ł�Manager���擾�ł��Ȃ������̂�OnUpdate�ŏ�����
                if (!playerManager.IsPlayerInitialize)
                {
                    gameManager.KomaManager.SetKomaDate(entity, playerManager.PlayerKomaData, ref gunPortTag, commandBuffer);
                    playerManager.IsPlayerInitialize = true;
                }

                // �v���C���[��HP��0�ȉ��Ȃ����
                if (hpTag._hp <= 0)
                {
                    commandBuffer.DestroyEntity(entity);
                    gameManager.UIManager.CallGameFinish(false);
                }

            }).Run();// ���C���X���b�h����

        // �w�肵��Job�������ECB�ɓo�^�������߂����s
        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}
