using Unity.Entities;
using Unity.Rendering;

public class BossPhaseSytem : SystemBase
{
    // �ϐ��錾------------------------------------------------------------------
    // ���s�^�C�~���O���Ǘ����Ă���V�X�e���O���[�v
    private BeginSimulationEntityCommandBufferSystem _entityCommandBufferSystem;


    // �萔�錾--------------------------------------------------------



    protected override void OnCreate()
    {
        // EntityCommandBuffer�̎擾
        _entityCommandBufferSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        // �eManager���擾
        GameManager gameManager = GameManager.instance;
        BossManager bossManager = gameManager.BossManager;

        // �t�B�[���h��OnCreate�ł�Manager���擾�ł��Ȃ������̂�OnUpdate�ŏ�����
        if (!bossManager.IsBossKomaInitialize)
        {
            SetBossKomaDate(bossManager.BossKomaData1);
            bossManager.IsBossKomaInitialize = true;
        }

        // �Q�[���̃X�e�[�g���Q�[�����ȊO�Ȃ珈�����Ȃ�
        if (gameManager.gameState != GameManager.GameState.GameNow)
        {
            return;
        }

        Entities
            .WithName("Boss_Phase")
            .WithAll<EnemyTag, BossEnemyTag>()
            .WithoutBurst()
            .ForEach((in EnemyTag enemyTag) =>
            {
                // ���݂̃{�X�̍U���i�K�ɍ��킹������������
                switch (bossManager.BossPhaseCount)
                {
                    // ��1�i�K
                    case 1:
                        // �{�X��HP��0��菬�����Ȃ�����
                        if (enemyTag._enemyHp < 0)
                        {
                            // @���̋���Z�b�g����
                            SetBossKomaDate(bossManager.BossKomaData2);

                            // �{�X�̍U���i�K���グ��
                            bossManager.UpdateBossCount();
                        }
                        break;
                    // ��2�i�K
                    case 2:
                        // �{�X��HP��0��菬�����Ȃ�����
                        if (enemyTag._enemyHp < 0)
                        {
                            // @���̋���Z�b�g����
                            SetBossKomaDate(bossManager.BossKomaData3);

                            // �{�X�̍U���i�K���グ��
                            bossManager.UpdateBossCount();
                        }
                        break;
                    // ��3�i�K
                    case 3:
                        // �{�X��HP��0��菬�����Ȃ�����
                        if (enemyTag._enemyHp < 0)
                        {
                            gameManager.UIManager.CallGameFinish(true);
                        }
                        break;
                }

            }).Run();

        // �w�肵��Job�������ECB�ɓo�^�������߂����s
        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }

    /// <summary>
    /// �{�X�̋�̃X�e�[�^�X��ݒ肷��
    /// </summary>
    /// <param name="komaData"></param>
    private void SetBossKomaDate(KomaData komaData)
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

    /// <summary>
    /// �ˌ��̎�ނ̃R���|�[�l���g��ݒ肷��
    /// </summary>
    private void SetShootKInd(Entity entity, KomaData komaData)
    {
        // �R�}���h�o�b�t�@���擾
        EntityCommandBuffer commandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

        // �}�e���A����ύX
        commandBuffer.SetSharedComponent(entity, new RenderMesh
        {
            mesh = GameManager.instance.Quad,
            material = komaData.material
        });

        // ���ɃZ�b�g����GunPort�̎�ނ͉���
        switch (komaData.shootKind)
        {
            case KomaData.ShootKind.StraightGunPortTag:

                // ���ɂ���GunPort�̎�ʃ^�O���폜����
                commandBuffer.RemoveComponent<WideGunPortTag>(entity);
                commandBuffer.RemoveComponent<AimGunPortTag>(entity);

                // StraightGunPortTag��ǉ�����
                commandBuffer.AddComponent(entity, new StraightGunPortTag
                {
                    _lines = komaData.shootLine
                });
                break;
            case KomaData.ShootKind.WideGunPortTag:

                // ���ɂ���GunPort�̎�ʃ^�O���폜����
                commandBuffer.RemoveComponent<StraightGunPortTag>(entity);
                commandBuffer.RemoveComponent<AimGunPortTag>(entity);

                // WideGunPortTag��ǉ�����
                commandBuffer.AddComponent(entity, new WideGunPortTag
                {
                    _lines = komaData.shootLine
                });
                break;
            case KomaData.ShootKind.AimGunPortTag:

                // ���ɂ���GunPort�̎�ʃ^�O���폜����
                commandBuffer.RemoveComponent<StraightGunPortTag>(entity);
                commandBuffer.RemoveComponent<WideGunPortTag>(entity);

                // AimGunPortTag��ǉ�����
                commandBuffer.AddComponent(entity, new AimGunPortTag { });
                break;
        }

    }

}