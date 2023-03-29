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
    // �ϐ��錾------------------------------------------------------------------
    private Camera _mainCamera;

    // �X�N���[���̍����̃��[���h���W��
    private Vector2 _lowerLeft;

    // �X�N���[���̉E��̃��[���h���W
    private Vector2 _upperRight;

    // �萔�錾--------------------------------------------------------
    // �X�N���[�����W�̍���
    private readonly Vector2 _MIN_SCREEN_POINT = new Vector2(0.1f, 0.1f);
    // �X�N���[�����W�̉E��
    private readonly Vector2 _MAX_SCREEN_POINT = new Vector2(0.9f, 0.9f);

    /// <summary>
    /// �V�X�e�����s��~����OnDestroy()�̑O�ɌĂ΂�鏈��
    /// �J�����̐؂�ւ��ɑΉ����邽�߂����ŃJ�������擾
    /// </summary>
    protected override void OnStartRunning()
    {
        // �X�N���[�����W�����[���h���W�ɕϊ�����
        _mainCamera = Camera.main;
        _lowerLeft = _mainCamera.ViewportToWorldPoint(_MIN_SCREEN_POINT);
        _upperRight = _mainCamera.ViewportToWorldPoint(_MAX_SCREEN_POINT);
    }
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
            .WithName("Player_Input")
            .WithAll<PlayerTag>()
            .ForEach((ref Translation translation, in PlayerTag playerTag) =>
            {
                // �v���C���[�̈ړ�
                translation.Value.x += inputHorizontal * deltaTime * playerTag._moveSpeed;
                translation.Value.y += inputVertical * deltaTime * playerTag._moveSpeed;

                // �v���C���[����ʊO�ɏo�Ȃ��悤�ɐ�������
                translation.Value.x = Mathf.Clamp(translation.Value.x, _lowerLeft.x, _upperRight.x);
                translation.Value.y = Mathf.Clamp(translation.Value.y, _lowerLeft.y, _upperRight.y);

            }).WithoutBurst().Run();// ���C���X���b�h�ŏ���
    }
}
