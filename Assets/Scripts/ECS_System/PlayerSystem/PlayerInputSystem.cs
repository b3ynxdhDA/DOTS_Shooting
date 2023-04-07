using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// �v���C���[�̓��͂��󂯎��V�X�e��
/// </summary>
public class PlayerInputSystem : SystemBase
{
    // �ϐ��錾------------------------------------------------------------------

    // �萔�錾--------------------------------------------------------

    /// <summary>
    /// �V�X�e���L�����Ƀt���[�����ɌĂ΂�鏈��
    /// </summary>
    protected override void OnUpdate()
    {
        // ���������̓���
        float inputVertical = Input.GetAxis("Vertical");
        // ���������̓���
        float inputHorizontal = Input.GetAxis("Horizontal");

        Entities
            .WithName("Player_Input")
            .WithAll<PlayerTag>()
            .ForEach((ref PlayerMoveDate playerMove) =>
            {
                // �v���C���[�̈ړ�
                playerMove._moveDirection.x = inputHorizontal;
                playerMove._moveDirection.y = inputVertical;

            }).Run();// ���C���X���b�h�ŏ���
    }
}
