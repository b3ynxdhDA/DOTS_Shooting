using Unity.Entities;
using Unity.Transforms;

/// <summary>
/// 弾を移動させるシステム
/// </summary>
public class BulletMovementSystem : SystemBase
{
    /// <summary>
    /// システム有効時にフレーム毎に呼ばれる処理
    /// </summary>
    protected override void OnUpdate()
    {
        // 直前のフレームからの経過時間
        float deltaTime = Time.DeltaTime;

        Entities
            .WithName("Bullet_Move")
            .WithAll<BulletTag>()
            .WithNone<AutoAimBulletTag>()
            .ForEach((ref Translation translation, in LocalToWorld localToWorld, in BulletTag bulletTag) =>
            {
                // BulletTagのみを持つエンティティを進行方向へ動かす
                translation.Value += localToWorld.Up * bulletTag._bulletSpeed * deltaTime;
            
            }).ScheduleParallel();// 分散並列スレッド処理

        Entities
            .WithName("Aim_Bullet_Move")
            .WithAll<BulletTag, AutoAimBulletTag>()
            .ForEach((ref Translation translation, in AutoAimBulletTag autoAimBulletTag, in BulletTag bulletTag) =>
            {
                // AutoAimBulletTagのターゲット狙いで移動
                translation.Value += autoAimBulletTag._moveDirection * bulletTag._bulletSpeed * deltaTime;
            
            }).ScheduleParallel();// 分散並列スレッド処理
    }
}
