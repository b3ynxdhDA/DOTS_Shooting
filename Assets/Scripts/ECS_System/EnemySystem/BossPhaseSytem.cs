using Unity.Entities;

public class BossPhaseSytem : SystemBase
{
    // �ϐ��錾------------------------------------------------------------------
    // ���s�^�C�~���O���Ǘ����Ă���V�X�e���O���[�v
    private BeginSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

    // �萔�錾------------------------------------------------------------------
    // �{�X�̒i�K���ω�����HP
    private const int _CHANGE_PHASE_HP = 1;

    /// <summary>
    /// �V�X�e���쐬���ɌĂ΂�鏈��
    /// </summary>
    protected override void OnCreate()
    {
        // EntityCommandBuffer�̎擾
        _entityCommandBufferSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
    }

    /// <summary>
    /// �V�X�e���L�����Ƀt���[�����ɌĂ΂�鏈��
    /// </summary>
    protected override void OnUpdate()
    {
        // �eManager���擾
        GameManager gameManager = GameManager.instance;
        BossManager bossManager = gameManager.BossManager;

        // �R�}���h�o�b�t�@���擾
        EntityCommandBuffer commandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

        Entities
            .WithName("Boss_Phase")
            .WithAll<EnemyTag, BossEnemyTag>()
            .WithoutBurst()
            .ForEach((Entity entity, ref GunPortTag gunPortTag, in HPTag hpTag) =>
            {
                // �t�B�[���h��OnCreate�ł�Manager���擾�ł��Ȃ������̂�OnUpdate�ŏ�����
                if (!bossManager.IsBossInitialize)
                {
                    // ���i�K�̋���Z�b�g����
                    gameManager.KomaManager.SetKomaDate(entity, bossManager.BossKomaData1, gunPortTag, commandBuffer);

                    // HP�o�[��ݒ�
                    gameManager.UIManager.SetSliderBossHP(bossManager.BossKomaData1);

                    bossManager.IsBossInitialize = true;
                }

                // ���݂̃{�X�̍U���i�K�ɍ��킹������������
                switch (bossManager.BossPhaseCount)
                {
                    // ��1�i�K
                    case 1:
                        // �{�X��HP��0��菬�����Ȃ�����
                        if (hpTag._hp < _CHANGE_PHASE_HP)
                        {
                            // ���̋���Z�b�g����
                            gameManager.KomaManager.SetKomaDate(entity, bossManager.BossKomaData2, gunPortTag, commandBuffer);

                            // HP�o�[���Đݒ�
                            gameManager.UIManager.SetSliderBossHP(bossManager.BossKomaData2);

                            // �{�X�̍U���i�K���グ��
                            bossManager.UpdateBossCount();
                        }
                        break;
                    // ��2�i�K
                    case 2:
                        // �{�X��HP��0��菬�����Ȃ�����
                        if (hpTag._hp < _CHANGE_PHASE_HP)
                        {
                            // ���̋���Z�b�g����
                            gameManager.KomaManager.SetKomaDate(entity, bossManager.BossKomaData3, gunPortTag, commandBuffer);

                            // HP�o�[���Đݒ�
                            gameManager.UIManager.SetSliderBossHP(bossManager.BossKomaData3);

                            // �{�X�̍U���i�K���グ��
                            bossManager.UpdateBossCount();
                        }
                        break;
                    // ��3�i�K
                    case 3:
                        // �{�X��HP��0��菬�����Ȃ�����
                        if (hpTag._hp < _CHANGE_PHASE_HP)
                        {
                            gameManager.UIManager.CallGameFinish(true);
                        }
                        break;
                }

                // �Q�Ƃł���悤��BossManager��HP��n��
                bossManager.GetBossHP = hpTag._hp;

            }).Run();

        // �w�肵��Job�������ECB�ɓo�^�������߂����s
        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}