using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

/// <summary>
/// 雑魚敵を生成するSystem
/// </summary>
public class EnemySpawnSytem : SystemBase
{
    // 変数宣言------------------------------------------------------------------
    // 実行タイミングを管理しているシステムグループ
    private EntityCommandBufferSystem _entityCommandBufferSystem;

    // 敵の出現時間間隔
    private float _spawnCoolTime = 0;

    // 定数宣言------------------------------------------------------------------
    // 敵の出現間隔の最大
    const float _SPAWN_TIME_MAX = 5;

    // 敵の出現間隔の最小
    const float _SPAWN_TIME_MIN = 3;

    // 敵の出現位置のXの最大
    const float _SPAWN_POS_X_MAX = 8.6f;

    // 敵の出現位置のXの最小
    const float _SPAWN_POS_X_MIN = -8.6f;

    // 敵の出現位置のYの最大
    const float _SPAWN_POS_Y_MAX = 4.1f;

    // 敵の出現位置のYの最小
    const float _SPAWN_POS_Y_MIN = -1.1f;

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
        // ゲームのステートがゲーム中以外なら処理しない
        if (GameManager.instance.gameState != GameManager.GameState.GameNow)
        {
            return;
        }

        // コマンドバッファを取得
        EntityCommandBuffer commandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

        // 各Managerを取得
        GameManager gameManager = GameManager.instance;
        NormalEnemyManager normalEnemyManager = gameManager.NormalEnemyManager;

        if (_spawnCoolTime < 0)
        {
            float spawnPosX = UnityEngine.Random.Range(_SPAWN_POS_X_MIN, _SPAWN_POS_X_MAX);
            float spawnPosY = UnityEngine.Random.Range(_SPAWN_POS_Y_MIN, _SPAWN_POS_Y_MAX);
            Entities
                .WithName("EnemySpawn")
                .WithAll<Spawner>()
                .WithBurst()
                .ForEach((Entity entity, in SpawnerData spawnerData) =>
                {
                // エンティティを生成
                Entity newEntity = commandBuffer.Instantiate(spawnerData.SpawnPrefabEntity);

                    commandBuffer.SetComponent(newEntity, new Translation
                    {
                        Value = new float3(spawnPosX, spawnPosY, 0f)// @ここがWithoutBurst()とRun()が必要
                });

                }).Schedule();

            _spawnCoolTime = UnityEngine.Random.Range(_SPAWN_TIME_MIN, _SPAWN_TIME_MAX);
        }

        _spawnCoolTime -= Time.DeltaTime;

        Entities
            .WithName("EnemyInitialization")
            .WithAll<EnemyTag, PlaneEnemyTag>()
            .WithNone<BossEnemyTag>()
            .WithoutBurst()
            .ForEach((Entity entity, ref GunPortTag gunPortTag) =>
            {

                // 駒データを配列から使用する駒をランダムに決める
                KomaData nowKomaData = normalEnemyManager.NormalEnemyKomaData[UnityEngine.Random.Range(0, normalEnemyManager.NormalEnemyKomaData.Length)];

                // 雑魚敵の駒データを設定する
                gameManager.KomaManager.SetKomaDate(entity, nowKomaData, ref gunPortTag, commandBuffer);

                // 初期化していない敵についているコンポーネントタグを削除
                commandBuffer.RemoveComponent<PlaneEnemyTag>(entity);

            }).Run();

        // 指定したJob完了後にECBに登録した命令を実行
        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}