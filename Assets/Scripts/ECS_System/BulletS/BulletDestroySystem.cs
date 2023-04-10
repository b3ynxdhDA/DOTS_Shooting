using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

/// <summary>
/// 画面外の弾を削除するシステム
/// </summary>
public class BulletDestroySystem : SystemBase
{
    // 変数宣言------------------------------------------------------------------
    // 実行タイミングを管理しているシステムグループ
    private EndSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

    private Camera _mainCamera;

    // スクリーンの左下のワールド座標の
    private Vector2 _lowerLeft;

    // スクリーンの右上のワールド座標
    private Vector2 _upperRight;

    // 定数宣言--------------------------------------------------------
    // スクリーン座標の左下
    private readonly Vector2 _MIN_SCREEN_POINT = new Vector2(-0.1f, -0.1f);
    // スクリーン座標の右上
    private readonly Vector2 _MAX_SCREEN_POINT = new Vector2(1.1f, 1.1f);

    /// <summary>
    /// システム作成時に呼ばれる処理
    /// </summary>
    protected override void OnCreate()
    {
        // EntityCommandBufferの取得
        _entityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

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
        // コマンドバッファを取得
        EntityCommandBuffer commandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

        Entities
            .WithName("Bullet_Destroy")
            .WithAll<BulletTag>()
            .WithBurst()
            .ForEach((Entity entity, in Translation translation) =>
            {
                // 弾のエンティティが画面外に出たら消す
                if (translation.Value.x > _upperRight.x || translation.Value.y > _upperRight.y ||
                   translation.Value.x < _lowerLeft.x || translation.Value.y < _lowerLeft.y)
                {
                    commandBuffer.DestroyEntity(entity);
                }

            }).WithoutBurst().Run();// メインスレッド処理

        // 指定したJob完了後にECBに登録した命令を実行
        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}
