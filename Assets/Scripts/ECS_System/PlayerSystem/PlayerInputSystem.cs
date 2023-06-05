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
        // 低速モードの入力
        bool inputSlowButtonDown = Input.GetButtonDown("Slow");
        // 低速モードの入力
        bool inputSlowButtonUp = Input.GetButtonUp("Slow");

        Entities
            .WithName("Player_Input")
            .WithAll<PlayerTag>()
            .ForEach((ref PlayerMoveDate playerMove) =>
            {
                // プレイヤーの移動
                playerMove._moveDirection.x = inputHorizontal;
                playerMove._moveDirection.y = inputVertical;

                if (inputSlowButtonDown || inputSlowButtonUp)
                {
                    float tmp = playerMove._moveSpeed;

                    playerMove._moveSpeed = playerMove._slowSpeed;

                    playerMove._slowSpeed = tmp;
                }

            }).Run();// メインスレッドで処理
    }
}
