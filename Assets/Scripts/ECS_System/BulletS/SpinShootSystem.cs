using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// 回転する弾を生成するシステム
/// </summary>
[AlwaysUpdateSystem]
public class SpinShootSystem : SystemBase
{
    /// <summary>
    /// システム有効時にフレーム毎に呼ばれる処理
    /// </summary>
    protected override void OnUpdate()
    {
        // 直前のフレームからの経過時間
        float deltaTime = Time.DeltaTime;

        Entities
            .WithName("Spin_Shoot")
            .WithAll<SpinGunPortTag>()
            .ForEach((ref Rotation rotation, in SpinGunPortTag spinGunPortTag) =>
            {
                // 回転軸の法線を計算する
                quaternion normalizedRotation = math.normalizesafe(rotation.Value);
                // 回転させる角度を計算する
                quaternion angleToRotate = quaternion.AxisAngle(math.forward(), spinGunPortTag._spinSpeed * deltaTime);

                rotation.Value = math.mul(normalizedRotation, angleToRotate);

            }).ScheduleParallel();// 分散並列スレッド処理

    }
}
