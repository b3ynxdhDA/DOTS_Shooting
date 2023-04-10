using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// ��]����e�𐶐�����V�X�e��
/// </summary>
[AlwaysUpdateSystem]
public class SpinShootSystem : SystemBase
{
    /// <summary>
    /// �V�X�e���L�����Ƀt���[�����ɌĂ΂�鏈��
    /// </summary>
    protected override void OnUpdate()
    {
        // ���O�̃t���[������̌o�ߎ���
        float deltaTime = Time.DeltaTime;

        Entities
            .WithName("Spin_Shoot")
            .WithAll<SpinGunPortTag>()
            .ForEach((ref Rotation rotation, in SpinGunPortTag spinGunPortTag) =>
            {
                // ��]���̖@�����v�Z����
                quaternion normalizedRotation = math.normalizesafe(rotation.Value);
                // ��]������p�x���v�Z����
                quaternion angleToRotate = quaternion.AxisAngle(math.forward(), spinGunPortTag._spinSpeed * deltaTime);

                rotation.Value = math.mul(normalizedRotation, angleToRotate);

            }).ScheduleParallel();// ���U����X���b�h����

    }
}
