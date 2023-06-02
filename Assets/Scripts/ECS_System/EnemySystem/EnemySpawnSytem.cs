using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

/// <summary>
/// �G���G�𐶐�����System
/// </summary>
public class EnemySpawnSytem : SystemBase
{
    // �ϐ��錾------------------------------------------------------------------
    // ���s�^�C�~���O���Ǘ����Ă���V�X�e���O���[�v
    private EntityCommandBufferSystem _entityCommandBufferSystem;

    // �G�̏o�����ԊԊu
    private float _spawnCoolTime = 0;

    // �萔�錾------------------------------------------------------------------
    // �G�̏o���Ԋu�̍ő�
    const float _SPAWN_TIME_MAX = 5;

    // �G�̏o���Ԋu�̍ŏ�
    const float _SPAWN_TIME_MIN = 3;

    // �G�̏o���ʒu��X�̍ő�
    const float _SPAWN_POS_X_MAX = 8.6f;

    // �G�̏o���ʒu��X�̍ŏ�
    const float _SPAWN_POS_X_MIN = -8.6f;

    // �G�̏o���ʒu��Y�̍ő�
    const float _SPAWN_POS_Y_MAX = 4.1f;

    // �G�̏o���ʒu��Y�̍ŏ�
    const float _SPAWN_POS_Y_MIN = -1.1f;

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
        // �Q�[���̃X�e�[�g���Q�[�����ȊO�Ȃ珈�����Ȃ�
        if (GameManager.instance.gameState != GameManager.GameState.GameNow)
        {
            return;
        }

        // �R�}���h�o�b�t�@���擾
        EntityCommandBuffer commandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

        // �eManager���擾
        GameManager gameManager = GameManager.instance;
        NormalEnemyManager normalEnemyManager = gameManager.NormalEnemyManager;

        if (_spawnCoolTime < 0)
        {
            float spawnPosX = UnityEngine.Random.Range(_SPAWN_POS_X_MIN, _SPAWN_POS_X_MAX);
            float spawnPosY = UnityEngine.Random.Range(_SPAWN_POS_Y_MIN, _SPAWN_POS_Y_MAX);
            Entities
                .WithName("EnemySpawn")
                .WithAll<Spawner>()
                .WithBurst()
                .ForEach((Entity entity, in SpawnerData spawnerData) =>
                {
                // �G���e�B�e�B�𐶐�
                Entity newEntity = commandBuffer.Instantiate(spawnerData.SpawnPrefabEntity);

                    commandBuffer.SetComponent(newEntity, new Translation
                    {
                        Value = new float3(spawnPosX, spawnPosY, 0f)// @������WithoutBurst()��Run()���K�v
                });

                }).Schedule();

            _spawnCoolTime = UnityEngine.Random.Range(_SPAWN_TIME_MIN, _SPAWN_TIME_MAX);
        }

        _spawnCoolTime -= Time.DeltaTime;

        Entities
            .WithName("EnemyInitialization")
            .WithAll<EnemyTag, PlaneEnemyTag>()
            .WithNone<BossEnemyTag>()
            .WithoutBurst()
            .ForEach((Entity entity, ref GunPortTag gunPortTag) =>
            {

                // ��f�[�^��z�񂩂�g�p�����������_���Ɍ��߂�
                KomaData nowKomaData = normalEnemyManager.NormalEnemyKomaData[UnityEngine.Random.Range(0, normalEnemyManager.NormalEnemyKomaData.Length)];

                // �G���G�̋�f�[�^��ݒ肷��
                gameManager.KomaManager.SetKomaDate(entity, nowKomaData, ref gunPortTag, commandBuffer);

                // ���������Ă��Ȃ��G�ɂ��Ă���R���|�[�l���g�^�O���폜
                commandBuffer.RemoveComponent<PlaneEnemyTag>(entity);

            }).Run();

        // �w�肵��Job�������ECB�ɓo�^�������߂����s
        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}