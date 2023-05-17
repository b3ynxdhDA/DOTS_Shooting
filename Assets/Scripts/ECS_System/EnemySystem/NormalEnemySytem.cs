using Unity.Collections;
using Unity.Entities;

public class NormalEnemySytem : SystemBase
{
    // �ϐ��錾------------------------------------------------------------------
    // 
    private EntityManager entityManager;

    // ���s�^�C�~���O���Ǘ����Ă���V�X�e���O���[�v
    private BeginSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

    private bool _isNormalEnemyInitialize = false;

    /// <summary>
    /// �V�X�e���쐬���ɌĂ΂�鏈��
    /// </summary>
    protected override void OnCreate()
    {
        //�@�f�t�H���g�̃��[���h��EntityManager�̎擾
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

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
        NormalEnemyManager normalEnemyManager = gameManager.NormalEnemyManager;

        // �R�}���h�o�b�t�@���擾
        EntityCommandBuffer commandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

        Entities
            .WithName("Enemy")
            .WithAll<EnemyTag>()
            .WithNone<BossEnemyTag>()
            .WithoutBurst()
            .ForEach((Entity entity, ref HPTag hpTag, ref GunPortTag gunPortTag) =>
            {   
                // enemyTag�������G���e�B�e�B��z��Ɏ擾������
                //NativeArray<Entity> enemys = entityManager.get

                // @��񂵂����s���Ȃ��ƈ�̂�������������Ȃ�
                // �t�B�[���h��OnCreate�ł�Manager���擾�ł��Ȃ������̂�OnUpdate�ŏ�����
                if (!_isNormalEnemyInitialize)
                {
                    gameManager.KomaManager.SetKomaDate(entity, ref hpTag, ref gunPortTag, normalEnemyManager.NormalEnemyKomaData, commandBuffer);
                    _isNormalEnemyInitialize = true;
                    
                }
            }).Run();

        // �w�肵��Job�������ECB�ɓo�^�������߂����s
        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}