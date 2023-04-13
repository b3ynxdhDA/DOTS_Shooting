using Unity.Entities;

public class BossPhaseSytem : SystemBase
{
    // �ϐ��錾------------------------------------------------------------------
    // ���s�^�C�~���O���Ǘ����Ă���V�X�e���O���[�v
    private BeginSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

    // ��1�i�K�̋�ݒ肳��Ă��邩
    private bool _isKomaInitialize�@= false;

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
        if (!_isKomaInitialize)
        {
            SetKomaDate(bossManager.BossKomaData1);
            _isKomaInitialize = true;
        }

        // �Q�[���̃X�e�[�g���Q�[�����ȊO�Ȃ珈�����Ȃ�
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
                // ���݂̃{�X�̍U���i�K�ɍ��킹������������
                switch (bossManager.BossPhaseCount)
                {
                    // ��1�i�K
                    case 1:
                        // �{�X��HP��0��菬�����Ȃ�����
                        if (enemyTag._enemyHp < 0)
                        {
                            // @���̋���Z�b�g����
                            SetKomaDate(bossManager.BossKomaData2);

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
                            SetKomaDate(bossManager.BossKomaData3);

                            // �{�X�̍U���i�K���グ��
                            bossManager.UpdateBossCount();
                        }
                        break;
                    // ��3�i�K
                    case 3:
                        // �{�X��HP��0��菬�����Ȃ�����
                        if (enemyTag._enemyHp < 0)
                        {

                        }
                        break;
                }

            }).Run();

        // �w�肵��Job�������ECB�ɓo�^�������߂����s
        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }

    /// <summary>
    /// ��̃X�e�[�^�X��ݒ肷��
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
        // �R�}���h�o�b�t�@���擾
        EntityCommandBuffer comandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

        // ���ɃZ�b�g����GunPort�̎�ނ͉���
        switch (komaData.shootKind)
        {
            case KomaData.ShootKind.StraightGunPortTag:

                // ���ɂ���GunPort�̎�ʃ^�O���폜����
                comandBuffer.RemoveComponent<WideGunPortTag>(entity);
                comandBuffer.RemoveComponent<AimGunPortTag>(entity);

                // StraightGunPortTag��ǉ�����
                comandBuffer.AddComponent(entity, new StraightGunPortTag
                {
                    _lines = komaData.shootLine
                });
                break;
            case KomaData.ShootKind.WideGunPortTag:

                // ���ɂ���GunPort�̎�ʃ^�O���폜����
                comandBuffer.RemoveComponent<StraightGunPortTag>(entity);
                comandBuffer.RemoveComponent<AimGunPortTag>(entity);

                // WideGunPortTag��ǉ�����
                comandBuffer.AddComponent(entity, new WideGunPortTag
                {
                    _lines = komaData.shootLine
                });
                break;
            case KomaData.ShootKind.AimGunPortTag:

                // ���ɂ���GunPort�̎�ʃ^�O���폜����
                comandBuffer.RemoveComponent<StraightGunPortTag>(entity);
                comandBuffer.RemoveComponent<WideGunPortTag>(entity);

                // AimGunPortTag��ǉ�����
                comandBuffer.AddComponent(entity, new AimGunPortTag { });
                break;
        }

    }

}