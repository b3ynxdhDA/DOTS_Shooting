using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using System;

/// <summary>
/// �v���C���[�̓��͂��󂯎��V�X�e��
/// </summary>
public class PlayerInputSystem : SystemBase
{
    /// <summary>
    /// �V�X�e���L�����Ƀt���[�����ɌĂ΂�鏈��
    /// </summary>
    protected override void OnUpdate()
    {
        // ���O�̃t���[������̌o�ߎ���
        float deltaTime = Time.DeltaTime;
        // ���������̓���
        float inputVertical = Input.GetAxis("Vertical");
        // ���������̓���
        float inputHorizontal = Input.GetAxis("Horizontal");

        Entities
            .WithAll<PlayerTag>()
            .ForEach((ref Translation translation, in PlayerTag playerTag) =>
            {
                translation.Value.x += inputHorizontal * deltaTime * playerTag._moveSpeed;
                translation.Value.y += inputVertical * deltaTime * playerTag._moveSpeed;

            }).Run();
    }
}
