using Unity.Entities;
using UnityEngine;

namespace shooting
{
    public class BossPhaseSytem : SystemBase
    {
        // 変数宣言------------------------------------------------------------------
        // 実行タイミングを管理しているシステムグループ
        private BeginSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

        // ゲームマネージャーの参照
        private GameManager _gameManager = GameManager.instance;

        // ボスマネージャーの参照
        private BossManager _bossManager;

        // 定数宣言--------------------------------------------------------



        protected override void OnCreate()
        {
            // EntityCommandBufferの取得
            _entityCommandBufferSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();

            _bossManager = _gameManager.BossManager;

            // ボスキャラクターのHPを初期化
            SetKomaDate(_bossManager.BossKomaData0);


        }

        protected override void OnUpdate()
        {
            // ゲームのステートがゲーム中以外なら処理しない
            if (_gameManager.gameState != GameManager.GameState.GameNow)
            {
                return;
            }

            Entities
                .WithName("Boss_Phase")
                .WithAll<EnemyTag, BossEnemyTag>()
                .WithoutBurst()
                .ForEach((ref EnemyTag enemyTag) =>
                {

                // 現在のボスの攻撃段階に合わせた処理をする
                switch (_bossManager.BossPhaseCount)
                    {
                    // 第1段階
                    case 0:
                        // ボスのHPが0より小さくなったら
                        if (enemyTag._enemyHp < 0)
                            {
                            // @次の駒をセットする
                            SetKomaDate(_bossManager.BossKomaData1);

                            // ボスの攻撃段階を上げる
                            _bossManager.UpdateBossCount();
                            }
                            break;
                    // 第2段階
                    case 1:
                        // ボスのHPが0より小さくなったら
                        if (enemyTag._enemyHp < 0)
                            {
                            // @次の駒をセットする
                            SetKomaDate(_bossManager.BossKomaData2);

                            // ボスの攻撃段階を上げる
                            _bossManager.UpdateBossCount();
                            }
                            break;
                    // 第3段階
                    case 2:
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
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            switch (komaData.shootKind)
            {
                case KomaData.ShootKind.StraightGunPortTag:
                    entityManager.AddComponentData(entity, new StraightGunPortTag
                    {
                        _lines = komaData.shootLine
                    });
                    break;
                case KomaData.ShootKind.WideGunPortTag:
                    entityManager.AddComponentData(entity, new WideGunPortTag
                    {
                        _lines = komaData.shootLine
                    });
                    break;
                case KomaData.ShootKind.AimGunPortTag:
                    entityManager.AddComponentData(entity, new AimGunPortTag { });
                    break;
            }

        }

    }
}