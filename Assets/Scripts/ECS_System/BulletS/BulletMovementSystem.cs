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
            .ForEach((ref Translation translation, in BulletTag bulletTag, in BulletMoveDirectionTag bulletMoveDirection) =>
            {
                // BulletTagを持つエンティティを進行方向へ動かす
                translation.Value += bulletMoveDirection._moveDirection * bulletTag._bulletSpeed * deltaTime;
            
            }).ScheduleParallel();// 分散並列スレッド処理
    }
}
