using Unity.Entities;

public class NormalEnemySytem : SystemBase
{
    // 変数宣言------------------------------------------------------------------
    // 実行タイミングを管理しているシステムグループ
    private BeginSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

    private bool _isNormalEnemyInitialize = false;
    /// <summary>
    /// システム作成時に呼ばれる処理
    /// </summary>
    protected override void OnCreate()
    {
        // EntityCommandBufferの取得
        _entityCommandBufferSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
    }

    /// <summary>
    /// システム有効時にフレーム毎に呼ばれる処理
    /// </summary>
    protected override void OnUpdate()
    {
        // 各Managerを取得
        GameManager gameManager = GameManager.instance;
        NormalEnemyManager normalEnemyManager = gameManager.NormalEnemyManager;

        // コマンドバッファを取得
        EntityCommandBuffer commandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

        Entities
            .WithName("Enemy")
            .WithAll<EnemyTag>()
            .WithNone<BossEnemyTag>()
            .WithoutBurst()
            .ForEach((Entity entity, ref HPTag hpTag, ref GunPortTag gunPortTag) =>
            {
                // @一回しか実行しないと一体しか初期化されない
                // フィールドやOnCreateではManagerが取得できなかったのでOnUpdateで初期化
                if (!_isNormalEnemyInitialize)
                {
                    //gameManager.KomaManager.SetKomaDate(entity, ref hpTag, ref gunPortTag, normalEnemyManager.NormalEnemyKomaData, commandBuffer);
                    _isNormalEnemyInitialize = true;
                }
            }).Run();

        // 指定したJob完了後にECBに登録した命令を実行
        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}