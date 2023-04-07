using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// プレイヤーの入力を受け取るシステム
/// </summary>
public class PlayerInputSystem : SystemBase
{
    // 変数宣言------------------------------------------------------------------

    // 定数宣言--------------------------------------------------------

    /// <summary>
    /// システム有効時にフレーム毎に呼ばれる処理
    /// </summary>
    protected override void OnUpdate()
    {
        // 垂直方向の入力
        float inputVertical = Input.GetAxis("Vertical");
        // 水平方向の入力
        float inputHorizontal = Input.GetAxis("Horizontal");

        Entities
            .WithName("Player_Input")
            .WithAll<PlayerTag>()
            .ForEach((ref PlayerMoveDate playerMove) =>
            {
                // プレイヤーの移動
                playerMove._moveDirection.x = inputHorizontal;
                playerMove._moveDirection.y = inputVertical;

            }).Run();// メインスレッドで処理
    }
}
