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
        // �ᑬ���[�h�̓���
        bool inputSlowButtonDown = Input.GetButtonDown("Slow");
        // �ᑬ���[�h�̓���
        bool inputSlowButtonUp = Input.GetButtonUp("Slow");

        Entities
            .WithName("Player_Input")
            .WithAll<PlayerTag>()
            .ForEach((ref PlayerMoveDate playerMove) =>
            {
                // �v���C���[�̈ړ�
                playerMove._moveDirection.x = inputHorizontal;
                playerMove._moveDirection.y = inputVertical;

                if (inputSlowButtonDown || inputSlowButtonUp)
                {
                    float tmp = playerMove._moveSpeed;

                    playerMove._moveSpeed = playerMove._slowSpeed;

                    playerMove._slowSpeed = tmp;
                }

            }).Run();// ���C���X���b�h�ŏ���
    }
}
