using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Rendering;

/// <summary>
/// ����Ǘ�����N���X
/// </summary>
public class NormalEnemyManager : MonoBehaviour
{

    #region �ϐ��錾

    /// <summary>
    /// ���������̃v���C���[�̋�f�[�^
    /// </summary>
    public KomaData NormalEnemyKomaData { get; private set; }

    #endregion
    /// <summary>
    /// �Q�[���J�n����GameManager����Ă΂��KomaManager�̎Q�ƃ��\�b�h
    /// </summary>
    public NormalEnemyManager(KomaData komaData)
    {
        NormalEnemyKomaData = komaData;
    }

    /// <summary>
    /// �G���G�̏�����
    /// </summary>
    public void NormalEnemyInitialize()
    {

    }

    /// <summary>
    /// �G���G�𐶐�����
    /// </summary>
    private void NormalEnemyInstantiate()
    {
        KomaManager komaManager = GameManager.instance.KomaManager;

        // �G���e�B�e�B�̐���
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        // �e�̃G���e�B�e�B�̃A�[�L�^�C�v
        EntityArchetype bulletArchetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(LocalToWorld),
            typeof(BulletTag),
            typeof(BulletMoveDirectionTag),
            typeof(EnemyBulletTag)
            );

        // �e�̃G���e�B�e�B�𐶐�
        Entity bulletEntity = entityManager.CreateEntity(bulletArchetype);

        // �G���G�̃G���e�B�e�B�̃A�[�L�^�C�v
        EntityArchetype enemyArchetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(LocalToWorld),
            typeof(HPTag),
            typeof(EnemyTag),
            typeof(GunPortTag)
            );

        // �G���G�̃G���e�B�e�B�𐶐�
        Entity enemyEntity = entityManager.CreateEntity(enemyArchetype);



        // Translation�R���|�[�l���g�̃f�[�^�̎w��
        entityManager.AddComponentData(enemyEntity, new Translation
        {
            Value = new float3(0f, 0f, 0f)
        });
    }

}