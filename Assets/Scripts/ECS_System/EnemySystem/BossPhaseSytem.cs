using Unity.Entities;
using Unity.Rendering;

public class BossPhaseSytem : SystemBase
{
    // 変数宣言------------------------------------------------------------------
    // 実行タイミングを管理しているシステムグループ
    private BeginSimulationEntityCommandBufferSystem _entityCommandBufferSystem;


    // 定数宣言--------------------------------------------------------



    protected override void OnCreate()
    {
        // EntityCommandBufferの取得
        _entityCommandBufferSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        // 各Managerを取得
        GameManager gameManager = GameManager.instance;
        BossManager bossManager = gameManager.BossManager;


        // ゲームのステートがゲーム中以外なら処理しない
        if (gameManager.gameState != GameManager.GameState.GameNow)
        {
            return;
        }

        Entities
            .WithName("Boss_Phase")
            .WithAll<EnemyTag, BossEnemyTag>()
            .WithoutBurst()
            .ForEach((Entity entity, ref HPTag hpTag, ref GunPortTag gunPortTag) =>
            {
        // フィールドやOnCreateではManagerが取得できなかったのでOnUpdateで初期化
        if (!bossManager.IsBossKomaInitialize)
        {
            SetBossKomaDate(entity, ref hpTag, ref gunPortTag, bossManager.BossKomaData1);
            bossManager.IsBossKomaInitialize = true;
        }
                // 現在のボスの攻撃段階に合わせた処理をする
                switch (bossManager.BossPhaseCount)
                {
                    // 第1段階
                    case 1:
                        // ボスのHPが0より小さくなったら
                        if (hpTag._hp < 0)
                        {
                            // @次の駒をセットする
                            SetBossKomaDate(entity, ref hpTag, ref gunPortTag, bossManager.BossKomaData2);

                            // ボスの攻撃段階を上げる
                            bossManager.UpdateBossCount();
                        }
                        break;
                    // 第2段階
                    case 2:
                        // ボスのHPが0より小さくなったら
                        if (hpTag._hp < 0)
                        {
                            // @次の駒をセットする
                            SetBossKomaDate(entity, ref hpTag, ref gunPortTag, bossManager.BossKomaData3);

                            // ボスの攻撃段階を上げる
                            bossManager.UpdateBossCount();
                        }
                        break;
                    // 第3段階
                    case 3:
                        // ボスのHPが0より小さくなったら
                        if (hpTag._hp < 0)
                        {
                            gameManager.UIManager.CallGameFinish(true);
                        }
                        break;
                }

            }).Run();

        // 指定したJob完了後にECBに登録した命令を実行
        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }

    /// <summary>
    /// ボスの駒のステータスを設定する
    /// </summary>
    /// <param name="komaData"></param>
    private void SetBossKomaDate(Entity entity, ref HPTag hpTag, ref GunPortTag gunPortTag, KomaData komaData)
    {
        // コマンドバッファを取得
        EntityCommandBuffer commandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

        // 
        hpTag._hp = komaData.hp;
        gunPortTag._shootCoolTime = komaData.shootCoolTime;
        gunPortTag._bulletSpeed = komaData.bulletSpeed;

        // マテリアルを変更
        commandBuffer.SetSharedComponent(entity, new RenderMesh
        {
            mesh = GameManager.instance.Quad,
            material = komaData.material
        });

        // 射撃の種類のコンポーネントを設定する
        // 次にセットするGunPortの種類は何か
        switch (komaData.shootKind)
        {
            case KomaData.ShootKind.StraightGunPortTag:

                // 既にあるGunPortの種別タグを削除する
                commandBuffer.RemoveComponent<WideGunPortTag>(entity);
                commandBuffer.RemoveComponent<AimGunPortTag>(entity);

                // StraightGunPortTagを追加する
                commandBuffer.AddComponent(entity, new StraightGunPortTag
                {
                    _lines = komaData.shootLine
                });
                break;
            case KomaData.ShootKind.WideGunPortTag:

                // 既にあるGunPortの種別タグを削除する
                commandBuffer.RemoveComponent<StraightGunPortTag>(entity);
                commandBuffer.RemoveComponent<AimGunPortTag>(entity);

                // WideGunPortTagを追加する
                commandBuffer.AddComponent(entity, new WideGunPortTag
                {
                    _lines = komaData.shootLine
                });
                break;
            case KomaData.ShootKind.AimGunPortTag:

                // 既にあるGunPortの種別タグを削除する
                commandBuffer.RemoveComponent<StraightGunPortTag>(entity);
                commandBuffer.RemoveComponent<WideGunPortTag>(entity);

                // AimGunPortTagを追加する
                commandBuffer.AddComponent(entity, new AimGunPortTag { });
                break;
        }
    }
}