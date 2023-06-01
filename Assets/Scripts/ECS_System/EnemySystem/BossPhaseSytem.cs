using Unity.Entities;

public class BossPhaseSytem : SystemBase
{
    // 変数宣言------------------------------------------------------------------
    // 実行タイミングを管理しているシステムグループ
    private BeginSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

    // 定数宣言------------------------------------------------------------------
    // ボスの段階が変化するHP
    private const int _CHANGE_PHASE_HP = 1;

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
        BossManager bossManager = gameManager.BossManager;

        // コマンドバッファを取得
        EntityCommandBuffer commandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

        Entities
            .WithName("Boss_Phase")
            .WithAll<EnemyTag, BossEnemyTag>()
            .WithoutBurst()
            .ForEach((Entity entity, ref GunPortTag gunPortTag, in HPTag hpTag) =>
            {
                // フィールドやOnCreateではManagerが取得できなかったのでOnUpdateで初期化
                if (!bossManager.IsBossInitialize)
                {
                    // 第一段階の駒をセットする
                    gameManager.KomaManager.SetKomaDate(entity, bossManager.BossKomaData1, gunPortTag, commandBuffer);

                    // HPバーを設定
                    gameManager.UIManager.SetSliderBossHP(bossManager.BossKomaData1);

                    bossManager.IsBossInitialize = true;
                }

                // 現在のボスの攻撃段階に合わせた処理をする
                switch (bossManager.BossPhaseCount)
                {
                    // 第1段階
                    case 1:
                        // ボスのHPが0より小さくなったら
                        if (hpTag._hp < _CHANGE_PHASE_HP)
                        {
                            // 次の駒をセットする
                            gameManager.KomaManager.SetKomaDate(entity, bossManager.BossKomaData2, gunPortTag, commandBuffer);

                            // HPバーを再設定
                            gameManager.UIManager.SetSliderBossHP(bossManager.BossKomaData2);

                            // ボスの攻撃段階を上げる
                            bossManager.UpdateBossCount();
                        }
                        break;
                    // 第2段階
                    case 2:
                        // ボスのHPが0より小さくなったら
                        if (hpTag._hp < _CHANGE_PHASE_HP)
                        {
                            // 次の駒をセットする
                            gameManager.KomaManager.SetKomaDate(entity, bossManager.BossKomaData3, gunPortTag, commandBuffer);

                            // HPバーを再設定
                            gameManager.UIManager.SetSliderBossHP(bossManager.BossKomaData3);

                            // ボスの攻撃段階を上げる
                            bossManager.UpdateBossCount();
                        }
                        break;
                    // 第3段階
                    case 3:
                        // ボスのHPが0より小さくなったら
                        if (hpTag._hp < _CHANGE_PHASE_HP)
                        {
                            gameManager.UIManager.CallGameFinish(true);
                        }
                        break;
                }

                // 参照できるようにBossManagerにHPを渡す
                bossManager.GetBossHP = hpTag._hp;

            }).Run();

        // 指定したJob完了後にECBに登録した命令を実行
        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}