using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// �v���C���[�̓��͂��󂯎��V�X�e��
/// </summary>
public class PlayerMovementSystem : SystemBase
{
    // �ϐ��錾------------------------------------------------------------------
    private Camera _mainCamera;

    // �ړ��\�͈͂̍����̃��[���h���W��
    private Vector2 _lowerLeft;

    // �ړ��\�͈͂̉E��̃��[���h���W
    private Vector2 _upperRight;

    // �萔�錾--------------------------------------------------------
    // �ړ��\�͈͂̍����̃X�N���[�����W@�����Ղ����͈̔�new Vector2(0.28f, 0.08f)
    private readonly Vector2 _MIN_SCREEN_POINT = new Vector2(0.02f, 0.08f);
    // �ړ��\�͈͂̉E��̃X�N���[�����W@�����Ղ����͈̔�new Vector2(0.28f, 0.08f)
    private readonly Vector2 _MAX_SCREEN_POINT = new Vector2(0.98f, 0.92f);

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
        // �Q�[���̃X�e�[�g���Q�[�����ȊO�Ȃ珈�����Ȃ�
        if (GameManager.instance.gameState != GameManager.GameState.GameNow)
        {
            return;
        }

        // ���O�̃t���[������̌o�ߎ���
        float deltaTime = Time.DeltaTime;

        Entities
            .WithName("Player_Input")
            .WithoutBurst()
            .WithAll<PlayerTag>()
            .ForEach((ref Translation translation, in PlayerMoveDate playerMoveDate) =>
            {
                // �v���C���[�̈ړ�
                translation.Value += playerMoveDate._moveDirection * playerMoveDate._moveSpeed * deltaTime;

                // �v���C���[����ʊO�ɏo�Ȃ��悤�ɐ�������Clamp����
                translation.Value.x = Mathf.Clamp(translation.Value.x, _lowerLeft.x, _upperRight.x);
                translation.Value.y = Mathf.Clamp(translation.Value.y, _lowerLeft.y, _upperRight.y);

            }).Run();// ���C���X���b�h�ŏ���
    }
}
