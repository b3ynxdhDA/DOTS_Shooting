using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;

/// <summary>
/// ボスキャラクターの移動システム
/// </summary>
public class BossMovementSystem : ComponentSystem
{
    // 変数宣言------------------------------------------------------------------
    // ボスキャラクターの移動の間隔
    private float _moveInterval = 5f;

    // 次の移動地点の引数
    int _nextMovePointNum = 0;

    // 
    float3 nextPoint;

    //
    float3 moveVolume;

    // 定数宣言------------------------------------------------------------------
    // 
    const float _INTERVAL_MAX = 8f;

    //
    const float _INTERVAL_MIN = 3f;



    /// <summary>
    /// システム有効時にフレーム毎に呼ばれる処理
    /// </summary>
    protected override void OnUpdate()
    {
        // 直前のフレームからの経過時間
        float deltaTime = Time.DeltaTime;
        Entities
            //.WithName("Boss_Move")
            .WithAll<BossEnemyTag>()
            .ForEach((ref Translation translation, DynamicBuffer<BossMoveElement> dynamicBuffer) =>
            {
                if (_moveInterval < 0)
                {
                    // タイマーを初期化
                    _moveInterval = UnityEngine.Random.Range(_INTERVAL_MIN, _INTERVAL_MAX);
                    // ランダムに次の移動地点の引数を設定
                    _nextMovePointNum = UnityEngine.Random.Range(0, dynamicBuffer.Length);

                    // 
                    BossMoveElement bossMoveElement = dynamicBuffer[_nextMovePointNum];

                    nextPoint = bossMoveElement._movePoints;

                    moveVolume = nextPoint - translation.Value * 0.1f;

                }

                // ボスの現在地がX,Yともに次の移動地点より小さい場合
                // @@if (moveVolume.x == 0 && moveVolume.y == 0)@@
                {
                    // 動かす
                    translation.Value += moveVolume * deltaTime;
                }
                 

            });// 分散並列スレッド処理.ScheduleParallel()

        _moveInterval -= Time.DeltaTime;
    }
}
