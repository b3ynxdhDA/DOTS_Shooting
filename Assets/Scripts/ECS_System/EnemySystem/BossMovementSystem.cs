using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;

/// <summary>
/// �{�X�L�����N�^�[�̈ړ��V�X�e��
/// </summary>
public class BossMovementSystem : ComponentSystem
{
    // �ϐ��錾------------------------------------------------------------------
    // �{�X�L�����N�^�[�̈ړ��̊Ԋu
    private float _moveInterval = 5f;

    // ���̈ړ��n�_
    float3 nextPoint;

    // �O�̈ړ��n�_
    float3 lastPoint;

    // �ړ��������
    float3 moveDirection;

    // �萔�錾------------------------------------------------------------------
    // �ړ��C���^�[�o���̍ő�
    const float _INTERVAL_MAX = 8f;

    // �ړ��C���^�[�o���̍ŏ�
    const float _INTERVAL_MIN = 3f;

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
            .WithAll<BossEnemyTag>()
            .ForEach((ref Translation translation, DynamicBuffer<BossMoveElement> dynamicBuffer) =>
            {
                if (_moveInterval < 0)
                {
                    // �^�C�}�[��������
                    _moveInterval = UnityEngine.Random.Range(_INTERVAL_MIN, _INTERVAL_MAX);

                    // �����_���Ɏ��̈ړ��n�_�̈�����ݒ�
                    int _nextMovePointNum = UnityEngine.Random.Range(0, dynamicBuffer.Length);

                    // ���̈ړ��n�_�̍��W���擾
                    BossMoveElement bossMoveElement = dynamicBuffer[_nextMovePointNum];
                    nextPoint = bossMoveElement._movePoints;

                    // ���ݒn��O�̈ړ��n�_�Ƃ��ċL�^
                    lastPoint = translation.Value;

                    // �ړ�����������v�Z
                    moveDirection = nextPoint - lastPoint;

                }

                // �O�̈ړ��n�_���猻�ݒn�ւ̋������ړ��n�_���m�̋�����菬������
                if (math.distance(translation.Value, lastPoint) < math.distance(nextPoint, lastPoint))
                {
                    // ������
                    translation.Value += moveDirection * deltaTime;
                }

            });// ���U����X���b�h����.ScheduleParallel()

        _moveInterval -= Time.DeltaTime;
    }
}
