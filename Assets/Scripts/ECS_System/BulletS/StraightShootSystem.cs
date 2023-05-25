using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// 弾を生成するシステム
/// </summary>
public class StraightShootSystem : SystemBase
{
    // 変数宣言------------------------------------------------------------------
    // 実行タイミングを管理しているシステムグループ
    private BeginSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

    // 定数宣言--------------------------------------------------------
    // 射撃する列の間隔
    const float _SHOOT_SPACE = 0.5f;

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
        // ゲームのステートがゲーム中以外なら処理しない
        if (GameManager.instance.gameState != GameManager.GameState.GameNow)
        {
            return;
        }

        // コマンドバッファを取得
        EntityCommandBuffer comandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

        Entities
            .WithName("Straight_Shoot")
            .WithAll<GunPortTag, StraightGunPortTag>()
            .WithoutBurst()
            .ForEach((ref GunPortTag gunporttag, in StraightGunPortTag straightTag, in LocalToWorld localToWorld) =>
            {
                // クールタイムが0より小さかったら弾を発射する
                if (gunporttag._shootInterval < 0)
                {
                    // 発射する弾の列の数だけループ
                    for (int i = 0; i < straightTag._lines; i++)
                    {
                        // PrefabとなるEntityから弾を複製する
                        Entity instantiateEntity = comandBuffer.Instantiate(gunporttag._BulletEntity);

                        // 発射の列ごとの位置を計算する
                        float3 pos = new float3(localToWorld.Position.x + i - _SHOOT_SPACE * straightTag._lines + _SHOOT_SPACE, localToWorld.Position.y, 0f);

                        // 位置の初期化
                        comandBuffer.SetComponent(instantiateEntity, new Translation
                        {
                            Value = pos
                        });

                        // 弾の向きを初期化
                        comandBuffer.SetComponent(instantiateEntity, new Rotation
                        {
                            Value = localToWorld.Rotation
                        });

                        // 弾の進む角度を設定
                        comandBuffer.SetComponent(instantiateEntity, new BulletMoveDirectionTag
                        {
                            _moveDirection = localToWorld.Up
                        });

                        // 弾の進む速度を設定
                        comandBuffer.SetComponent(instantiateEntity, new BulletTag
                        {
                            _bulletSpeed = gunporttag._bulletSpeed
                        });
                    }
                    // インターバルをセットする
                    gunporttag._shootInterval = gunporttag._shootCoolTime;
                }
                // インターバルから経過時間を減らす
                gunporttag._shootInterval -= Time.DeltaTime;

            }).Run();// メインスレッドで実行

        // 指定したJob完了後にECBに登録した命令を実行
        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}
