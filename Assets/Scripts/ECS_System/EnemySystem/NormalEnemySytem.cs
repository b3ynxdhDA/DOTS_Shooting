using Unity.Collections;
using Unity.Entities;

public class NormalEnemySytem : SystemBase
{
    // �ϐ��錾------------------------------------------------------------------
    // �G���e�B�e�B�}�l�[�W���[���擾
    private EntityManager entityManager;

    // ���s�^�C�~���O���Ǘ����Ă���V�X�e���O���[�v
    private BeginSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

    // ���ڂ̏������I��������ǂ���
    private bool _isNormalEnemyInitialize = false;

    // �S�ẴG���e�B�e�B�ŏ�����������
    private bool _isLoopEntity = false;

    // �ŏ��ɏ��������G���e�B�e�B�̃C���f�b�N�X
    private int _firstIndex = 0;

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
                if (_isLoopEntity)
                {
                    return;
                }

                // �S�ẴG���e�B�e�B�ň�x��������������
                if(_firstIndex == entity.Index && !_isLoopEntity)
                {
                    _isLoopEntity = true;
                }

                // �t�B�[���h��OnCreate�ł�Manager���擾�ł��Ȃ������̂�OnUpdate�ŏ�����
                if (!_isNormalEnemyInitialize)
                {
                    _isNormalEnemyInitialize = true;
                    _firstIndex = entity.Index;
                }

                // �G���G�̋�f�[�^��ݒ肷��
                gameManager.KomaManager.SetKomaDate(entity, ref hpTag, ref gunPortTag, normalEnemyManager.NormalEnemyKomaData, commandBuffer);
                    
            }).Run();

        // �w�肵��Job�������ECB�ɓo�^�������߂����s
        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}