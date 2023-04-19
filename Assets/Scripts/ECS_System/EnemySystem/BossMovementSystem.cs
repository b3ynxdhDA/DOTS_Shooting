using UnityEngine;
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

    // 次の移動地点
    float3 nextPoint;

    // 前の移動地点
    float3 lastPoint;

    // 移動する向き
    float3 moveDirection;

    // 定数宣言------------------------------------------------------------------
    // 移動インターバルの最大
    const float _INTERVAL_MAX = 8f;

    // 移動インターバルの最小
    const float _INTERVAL_MIN = 3f;

    /// <summary>
    /// システム有効時にフレーム毎に呼ばれる処理
    /// </summary>
    protected override void OnUpdate()
    {
        // ゲームのステートがゲーム中以外なら処理しない
        if (GameManager.instance.gameState != GameManager.GameState.GameNow)
        {
            return;
        }
        // 直前のフレームからの経過時間
        float deltaTime = Time.DeltaTime;
        Entities
            .WithAll<BossEnemyTag>()
            .ForEach((ref Translation translation, DynamicBuffer<BossMoveElement> dynamicBuffer) =>
            {
                if (_moveInterval < 0)
                {
                    // タイマーを初期化
                    _moveInterval = UnityEngine.Random.Range(_INTERVAL_MIN, _INTERVAL_MAX);

                    // ランダムに次の移動地点の引数を設定
                    int _nextMovePointNum = UnityEngine.Random.Range(0, dynamicBuffer.Length);

                    // 次の移動地点の座標を取得
                    BossMoveElement bossMoveElement = dynamicBuffer[_nextMovePointNum];
                    nextPoint = bossMoveElement._movePoints;

                    // 現在地を前の移動地点として記録
                    lastPoint = translation.Value;

                    // 移動する向きを計算
                    moveDirection = nextPoint - lastPoint;

                }

                // 前の移動地点から現在地への距離が移動地点同士の距離より小さい間
                if (math.distance(translation.Value, lastPoint) < math.distance(nextPoint, lastPoint))
                {
                    // 動かす
                    translation.Value += moveDirection * deltaTime;
                }

            });// 分散並列スレッド処理.ScheduleParallel()

        _moveInterval -= Time.DeltaTime;
    }
}
