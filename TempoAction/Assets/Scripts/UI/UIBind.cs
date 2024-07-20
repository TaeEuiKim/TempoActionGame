using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBind : MonoBehaviour
{
    public Define.UIType type;

    private void Awake()
    {
        switch (type)
        {
            case Define.UIType.BUTTON:
                Bind<Button>();
                break;
            case Define.UIType.CANVASGROUP:
                Bind<CanvasGroup>();
                break;
            case Define.UIType.IMAGE:
                Bind<Image>();
                break;
            case Define.UIType.TMPRO:
                Bind<TextMeshProUGUI>();
                break;
            case Define.UIType.TOGGLE:
                Bind<Toggle>();
                break;
            case Define.UIType.SLIDER:
                Bind<Slider>();
                break;
            default:
                Bind<Button>();
                Bind<CanvasGroup>();
                Bind<Image>();
                Bind<TextMeshProUGUI>();
                Bind<Toggle>();
                Bind<Slider>();
                break;

        }
    }

    private void Bind<T>() where T : UnityEngine.Object //UI ¹­´Â ºÎºÐ
    {
        Dictionary<string, UnityEngine.Object> typeList = new Dictionary<string, UnityEngine.Object>();

        foreach (T t in gameObject.GetComponentsInChildren<T>())
        {
            if (typeList.ContainsKey(t.name))
            {
                t.name += Random.Range(0, 99999);
            }

            typeList.Add(t.name, t);
        }

        if (UIManager.Instance.Storage.ContainsKey(typeof(T)))
        {

            foreach (var t in typeList)
            {
                UIManager.Instance.Storage[typeof(T)].Add(t.Key, t.Value);
            }

        }
        else
        {
            UIManager.Instance.Storage.Add(typeof(T), typeList);
        }


    }
}