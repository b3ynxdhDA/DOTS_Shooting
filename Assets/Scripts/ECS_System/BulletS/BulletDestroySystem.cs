using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

/// <summary>
/// ��ʊO�̒e���폜����V�X�e��
/// </summary>
public class BulletDestroySystem : SystemBase
{
    // �ϐ��錾------------------------------------------------------------------
    // ���s�^�C�~���O���Ǘ����Ă���V�X�e���O���[�v
    private EndSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

    private Camera _mainCamera;

    // �X�N���[���̍����̃��[���h���W��
    private Vector2 _lowerLeft;

    // �X�N���[���̉E��̃��[���h���W
    private Vector2 _upperRight;

    // �萔�錾--------------------------------------------------------
    // �X�N���[�����W�̍���
    private readonly Vector2 _MIN_SCREEN_POINT = new Vector2(-0.1f, -0.1f);
    // �X�N���[�����W�̉E��
    private readonly Vector2 _MAX_SCREEN_POINT = new Vector2(1.1f, 1.1f);

    /// <summary>
    /// �V�X�e���쐬���ɌĂ΂�鏈��
    /// </summary>
    protected override void OnCreate()
    {
        // EntityCommandBuffer�̎擾
        _entityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    /// <summary>
    /// �V�X�e�����s��~����OnDestroy()�̑O�ɌĂ΂�鏈��
    /// �J�����̐؂�ւ��ɑΉ����邽�߂����ŃJ�������擾
    /// </summary>
    protected override void OnStartRunning()
    {
        // �X�N���[�����W�����[���h���W�ɕϊ�����
        _mainCamera = Camera.main;
        _lowerLeft = _mainCamera.ViewportToWorldPoint(_MIN_SCREEN_POINT);
        _upperRight = _mainCamera.ViewportToWorldPoint(_MAX_SCREEN_POINT);
    }

    /// <summary>
    /// �V�X�e���L�����Ƀt���[�����ɌĂ΂�鏈��
    /// </summary>
    protected override void OnUpdate()
    {
        // �R�}���h�o�b�t�@���擾
        EntityCommandBuffer commandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

        Entities
            .WithName("Bullet_Destroy")
            .WithAll<BulletTag>()
            .WithBurst()
            .ForEach((Entity entity, in Translation translation) =>
            {
                // �e�̃G���e�B�e�B����ʊO�ɏo�������
                if (translation.Value.x > _upperRight.x || translation.Value.y > _upperRight.y ||
                   translation.Value.x < _lowerLeft.x || translation.Value.y < _lowerLeft.y)
                {
                    commandBuffer.DestroyEntity(entity);
                }

            }).WithoutBurst().Run();// ���C���X���b�h����

        // �w�肵��Job�������ECB�ɓo�^�������߂����s
        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}
