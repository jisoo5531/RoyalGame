using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Org.BouncyCastle.Asn1;

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
    /// 투명도 조절
    /// </summary>
    /// <param name="image">투명도를 조절할 이미지</param>
    /// <param name="alpha"></param>
    public static void ImageTransparent(this Image image, float alpha)
    {
        Color tempColor = image.color;
        tempColor.a = alpha;
        image.color = tempColor;        
    }
    /// <summary>
    /// 유닛 투명도 조절
    /// </summary>
    /// <param name="unitPrefab">투명도를 조절할 모델</param>
    /// <param name="alpha">alpha값 조절</param>
    public static void UnitTransparent(this GameObject unitPrefab, float alpha)
    {
        Renderer[] renderers = unitPrefab.GetComponentsInChildren<Renderer>();

        if (renderers != null)
        {
            foreach (Renderer renderer in renderers)
            {
                renderer.material = GameManager.instance.allyMaterial[1];
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
        
    }
    /// <summary>
    /// 유닛 유형에 맞게 분류
    /// </summary>
    public static void UnitClassification(this GameObject unit, AllCardData unitData)
    {
        if (unitData.type.Equals("유닛"))
        {
            //// Animator 컴포넌트를 가진 오브젝트에 넣어주기
            //Animator[] unitsAnim = unit.GetComponentsInChildren<Animator>();

            //foreach (var unitAnim in unitsAnim)
            //{
            //    unitAnim.gameObject.AddComponent<MovableUnit>();
            //    unitAnim.gameObject.AddComponent<MovableUnit>();
            //}
            unit.AddComponent<MovableUnit>();
        }
        else if (unitData.type.Equals("방어타워"))
        {
            unit.AddComponent<DeffenseTower>();
        }
        else
        {
            unit.AddComponent<Fireball>();

        }
    }
    public static void ColorNormal<T>(this T uiElement)
    {
        Color color = new Color(154f / 255f, 154f / 255f, 154f / 255f);        

        if (uiElement is Image)
        {
            (uiElement as Image).color = color;
        }
        else if (uiElement is Text)
        {
            (uiElement as Text).color = color;
        }
        else if (uiElement is TMP_Text)
        {
            (uiElement as TMP_Text).color = color;
        }
    }    
    public static void ColorRare<T>(this T uiElement)
    {
        Color color = new Color(255f / 255f, 166f / 255f, 57f / 255f);

        if (uiElement is Image)
        {
            (uiElement as Image).color = color;
        }
        else if (uiElement is Text)
        {
            (uiElement as Text).color = color;
        }
        else if (uiElement is TMP_Text)
        {
            (uiElement as TMP_Text).color = color;
        }
    }
    public static void ColorEpic<T>(this T uiElement)
    {
        Color color = new Color(177f / 255f, 69f / 255f, 255f / 255f);

        if (uiElement is Image)
        {
            (uiElement as Image).color = color;
        }
        else if (uiElement is Text)
        {
            (uiElement as Text).color = color;
        }
        else if (uiElement is TMP_Text)
        {
            (uiElement as TMP_Text).color = color;
        }
    }
    public static void ColorSky<T>(this T uiElement)
    {
        Color color = new Color(29f / 255f, 212f / 255f, 226f / 255f);

        if (uiElement is Image)
        {
            (uiElement as Image).color = color;
        }
        else if (uiElement is Text)
        {
            (uiElement as Text).color = color;
        }
        else if (uiElement is TMP_Text)
        {
            (uiElement as TMP_Text).color = color;
        }
    }

    public static void ColorGreen<T>(this T uiElement)
    {
        Color color = new Color(80f / 255f, 255f / 255f, 0f / 255f);

        if (uiElement is Image)
        {
            (uiElement as Image).color = color;
        }
        else if (uiElement is Text)
        {
            (uiElement as Text).color = color;
        }
        else if (uiElement is TMP_Text)
        {
            (uiElement as TMP_Text).color = color;
        }
    }

    public static void ColorWhite<T>(this T uiElement)
    {
        Color color = new Color(255f / 255f, 255f / 255f, 255f / 255f);

        if (uiElement is Image)
        {
            (uiElement as Image).color = color;
        }
        else if (uiElement is Text)
        {
            (uiElement as Text).color = color;
        }
        else if (uiElement is TMP_Text)
        {
            (uiElement as TMP_Text).color = color;
        }
    }

    public static void ColorBlue<T>(this T uiElement)
    {
        Color color = new Color(0f / 255f, 49f / 255f, 255f / 255f);

        if (uiElement is Image)
        {
            (uiElement as Image).color = color;
        }
        else if (uiElement is Text)
        {
            (uiElement as Text).color = color;
        }
        else if (uiElement is TMP_Text)
        {
            (uiElement as TMP_Text).color = color;
        }
    }
    
    public static void ColorYellow<T>(this T uiElement)
    {
        Color color = new Color(255f / 255f, 207f / 255f, 0f / 255f);

        if (uiElement is Image)
        {
            (uiElement as Image).color = color;
        }
        else if (uiElement is Text)
        {
            (uiElement as Text).color = color;
        }
        else if (uiElement is TMP_Text)
        {
            (uiElement as TMP_Text).color = color;
        }
    }
    
}
