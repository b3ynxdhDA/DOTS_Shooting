using Unity.Entities;

public class BossPhaseSytem : SystemBase
{
    // 変数宣言------------------------------------------------------------------
    // 実行タイミングを管理しているシステムグループ
    private BeginSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

    // 第2フェーズのボスのHP
    private float _2ndPhaseHP;

    // 第3フェーズのボスのHP
    private float _3rdPhaseHP;

    // 定数宣言--------------------------------------------------------



    protected override void OnCreate()
    {
        // EntityCommandBufferの取得
        _entityCommandBufferSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();

        // ボスキャラクターのHPを初期化
        Entities
            .WithName("Boss_HP_Initialize")
            .WithAll<EnemyTag,BossEnemyTag>()
            .ForEach((ref EnemyTag enemyTag, in BossEnemyTag bossEnemyTag) =>
            {
                enemyTag._enemyHp = bossEnemyTag._1st_bossHp;
                _2ndPhaseHP = bossEnemyTag._2nd_bossHp;
                _3rdPhaseHP = bossEnemyTag._3rd_bossHp;
            }).Run();

        
    }

    protected override void OnUpdate()
    {
        GameManager gameManager = GameManager.instance;
        BossManager bossManager = gameManager.BossManager;
        // コマンドバッファを取得
        EntityCommandBuffer comandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

        Entities
            .WithName("Boss_Phase")
            .WithAll<EnemyTag, BossEnemyTag>()
            .ForEach((Entity entity, ref EnemyTag enemyTag) =>
            {
                // ゲームのステートがゲーム中以外なら処理しない
                if (gameManager.game_State != GameManager.GameState.GameNow)
                {
                    return;
                }

                switch (bossManager.BossPhaseCount)
                {
                    case 0:
                        Phase0(ref enemyTag);
                        break;
                }

            }).Run();

        // 指定したJob完了後にECBに登録した命令を実行
        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }

    private void Phase0(ref EnemyTag enemyTag)
    {
        // ボスのHPが0より小さくなったら
        if(enemyTag._enemyHp < 0)
        {

        }
    }
}
