using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using System;

/// <summary>
/// プレイヤーの入力を受け取るシステム
/// </summary>
public class PlayerInputSystem : SystemBase
{
    /// <summary>
    /// システム有効時にフレーム毎に呼ばれる処理
    /// </summary>
    protected override void OnUpdate()
    {
        // 直前のフレームからの経過時間
        float deltaTime = Time.DeltaTime;
        // 垂直方向の入力
        float inputVertical = Input.GetAxis("Vertical");
        // 水平方向の入力
        float inputHorizontal = Input.GetAxis("Horizontal");

        Entities
            .WithAll<PlayerTag>()
            .ForEach((ref Translation translation, in PlayerTag playerTag) =>
            {
                translation.Value.x += inputHorizontal * deltaTime * playerTag._moveSpeed;
                translation.Value.y += inputVertical * deltaTime * playerTag._moveSpeed;

            }).Run();
    }
}
