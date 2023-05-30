using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;

/// <summary>
/// 雑魚敵を生成するSystem
/// </summary>
public class EnemySpawnSytem : SystemBase
{
    // 変数宣言------------------------------------------------------------------

    // 実行タイミングを管理しているシステムグループ
    private EntityCommandBufferSystem _entityCommandBufferSystem;

    // 一回目の処理が終わったかどうか
    private bool _isNormalEnemyInitialize = false;

    // 全てのエンティティで処理をしたか
    private bool _isLoopEntity = false;

    // 最初に処理されるエンティティのインデックス
    private int _firstIndex = 0;

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
        // コマンドバッファを取得
        EntityCommandBuffer concurrent = _entityCommandBufferSystem.CreateCommandBuffer();

        // 各Managerを取得
        GameManager gameManager = GameManager.instance;
        NormalEnemyManager normalEnemyManager = gameManager.NormalEnemyManager;

        Entities
            .WithName("EnemySpawn")
            .WithAll<Spawner, SpawnTag>()
            .WithoutBurst()
            .ForEach((Entity entity,in SpawnerData spawnerData) =>
            {
                // エンティティを生成
                var createEntity = concurrent.Instantiate(spawnerData.SpawnPrefabEntity);

                // 駒データを配列から使用する駒をランダムに決める
                KomaData nowKomaData = normalEnemyManager.NormalEnemyKomaData[UnityEngine.Random.Range(0, normalEnemyManager.NormalEnemyKomaData.Length)];

                // 雑魚敵の駒データを設定する
                gameManager.KomaManager.SetKomaDate(createEntity, nowKomaData, concurrent);

            }).Run();

        // 指定したJob完了後にECBに登録した命令を実行
        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}