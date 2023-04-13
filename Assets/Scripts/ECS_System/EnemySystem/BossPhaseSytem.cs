using Unity.Entities;

public class BossPhaseSytem : SystemBase
{
    // 変数宣言------------------------------------------------------------------
    // 実行タイミングを管理しているシステムグループ
    private BeginSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

    // 第1段階の駒が設定されているか
    private bool _isKomaInitialize　= false;

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

        // フィールドやOnCreateではManagerが取得できなかったのでOnUpdateで初期化
        if (!_isKomaInitialize)
        {
            SetKomaDate(bossManager.BossKomaData1);
            _isKomaInitialize = true;
        }

        // ゲームのステートがゲーム中以外なら処理しない
        if (gameManager.gameState != GameManager.GameState.GameNow)
        {
            //@ return;
        }

        Entities
            .WithName("Boss_Phase")
            .WithAll<EnemyTag, BossEnemyTag>()
            .WithoutBurst()
            .ForEach((in EnemyTag enemyTag) =>
            {
                // 現在のボスの攻撃段階に合わせた処理をする
                switch (bossManager.BossPhaseCount)
                {
                    // 第1段階
                    case 1:
                        // ボスのHPが0より小さくなったら
                        if (enemyTag._enemyHp < 0)
                        {
                            // @次の駒をセットする
                            SetKomaDate(bossManager.BossKomaData2);

                            // ボスの攻撃段階を上げる
                            bossManager.UpdateBossCount();
                        }
                        break;
                    // 第2段階
                    case 2:
                        // ボスのHPが0より小さくなったら
                        if (enemyTag._enemyHp < 0)
                        {
                            // @次の駒をセットする
                            SetKomaDate(bossManager.BossKomaData3);

                            // ボスの攻撃段階を上げる
                            bossManager.UpdateBossCount();
                        }
                        break;
                    // 第3段階
                    case 3:
                        // ボスのHPが0より小さくなったら
                        if (enemyTag._enemyHp < 0)
                        {

                        }
                        break;
                }

            }).Run();

        // 指定したJob完了後にECBに登録した命令を実行
        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }

    /// <summary>
    /// 駒のステータスを設定する
    /// </summary>
    /// <param name="komaData"></param>
    private void SetKomaDate(KomaData komaData)
    {
        Entities
           .WithName("Set_Boss_KomaDate")
               .WithAll<EnemyTag, BossEnemyTag>()
               .WithoutBurst()
               .ForEach((Entity entity, ref EnemyTag enemyTag, ref GunPortTag gunPortTag) =>
               {
                   enemyTag._enemyHp = komaData.hp;
                   gunPortTag._shootCoolTime = komaData.shootCoolTime;
                   gunPortTag._bulletSpeed = komaData.bulletSpeed;
                   
                   SetShootKInd(entity, komaData);

               }).Run();
    }

    private void SetShootKInd(Entity entity, KomaData komaData)
    {
        // コマンドバッファを取得
        EntityCommandBuffer comandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

        // 次にセットするGunPortの種類は何か
        switch (komaData.shootKind)
        {
            case KomaData.ShootKind.StraightGunPortTag:

                // 既にあるGunPortの種別タグを削除する
                comandBuffer.RemoveComponent<WideGunPortTag>(entity);
                comandBuffer.RemoveComponent<AimGunPortTag>(entity);

                // StraightGunPortTagを追加する
                comandBuffer.AddComponent(entity, new StraightGunPortTag
                {
                    _lines = komaData.shootLine
                });
                break;
            case KomaData.ShootKind.WideGunPortTag:

                // 既にあるGunPortの種別タグを削除する
                comandBuffer.RemoveComponent<StraightGunPortTag>(entity);
                comandBuffer.RemoveComponent<AimGunPortTag>(entity);

                // WideGunPortTagを追加する
                comandBuffer.AddComponent(entity, new WideGunPortTag
                {
                    _lines = komaData.shootLine
                });
                break;
            case KomaData.ShootKind.AimGunPortTag:

                // 既にあるGunPortの種別タグを削除する
                comandBuffer.RemoveComponent<StraightGunPortTag>(entity);
                comandBuffer.RemoveComponent<WideGunPortTag>(entity);

                // AimGunPortTagを追加する
                comandBuffer.AddComponent(entity, new AimGunPortTag { });
                break;
        }

    }

}