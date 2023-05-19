using Unity.Collections;
using Unity.Entities;

public class NormalEnemySytem : SystemBase
{
    // 変数宣言------------------------------------------------------------------
    // エンティティマネージャーを取得
    private EntityManager entityManager;

    // 実行タイミングを管理しているシステムグループ
    private BeginSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

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
        //　デフォルトのワールドのEntityManagerの取得
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

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
                if (_isLoopEntity)
                {
                    return;
                }

                // 全てのエンティティで一度ずつ処理をしたか
                if(_firstIndex == entity.Index && !_isLoopEntity)
                {
                    _isLoopEntity = true;
                }

                // フィールドやOnCreateではManagerが取得できなかったのでOnUpdateで初期化
                if (!_isNormalEnemyInitialize)
                {
                    _isNormalEnemyInitialize = true;
                    _firstIndex = entity.Index;
                }

                // 雑魚敵の駒データを設定する
                gameManager.KomaManager.SetKomaDate(entity, ref hpTag, ref gunPortTag, normalEnemyManager.NormalEnemyKomaData, commandBuffer);
                    
            }).Run();

        // 指定したJob完了後にECBに登録した命令を実行
        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}