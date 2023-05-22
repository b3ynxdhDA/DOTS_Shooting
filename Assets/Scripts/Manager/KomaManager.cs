using Unity.Entities;
using Unity.Rendering;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
#if UNITY_EDITOR
using UnityEngine;
#endif

/// <summary>
/// ����Ǘ�����N���X
/// </summary>
public class KomaManager
{
    private System.Threading.Tasks.Task<KomaData> normalKoma;

    private Dictionary<GameManager.KomaKind, KomaData> _normalKoma = new Dictionary<GameManager.KomaKind, KomaData>();


    /// <summary>
    /// �Q�[���J�n����GameManager����Ă΂��KomaManager�̎Q�ƃ��\�b�h
    /// </summary>
    public KomaManager()
    {
        // Sprite�����[�h
        var sprite = Addressables.LoadAssetAsync<KomaData>("NariKoma/Koma01_f_hu.asset");

        // ���[�h�Ɏ��s���Ă��ADebug.LogError�ɕ\������邾���ŁA�G���[�ɂ͂Ȃ�Ȃ��͗l
        var sprite2 = Addressables.LoadAssetAsync<KomaData>("hoge").Task;
        if (sprite2 == default)
        {
            // default�ł���΁A���[�h�Ɏ��s���Ă���
            Debug.LogError("���[�h�Ɏ��s���܂���");
        }

        Debug.Log(sprite);
        //�@�g���I������烁��������J������
        Addressables.Release(sprite);
    }

    /// <summary>
    /// �{�X�̋�̃X�e�[�^�X��ݒ肷��
    /// </summary>
    /// <param name="entity">�Ώۂ̃G���e�B�e�B</param>
    /// <param name="hpTag">�Ώۂ�HPTag�R���|�[�l���g</param>
    /// <param name="gunPortTag">�Ώۂ�GunPortTag�R���|�[�l���g</param>
    /// <param name="komaData">�ݒ肷��KomaDate</param>
    public void SetKomaDate(Entity entity, ref HPTag hpTag, ref GunPortTag gunPortTag, KomaData komaData, EntityCommandBuffer commandBuffer)
    {
        // �{�X�̊�b�X�e�[�^�X��ݒ肷��
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