using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;

/// <summary>
/// �v���C���[�̏�Ԃ��݂�V�X�e��
/// </summary>
public class PlayerStateSystem : SystemBase
{
    // �ϐ��錾------------------------------------------------------------------
    // ���s�^�C�~���O���Ǘ����Ă���V�X�e���O���[�v
    private EndSimulationEntityCommandBufferSystem _entityCommandBufferSystem;


    // �萔�錾--------------------------------------------------------


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
        // �eManager���擾
        GameManager gameManager = GameManager.instance;
        PlayerManager playerManager = gameManager.PlayerManager;

        // �R�}���h�o�b�t�@���擾
        EntityCommandBuffer commandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

        Entities
            .WithName("Player_State")
            .WithAll<PlayerTag>()
            .WithoutBurst()
            .ForEach((Entity entity, ref HPTag hpTag, ref GunPortTag gunPortTag) =>
            {
                // �t�B�[���h��OnCreate�ł�Manager���擾�ł��Ȃ������̂�OnUpdate�ŏ�����
                if (!playerManager.GetSetIsPlayerInitialize)
                {
                    SetPlayerKomaDate(entity, ref hpTag, ref gunPortTag, playerManager.PlayerKomaData);
                    playerManager.GetSetIsPlayerInitialize = true;
                }

                // �v���C���[��HP��0�ȉ��Ȃ����
                if (hpTag._hp <= 0)
                {
                    commandBuffer.DestroyEntity(entity);
                    gameManager.UIManager.CallGameFinish(false);
                }

            }).Run();// ���C���X���b�h����

        // �w�肵��Job�������ECB�ɓo�^�������߂����s
        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }

    /// <summary>
    /// �v���C���[�̋�f�[�^���Z�b�g����
    /// </summary>
    private void SetPlayerKomaDate(Entity entity, ref HPTag hpTag, ref GunPortTag gunPortTag, KomaData komaData)
    {
        // �R�}���h�o�b�t�@���擾
        EntityCommandBuffer commandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

        // �v���C���[�̊�b�X�e�[�^�X��ݒ肷��
        hpTag._hp = komaData.hp;
        gunPortTag._shootCoolTime = komaData.shootCoolTime;
        gunPortTag._bulletSpeed = komaData.bulletSpeed;

        // �}�e���A����ύX
        commandBuffer.SetSharedComponent(entity, new RenderMesh
        {
            mesh = GameManager.instance.Quad,
            material = komaData.material
        });

        // �ˌ��̎�ނ̃R���|�[�l���g��ݒ肷��
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
