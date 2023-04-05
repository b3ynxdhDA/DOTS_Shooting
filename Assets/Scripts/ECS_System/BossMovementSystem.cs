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

    // ���̈ړ��n�_�̈���
    int _nextMovePointNum = 0;

    // 
    float3 nextPoint;

    //
    float3 moveVolume;

    // �萔�錾------------------------------------------------------------------
    // 
    const float _INTERVAL_MAX = 8f;

    //
    const float _INTERVAL_MIN = 3f;



    /// <summary>
    /// �V�X�e���L�����Ƀt���[�����ɌĂ΂�鏈��
    /// </summary>
    protected override void OnUpdate()
    {
        // ���O�̃t���[������̌o�ߎ���
        float deltaTime = Time.DeltaTime;
        Entities
            //.WithName("Boss_Move")
            .WithAll<BossEnemyTag>()
            .ForEach((ref Translation translation, DynamicBuffer<BossMoveElement> dynamicBuffer) =>
            {
                if (_moveInterval < 0)
                {
                    // �^�C�}�[��������
                    _moveInterval = UnityEngine.Random.Range(_INTERVAL_MIN, _INTERVAL_MAX);
                    // �����_���Ɏ��̈ړ��n�_�̈�����ݒ�
                    _nextMovePointNum = UnityEngine.Random.Range(0, dynamicBuffer.Length);

                    // 
                    BossMoveElement bossMoveElement = dynamicBuffer[_nextMovePointNum];

                    nextPoint = bossMoveElement._movePoints;

                    moveVolume = nextPoint - translation.Value * 0.1f;

                }

                // �{�X�̌��ݒn��X,Y�Ƃ��Ɏ��̈ړ��n�_��菬�����ꍇ
                // @@if (moveVolume.x == 0 && moveVolume.y == 0)@@
                {
                    // ������
                    translation.Value += moveVolume * deltaTime;
                }
                 

            });// ���U����X���b�h����.ScheduleParallel()

        _moveInterval -= Time.DeltaTime;
    }
}
