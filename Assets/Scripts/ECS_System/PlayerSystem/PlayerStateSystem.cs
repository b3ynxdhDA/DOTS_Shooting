using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

/// <summary>
/// �v���C���[�̏�Ԃ��݂�V�X�e��
/// </summary>
public class PlayerStateSystem : SystemBase
{
    // �ϐ��錾------------------------------------------------------------------
    // ���s�^�C�~���O���Ǘ����Ă���V�X�e���O���[�v
    private EndSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

    // �v���C���[�̋����������Ă��邩
    private bool _isKomaInitialize = false;

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
            .ForEach((Entity entity, ref PlayerTag playerTag, ref GunPortTag gunPortTag) =>
            {
                    SetPlayerKomaDate(entity, playerTag, gunPortTag, playerManager.PlayerKomaData);
                // �t�B�[���h��OnCreate�ł�Manager���擾�ł��Ȃ������̂�OnUpdate�ŏ�����
                if (!_isKomaInitialize)
                {
                    _isKomaInitialize = true;
                }

                // �v���C���[��HP��0�ȉ��Ȃ����
                if (playerTag._playerHp <= 0)
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
    private void SetPlayerKomaDate(Entity entity, PlayerTag playerTag, GunPortTag gunPortTag, KomaData komaData)
    {

        playerTag._playerHp = komaData.hp;
        gunPortTag._shootCoolTime = komaData.shootCoolTime;
        gunPortTag._bulletSpeed = komaData.bulletSpeed;

        SetShootKInd(entity, komaData);
    }

    /// <summary>
    /// �ˌ��̎�ނ̃R���|�[�l���g��ݒ肷��
    /// </summary>
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
