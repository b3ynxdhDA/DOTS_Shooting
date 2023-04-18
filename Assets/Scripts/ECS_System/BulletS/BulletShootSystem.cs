using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// 弾を生成するシステム
/// </summary>
public class BulletShootSystem : SystemBase
{
    // 変数宣言------------------------------------------------------------------
    // 実行タイミングを管理しているシステムグループ
    private BeginSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

    // 定数宣言--------------------------------------------------------
    // 射撃する列の間隔
    const float _SHOOT_SPACE = 0.5f;

    // 射撃する角度の間隔
    const float _SHOOT_RAD = 1.5f;
    // 円周率
    const float _PI = math.PI;

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
                        Entity instantiateEntity = comandBuffer.Instantiate(gunporttag._straightBulletEntity);

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

        Entities
            .WithName("Wide_Shoot")
            .WithAll<GunPortTag, WideGunPortTag>()
            .WithoutBurst()
            .ForEach((ref GunPortTag gunporttag, in WideGunPortTag WideTag, in LocalToWorld localToWorld) =>
            {
                // クールタイムが0より小さかったら弾を発射する
                if (gunporttag._shootInterval < 0)
                {
                    // 発射する弾の列の数だけループ
                    for (int i = 0; i < WideTag._lines; i++)
                    {
                        // PrefabとなるEntityから弾を複製する
                        Entity instantiateEntity = comandBuffer.Instantiate(gunporttag._straightBulletEntity);

                        // 発射地点のローカル座標
                        float3 localPos = localToWorld.Position;

                        // 発射角度     弧度法:(_PI / 12) = 度数法:15度、弧度法:(_PI / 24) = 度数法:7.5度
                        float angle = i * (_PI / 12) - WideTag._lines * (_PI / 24) + (_PI / 24);

                        // 発射角を求めるための座標
                        float3 position = new float3(localPos.x + _SHOOT_RAD * math.cos(angle), localPos.y + _SHOOT_RAD * math.sin(angle), localPos.z);
                        
                        // 発射源からみた座標の向き
                        float3 diff = math.normalizesafe(position - localToWorld.Position);

                        // 位置の初期化
                        comandBuffer.SetComponent(instantiateEntity, new Translation
                        {
                            Value = localToWorld.Position
                        });

                        // 弾の向きを初期化
                        comandBuffer.SetComponent(instantiateEntity, new Rotation
                        {
                            Value = quaternion.LookRotationSafe(diff, math.up())
                        });

                        // 弾の進む角度を設定
                        comandBuffer.SetComponent(instantiateEntity, new BulletMoveDirectionTag
                        {
                            // 横向きに進んでいたのでdiffのxとyを逆にして、yに駒ごとのlocalToWorldのyを掛けて向きを調整
                            _moveDirection = new float3(diff.y, diff.x * math.sign(localToWorld.Up.y), diff.z)
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

        Entities
            .WithName("Aim_Shoot")
            .WithAll<GunPortTag, AimGunPortTag>()
            .WithoutBurst()
            .ForEach((ref GunPortTag gunporttag, in AimGunPortTag aimgunporttag, in LocalToWorld localToWorld) =>
            {
                // クールタイムが0より小さかったら弾を発射する
                if (gunporttag._shootInterval < 0)
                {
                    // 発射台から見たターゲットの向きを上向きにして初期化
                    float3 direction = localToWorld.Up;

                    // ターゲットのエンティティが存在するとき
                    if (aimgunporttag._targetEntity != Entity.Null)
                    {
                        // ターゲットのローカル座標を取得
                        ComponentDataFromEntity<Translation> translationComponentData = GetComponentDataFromEntity<Translation>(true);
                        float3 targetPosition = translationComponentData[aimgunporttag._targetEntity].Value;

                        // 発射台から見たターゲットの向きを計算
                        direction = math.normalizesafe(targetPosition - localToWorld.Position);
                    }
                    // PrefabのEntityから弾を複製する
                    Entity instantiateEntity = comandBuffer.Instantiate(gunporttag._straightBulletEntity);

                    // 位置の初期化
                    comandBuffer.SetComponent(instantiateEntity, new Translation
                    {
                        Value = localToWorld.Position
                    });

                    // 弾の向きを初期化
                    comandBuffer.SetComponent(instantiateEntity, new Rotation
                    {
                        Value = quaternion.LookRotationSafe(direction, math.forward())
                    });

                    // 弾の進む角度を設定
                    comandBuffer.SetComponent(instantiateEntity, new BulletMoveDirectionTag
                    {
                        _moveDirection = direction
                    });

                    // 弾の進む速度を設定
                    comandBuffer.SetComponent(instantiateEntity, new BulletTag
                    {
                        _bulletSpeed = gunporttag._bulletSpeed
                    });
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
