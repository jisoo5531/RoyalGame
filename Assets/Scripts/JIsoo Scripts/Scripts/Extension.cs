using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
    /// <summary>
    /// 셔플 메서드
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="units"></param>
    public static void Shuffle<T>(this List<T> units)
    {
        System.Random random = new System.Random();

        for (int i = units.Count - 1; i > 0; i--)
        {
            int k = random.Next(i + 1);
            T temp = units[k];
            units[k] = units[i];
            units[i] = temp;
        }
    }
    /// <summary>
    /// 유닛 투명도 조절
    /// </summary>
    /// <param name="unitPrefab">대상 유닛 프리팹</param>
    /// <param name="alpha">alpha값 조절</param>
    public static void UnitTransparent(this GameObject unitPrefab, float alpha)
    {
        Renderer[] renderers = unitPrefab.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            // 각 Renderer의 Material 가져오기
            Material material = renderer.material;

            // Shader가 Standard가 아닌 경우 Standard로 변경
            if (material.shader.name != "Standard")
            {
                material.shader = Shader.Find("Standard");
            }

            // "Transparent" 렌더링 모드 설정
            material.SetFloat("_Mode", 3);
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;

            // 투명도 조절
            Color color = material.color;
            color.a = alpha;
            material.color = color;
        }
    }
    /// <summary>
    /// 유닛 유형에 맞게 분류
    /// </summary>
    public static void UnitClassification(this GameObject unit, UnitData_SO unitData)
    {
        if (unitData.type == Type.Unit)
        {
            unit.AddComponent<MovableUnit>();
        }
        else if (unitData.type == Type.Deffense)
        {
            unit.AddComponent<DismovableUnit>();
        }
    }
}
