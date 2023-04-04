using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;

/// <summary>
/// ボスキャラクターの移動システム
/// </summary>
public class BossMovementSystem : ComponentSystem
{
    // ボスキャラクターの移動の間隔
    private float _moveInterval = 5f;

    // 
    private float _moveTimer = 0;

    // 
    int _nextNum = 0;

    /// <summary>
    /// システム有効時にフレーム毎に呼ばれる処理
    /// </summary>
    protected override void OnUpdate()
    {
        // 直前のフレームからの経過時間
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
                    
                    // BulletTagのみを持つエンティティを進行方向へ動かす
                    translation.Value += nextPoint.Value - translation.Value * deltaTime;

                });// 分散並列スレッド処理.ScheduleParallel()

            // タイマーを初期化
            _moveTimer = 0;
        }
        _moveTimer += Time.DeltaTime;
    }
}
