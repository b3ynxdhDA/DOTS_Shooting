using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

/// <summary>
/// �v���C���[�̏�Ԃ��݂�V�X�e��
/// </summary>
public class PlayerStateSystem : SystemBase
{
    // �ϐ��錾------------------------------------------------------------------
    // ���s�^�C�~���O���Ǘ����Ă���V�X�e���O���[�v
    private EndSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

    // �萔�錾--------------------------------------------------------


    /// <summary>
    /// �V�X�e���쐬���ɌĂ΂�鏈��
    /// </summary>
    protected override void OnCreate()
    {
        // EntityCommandBuffer�̎擾
        _entityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    /// <summary>
    /// �V�X�e�����s��~����OnDestroy()�̑O�ɌĂ΂�鏈��
    /// </summary>
    protected override void OnStartRunning()
    {

    }

    /// <summary>
    /// �V�X�e���L�����Ƀt���[�����ɌĂ΂�鏈��
    /// </summary>
    protected override void OnUpdate()
    {
        // �R�}���h�o�b�t�@���擾
        EntityCommandBuffer commandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

        Entities
            .WithName("Player_State")
            .WithAll<PlayerTag>()
            .WithBurst()
            .ForEach((Entity entity, in PlayerTag playerTag) =>
            {
                // �v���C���[��HP��0�ȉ��Ȃ����
                if (playerTag._playerHp <= 0)
                {
                    commandBuffer.DestroyEntity(entity);
                }

            }).WithoutBurst().Run();// ���C���X���b�h����

        // �w�肵��Job�������ECB�ɓo�^�������߂����s
        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}
