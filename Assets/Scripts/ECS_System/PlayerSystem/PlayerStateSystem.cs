using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

/// <summary>
/// プレイヤーの状態をみるシステム
/// </summary>
public class PlayerStateSystem : SystemBase
{
    // 変数宣言------------------------------------------------------------------
    // 実行タイミングを管理しているシステムグループ
    private EndSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

    // プレイヤーの駒が初期化されているか
    private bool _isKomaInitialize = false;

    // 定数宣言--------------------------------------------------------


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
            .ForEach((Entity entity, ref PlayerTag playerTag, ref GunPortTag gunPortTag) =>
            {
                    SetPlayerKomaDate(entity, playerTag, gunPortTag, playerManager.PlayerKomaData);
                // フィールドやOnCreateではManagerが取得できなかったのでOnUpdateで初期化
                if (!_isKomaInitialize)
                {
                    _isKomaInitialize = true;
                }

                // プレイヤーのHPが0以下なら消す
                if (playerTag._playerHp <= 0)
                {
                    commandBuffer.DestroyEntity(entity);
                    gameManager.UIManager.CallGameFinish(false);
                }

            }).Run();// メインスレッド処理

        // 指定したJob完了後にECBに登録した命令を実行
        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }

    /// <summary>
    /// プレイヤーの駒データをセットする
    /// </summary>
    private void SetPlayerKomaDate(Entity entity, PlayerTag playerTag, GunPortTag gunPortTag, KomaData komaData)
    {

        playerTag._playerHp = komaData.hp;
        gunPortTag._shootCoolTime = komaData.shootCoolTime;
        gunPortTag._bulletSpeed = komaData.bulletSpeed;

        SetShootKInd(entity, komaData);
    }

    /// <summary>
    /// 射撃の種類のコンポーネントを設定する
    /// </summary>
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
