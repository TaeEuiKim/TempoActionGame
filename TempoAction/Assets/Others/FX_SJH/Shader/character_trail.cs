using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTrail : MonoBehaviour
{
    public float activeTime = 2f;

    [Header("Mesh Related")]
    public float meshRefreshRate = 0.1f;

    [Header("Shader Related")]
    public Material mat;
    public string shaderVarRef;
    public float shaderVarRate = 0.1f;
    public float shaderVarRefreshRate = 0.05f;

    public SkinnedMeshRenderer[] skinnedMeshRenderers;
    private bool isTrailActive;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && !isTrailActive)
        {
            isTrailActive = true;
            StartCoroutine(ActivateTrail(activeTime));
        }
    }

    public void StartTrail(float activetime)
    {
        StartCoroutine(ActivateTrail(activetime));
    }

    IEnumerator ActivateTrail(float timeActive)
    {
        Player player = GetComponentInParent<Player>();

        while (timeActive > 0)
        {
            timeActive -= meshRefreshRate;

            foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
            {
                GameObject gObj = ObjectPool.Instance.Spawn("TrailMesh", 1f);

                MeshRenderer meshRenderer = gObj.GetComponent<MeshRenderer>();
                MeshFilter meshFilter = gObj.GetComponent<MeshFilter>();

                // 사용자 지정 머티리얼 설정 (Material이 null이 아닌지 확인)
                if (mat != null)
                {
                    meshRenderer.material = new Material(mat);
                }
                else
                {
                    Debug.LogWarning("Material is null. Please assign a material.");
                }

                // 현재 애니메이션이 적용된 상태의 메쉬를 추출
                Mesh bakedMesh = new Mesh();
                skinnedMeshRenderer.BakeMesh(bakedMesh);

                // 노멀 벡터 재계산
                bakedMesh.RecalculateNormals();

                meshFilter.mesh = bakedMesh;

                // 원래 스킨된 메쉬의 위치와 회전을 유지
                gObj.transform.position = skinnedMeshRenderer.transform.position;
                gObj.transform.rotation = skinnedMeshRenderer.transform.rotation;

                // 셰이더 변수 애니메이션 시작 (Material이 null이 아닐 때만)
                if (mat != null)
                {
                    StartCoroutine(AnimateMaterialFloat(meshRenderer.material, 0, shaderVarRate, shaderVarRefreshRate));
                }
            }

            yield return new WaitForSeconds(meshRefreshRate);
        }

        player.Controller.isUltimate = false;
        for (int i = 0; i < player.MoveEffect.Length; ++i)
        {
            player.MoveEffect[i].SetActive(false);
        }

        GameObject effect = ObjectPool.Instance.Spawn("do_disappear", 1);
        effect.transform.position = player.SkillObject.transform.position;

        TestSound.Instance.PlaySound("Skill1");
        player.RimShader.SetFloat("_Float", 0f);
        player.SkillObject.SetActive(true);
        isTrailActive = false;
    }

    IEnumerator AnimateMaterialFloat(Material mat, float goal, float rate, float refreshRate)
    {
        float valueToAnimate = mat.GetFloat(shaderVarRef);

        while (valueToAnimate > goal)
        {
            valueToAnimate -= rate;
            mat.SetFloat(shaderVarRef, valueToAnimate);
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
