using Unity.Entities;

/// <summary>
/// プレイヤーの状態をみるシステム
/// </summary>
public class PlayerStateSystem : SystemBase
{
    // 変数宣言------------------------------------------------------------------
    // 実行タイミングを管理しているシステムグループ
    private EndSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

    /// <summary>
    /// システム作成時に呼ばれる処理
    /// </summary>
    protected override void OnCreate()
    {
        // EntityCommandBufferの取得
        _entityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    /// <summary>
    /// システム有効時にフレーム毎に呼ばれる処理
    /// </summary>
    protected override void OnUpdate()
    {
        // 各Managerを取得
        GameManager gameManager = GameManager.instance;
        PlayerManager playerManager = gameManager.PlayerManager;

        // コマンドバッファを取得
        EntityCommandBuffer commandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

        Entities
            .WithName("Player_State")
            .WithAll<PlayerTag>()
            .WithoutBurst()
            .ForEach((Entity entity, ref GunPortTag gunPortTag, in HPTag hpTag) =>
            {
                // フィールドやOnCreateではManagerが取得できなかったのでOnUpdateで初期化
                if (!playerManager.IsPlayerInitialize)
                {
                    gameManager.KomaManager.SetKomaDate(entity, playerManager.PlayerKomaData, ref gunPortTag, commandBuffer);
                    playerManager.IsPlayerInitialize = true;
                }

                // プレイヤーのHPが0以下なら消す
                if (hpTag._hp <= 0)
                {
                    commandBuffer.DestroyEntity(entity);
                    gameManager.UIManager.CallGameFinish(false);
                }

            }).Run();// メインスレッド処理

        // 指定したJob完了後にECBに登録した命令を実行
        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}
