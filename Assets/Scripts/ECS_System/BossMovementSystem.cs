using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;

/// <summary>
/// �{�X�L�����N�^�[�̈ړ��V�X�e��
/// </summary>
public class BossMovementSystem : ComponentSystem
{
    // �{�X�L�����N�^�[�̈ړ��̊Ԋu
    private float _moveInterval = 5f;

    // 
    private float _moveTimer = 0;

    // 
    int _nextNum = 0;

    /// <summary>
    /// �V�X�e���L�����Ƀt���[�����ɌĂ΂�鏈��
    /// </summary>
    protected override void OnUpdate()
    {
        // ���O�̃t���[������̌o�ߎ���
        float deltaTime = Time.DeltaTime;

        if (_moveTimer > _moveInterval)
        {
            Entities
                //.WithName("Boss_Move")
                .WithAll<BossEnemyTag>()
                .ForEach((ref Translation translation, DynamicBuffer<BossMoveElement> bossMoves) =>
                {
                    Entity nextPointEntity = bossMoves[1]._movePoints;
                    Translation nextPoint = EntityManager.GetComponentData<Translation>(nextPointEntity);
                    
                    // BulletTag�݂̂����G���e�B�e�B��i�s�����֓�����
                    translation.Value += nextPoint.Value - translation.Value * deltaTime;

                });// ���U����X���b�h����.ScheduleParallel()

            // �^�C�}�[��������
            _moveTimer = 0;
        }
        _moveTimer += Time.DeltaTime;
    }
}
