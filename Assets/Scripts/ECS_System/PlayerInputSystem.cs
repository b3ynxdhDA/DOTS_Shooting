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
    // 変数宣言------------------------------------------------------------------
    private Camera _mainCamera;

    // スクリーンの左下のワールド座標の
    private Vector2 _lowerLeft;

    // スクリーンの右上のワールド座標
    private Vector2 _upperRight;

    // 定数宣言--------------------------------------------------------
    // スクリーン座標の左下
    private readonly Vector2 _MIN_SCREEN_POINT = new Vector2(0.1f, 0.1f);
    // スクリーン座標の右上
    private readonly Vector2 _MAX_SCREEN_POINT = new Vector2(0.9f, 0.9f);

    /// <summary>
    /// システム実行停止時やOnDestroy()の前に呼ばれる処理
    /// カメラの切り替えに対応するためここでカメラを取得
    /// </summary>
    protected override void OnStartRunning()
    {
        // スクリーン座標をワールド座標に変換する
        _mainCamera = Camera.main;
        _lowerLeft = _mainCamera.ViewportToWorldPoint(_MIN_SCREEN_POINT);
        _upperRight = _mainCamera.ViewportToWorldPoint(_MAX_SCREEN_POINT);
    }
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
            .WithName("Player_Input")
            .WithAll<PlayerTag>()
            .ForEach((ref Translation translation, in PlayerTag playerTag) =>
            {
                // プレイヤーの移動
                translation.Value.x += inputHorizontal * deltaTime * playerTag._moveSpeed;
                translation.Value.y += inputVertical * deltaTime * playerTag._moveSpeed;

                // プレイヤーが画面外に出ないように制限する
                translation.Value.x = Mathf.Clamp(translation.Value.x, _lowerLeft.x, _upperRight.x);
                translation.Value.y = Mathf.Clamp(translation.Value.y, _lowerLeft.y, _upperRight.y);

            }).WithoutBurst().Run();// メインスレッドで処理
    }
}
