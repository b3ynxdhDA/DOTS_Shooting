using Unity.Entities;

public class BossPhaseSytem : SystemBase
{
    // �ϐ��錾------------------------------------------------------------------
    // ���s�^�C�~���O���Ǘ����Ă���V�X�e���O���[�v
    private BeginSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

    // ��2�t�F�[�Y�̃{�X��HP
    private float _2ndPhaseHP;

    // ��3�t�F�[�Y�̃{�X��HP
    private float _3rdPhaseHP;

    // �萔�錾--------------------------------------------------------



    protected override void OnCreate()
    {
        // EntityCommandBuffer�̎擾
        _entityCommandBufferSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();

        // �{�X�L�����N�^�[��HP��������
        Entities
            .WithName("Boss_HP_Initialize")
            .WithAll<EnemyTag,BossEnemyTag>()
            .ForEach((ref EnemyTag enemyTag, in BossEnemyTag bossEnemyTag) =>
            {
                enemyTag._enemyHp = bossEnemyTag._1st_bossHp;
                _2ndPhaseHP = bossEnemyTag._2nd_bossHp;
                _3rdPhaseHP = bossEnemyTag._3rd_bossHp;
            }).Run();

        
    }

    protected override void OnUpdate()
    {
        GameManager gameManager = GameManager.instance;
        BossManager bossManager = gameManager.BossManager;
        // �R�}���h�o�b�t�@���擾
        EntityCommandBuffer comandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

        Entities
            .WithName("Boss_Phase")
            .WithAll<EnemyTag, BossEnemyTag>()
            .ForEach((Entity entity, ref EnemyTag enemyTag) =>
            {
                // �Q�[���̃X�e�[�g���Q�[�����ȊO�Ȃ珈�����Ȃ�
                if (gameManager.game_State != GameManager.GameState.GameNow)
                {
                    return;
                }

                switch (bossManager.BossPhaseCount)
                {
                    case 0:
                        Phase0(ref enemyTag);
                        break;
                }

            }).Run();

        // �w�肵��Job�������ECB�ɓo�^�������߂����s
        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }

    private void Phase0(ref EnemyTag enemyTag)
    {
        // �{�X��HP��0��菬�����Ȃ�����
        if(enemyTag._enemyHp < 0)
        {

        }
    }
}
