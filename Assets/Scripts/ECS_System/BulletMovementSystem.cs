using Unity.Entities;
using Unity.Transforms;

/// <summary>
/// �e���ړ�������V�X�e��
/// </summary>
public class BulletMovementSystem : SystemBase
{
    /// <summary>
    /// �V�X�e���L�����Ƀt���[�����ɌĂ΂�鏈��
    /// </summary>
    protected override void OnUpdate()
    {
        // ���O�̃t���[������̌o�ߎ���
        float deltaTime = Time.DeltaTime;

        Entities
            .WithName("Bullet_Move")
            .WithAll<BulletTag>()
            .ForEach((ref Translation translation, in BulletTag bulletTag, in BulletMoveDirectionTag bulletMoveDirection) =>
            {
                // BulletTag�����G���e�B�e�B��i�s�����֓�����
                translation.Value += bulletMoveDirection._moveDirection * bulletTag._bulletSpeed * deltaTime;
            
            }).ScheduleParallel();// ���U����X���b�h����
    }
}
