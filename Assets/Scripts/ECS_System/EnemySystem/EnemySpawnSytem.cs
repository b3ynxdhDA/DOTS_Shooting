using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;

/// <summary>
/// �G���G�𐶐�����System
/// </summary>
public class EnemySpawnSytem : SystemBase
{
    // �ϐ��錾------------------------------------------------------------------

    // ���s�^�C�~���O���Ǘ����Ă���V�X�e���O���[�v
    private EntityCommandBufferSystem _entityCommandBufferSystem;

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
        // EntityCommandBuffer�̎擾
        _entityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();


    }

    /// <summary>
    /// �V�X�e���L�����Ƀt���[�����ɌĂ΂�鏈��
    /// </summary>
    protected override void OnUpdate()
    {
        // �R�}���h�o�b�t�@���擾
        EntityCommandBuffer concurrent = _entityCommandBufferSystem.CreateCommandBuffer();

        // �eManager���擾
        GameManager gameManager = GameManager.instance;
        NormalEnemyManager normalEnemyManager = gameManager.NormalEnemyManager;

        // ��f�[�^��z�񂩂�g�p�����������_���Ɍ��߂�
        KomaData nowKomaData = normalEnemyManager.NormalEnemyKomaData[UnityEngine.Random.Range(0, normalEnemyManager.NormalEnemyKomaData.Length)];

        Entities
            .WithName("EnemySpawn")
            .WithAll<Spawner, SpawnTag>()
            .WithoutBurst()
            .ForEach((Entity entity, in SpawnerData spawnerData) =>
            {
                // �G���e�B�e�B�𐶐�
                Entity createEntity = concurrent.Instantiate(spawnerData.SpawnPrefabEntity);

                // GunPortTag�R���|�[�l���g���擾
                GunPortTag gunPortTag = GetComponent<GunPortTag>(createEntity);

                // �G���G�̋�f�[�^��ݒ肷��
                gameManager.KomaManager.SetKomaDate(createEntity, nowKomaData, gunPortTag, concurrent);

                concurrent.RemoveComponent<SpawnTag>(entity);

            }).Run();

        // �w�肵��Job�������ECB�ɓo�^�������߂����s
        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}