using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// 回転する弾を生成するシステム
/// </summary>
[AlwaysUpdateSystem]
public class SpinShootSystem : SystemBase
{
    // 変数宣言------------------------------------------------------------------
    // 実行タイミングを管理しているシステムグループ
    private BeginSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

    private int _spinRad = 1;

    // 定数宣言--------------------------------------------------------
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
        // 直前のフレームからの経過時間
        float deltaTime = Time.DeltaTime;

        Entities
            .WithName("Spin_Shoot")
            .WithoutBurst()
            .WithAll<GunPortTag, SpinGunPortTag>()
            .ForEach((ref GunPortTag gunPortTag, in SpinGunPortTag spinGunPortTag, in LocalToWorld localToWorld) =>
            {
                // 回転軸の法線を計算する
                //quaternion normalizedRotation = math.normalizesafe(rotation.Value);
                // 回転させる角度を計算する
                //quaternion angleToRotate = quaternion.AxisAngle(math.forward(), spinGunPortTag._spinSpeed * deltaTime);

                //rotation.Value = math.mul(normalizedRotation, angleToRotate);
                if (_spinRad >= int.MaxValue)
                {
                    _spinRad = 0;
                    
                }

                // ゲームのステートがゲーム中以外なら処理しない
                if (GameManager.instance.gameState != GameManager.GameState.GameNow)
                {
                    return;
                }

                // コマンドバッファを取得
                EntityCommandBuffer comandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

                // クールタイムが0より小さかったら弾を発射する
                if (gunPortTag._shootInterval < 0)
                {
                    // PrefabとなるEntityから弾を複製する
                    Entity instantiateEntity = comandBuffer.Instantiate(gunPortTag._BulletEntity);

                    // 発射地点のローカル座標
                    float3 localPos = localToWorld.Position;

                    // 発射角度     弧度法:(_PI / 12) = 度数法:15度、弧度法:(_PI / 24) = 度数法:7.5度
                    float angle = _spinRad * (_PI / 12) * (_PI / 24) + (_PI / 24);

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
                        _bulletSpeed = gunPortTag._bulletSpeed
                    });

                    // 発射角をずらす
                    _spinRad += spinGunPortTag._spinSpeed;

                    // インターバルをセットする
                    gunPortTag._shootInterval = gunPortTag._shootCoolTime;
                }
                // インターバルから経過時間を減らす
                gunPortTag._shootInterval -= Time.DeltaTime;

            }).Run();// メインスレッドで実行

    }
}
