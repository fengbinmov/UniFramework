using UnityEngine;
using System.Collections.Generic;
using System.Text;

namespace Uni.UniPanel
{
    [RequireComponent(typeof(RectTransform))]
    public class PanelPool : MonoBehaviour
    {
        private Transform contentShow;
        private Transform contentClose;

        protected List<PanelBase> loadedUI = new List<PanelBase>();     //页面容器 -- 存放所有实例化的页面
        protected List<int> listOpenUI = new List<int>();               //记录已经显示的页面ID

        [Tooltip("保持显示层级与Canvas一样大")]
        public bool IsRectFull = true;

        /// <summary>
        /// 初始化界面系统
        /// </summary>
        public virtual void OnInit()
        {
            if (contentClose == null) contentClose = transform.Find("- Close Layer");
            if (contentShow == null) contentShow = transform.Find("- Show Layer");

            if (contentClose == null) 
            {
                GameObject obj = new GameObject("- Close Layer",typeof(RectTransform));
                contentClose = obj.transform;
                contentClose.SetParent(transform);
            }
            contentClose.gameObject.hideFlags = HideFlags.HideInHierarchy;
            contentClose.gameObject.transform.localScale = Vector3.one;
            contentClose.gameObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            if (IsRectFull) RectFull((RectTransform)contentClose);

            if (contentShow == null)
            {
                GameObject obj = new GameObject("- Show Layer", typeof(RectTransform));
                contentShow = obj.transform;
                contentShow.SetParent(transform);
            }
            contentShow.gameObject.transform.localScale = Vector3.one;
            contentShow.gameObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            if (IsRectFull) RectFull((RectTransform)contentShow);

            DestroyAll();

            loadedUI.Clear();
            listOpenUI.Clear();
        }

        /// <summary>
        /// 打开页面
        /// </summary>
        /// <param name="panelName">页面名称</param>
        /// <param name="param">传入参数</param>
        /// <returns>页面</returns>
        public PanelBase ShowPanel(string panelName, System.Object param = null,bool coercion = false)
        {
            return ShowPanel(GetPanel(panelName), param, coercion);
        }

        /// <summary>
        /// 打开页面
        /// </summary>
        /// <typeparam name="T">页面类型</typeparam>
        /// <param name="param">传入参数</param>
        /// <returns>页面</returns>
        public T ShowPanel<T>(System.Object param = null, bool coercion = false) where T : PanelBase
        {
            return (T)ShowPanel(GetPanel<T>(), param, coercion);
        }

        /// <summary>
        /// 打开页面
        /// </summary>
        /// <typeparam name="T">页面类型</typeparam>
        /// <param name="panelName">页面名称</param>
        /// <param name="param">传入参数</param>
        /// <returns>页面</returns>
        public T ShowPanel<T>(string panelName, System.Object param = null, bool coercion = false) where T : PanelBase
        {
            return (T)ShowPanel(GetPanel<T>(panelName), param, coercion);
        }

        /// <summary>
        /// 打开页面
        /// </summary>
        /// <param name="panel">页面</param>
        /// <param name="param">传入参数</param>
        /// <param name="coercion">覆写</param>
        /// <returns>页面</returns>
        public virtual PanelBase ShowPanel(PanelBase panel, System.Object param = null, bool coercion = false)
        {
            if (panel == null) return null;

            if (listOpenUI.Contains(panel.ID))
            {
                if (coercion)
                {
                    panel.transform.SetParent(contentShow);
                    panel.transform.SetAsLastSibling();

                    panel.ShowPanel(param);
                }
                return panel;
            }

            panel.transform.SetParent(contentShow);
            panel.transform.SetAsLastSibling();

            listOpenUI.Add(panel.ID);
            panel.ShowPanel(param);

            return panel;
        }

        /// <summary>
        /// 关闭页面
        /// </summary>
        /// <param name="panelName">页面名称</param>
        public void ClosePanel(string panelName)
        {
            ClosePanel(GetPanel(panelName));
        }

        /// <summary>
        /// 关闭页面
        /// </summary>
        /// <typeparam name="T">页面类型</typeparam>
        public void ClosePanel<T>() where T : PanelBase
        {
            ClosePanel(GetPanel<T>());
        }

        /// <summary>
        /// 关闭页面
        /// </summary>
        /// <param name="panelName">页面名称</param>
        public void ClosePanel<T>(string panelName) where T : PanelBase
        {
            ClosePanel(GetPanel<T>(panelName));
        }

        /// <summary>
        /// 关闭页面
        /// </summary>
        /// <param name="panel">页面</param>
        public virtual void ClosePanel(PanelBase panel)
        {
            if (panel == null) return;
            if (!listOpenUI.Contains(panel.ID)) return;

            panel.ClosePanel();
            panel.transform.SetParent(contentClose);
            panel.transform.SetAsLastSibling();

            listOpenUI.Remove(panel.ID);
        }

        public void CloseAllPanel()
        {
            for (int i = loadedUI.Count - 1; i >= 0; i--)
            {
                ClosePanel(loadedUI[i]);
            }
        }

        /// <summary>
        /// 销毁所有页面
        /// </summary>
        public void DestroyAll()
        {
            for (int i = loadedUI.Count - 1; i >= 0; i--)
            {
                DestroyPanel(loadedUI[i]);
            }
        }

        /// <summary>
        /// 销毁页面
        /// </summary>
        /// <param name="panel">页面</param>
        public void DestroyPanel(string panelName)
        {
            DestroyPanel(GetPanel(panelName));
        }

        /// <summary>
        /// 销毁页面
        /// </summary>
        /// <param name="panel">页面</param>
        public void DestroyPanel(PanelBase panel)
        {
            if (panel == null) return;

            int index = loadedUI.IndexOf(panel);

            if (index < 0) return;

            loadedUI.RemoveAt(index);

            ClosePanel(panel);

            GameObject.Destroy(panel.gameObject);
        }

        /// <summary>
        /// 获取页面 (模糊定位)
        /// </summary>
        /// <param name="name">页面名称</param>
        /// <returns>页面</returns>
        public PanelBase GetPanel(string name) {

            for (int i = loadedUI.Count - 1; i >= 0; i--)
            {
                if (loadedUI[i] == null)
                {
                    Debug.LogWarning($"GetPanel find a null panel! {i}");
                    loadedUI.RemoveAt(i);
                    continue;
                }
                if (loadedUI[i].name.Equals(name))
                    return loadedUI[i];
            }

            return null;
        }

        /// <summary>
        /// 获取页面  (模糊定位)
        /// </summary>
        /// <returns>页面</returns>
        public T GetPanel<T>() where T : PanelBase
        {
            var ttype = typeof(T);
            for (int i = 0; loadedUI.Count > i; i++)
            {
                if (loadedUI[i].GetType().Equals(ttype))
                    return (T)loadedUI[i];
            }
            return null;
        }

        /// <summary>
        /// 获取页面
        /// </summary>
        /// <returns>页面</returns>
        public T GetPanel<T>(string name) where T : PanelBase
        {
            var ttype = typeof(T);
            for (int i = 0; loadedUI.Count > i; i++)
            {
                var panel = loadedUI[i];
                if (panel.name.Equals(name) && panel.GetType().Equals(ttype))
                {
                    return (T)panel;
                }
            }
            return null;
        }

        /// <summary>
        /// 是否包含页面
        /// </summary>
        /// <param name="name">页面名称</param>
        /// <returns>页面</returns>
        private bool IsContains(string name)
        {
            for (int i = 0; i < loadedUI.Count; i++)
            {
                if (loadedUI[i].name == name)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 添加页面到Close容器
        /// </summary>
        /// <param name="prefab">页面预制件</param>
        /// <param name="panelName">页面名</param>
        /// <returns>页面</returns>
        public PanelBase AddPanel(GameObject prefab, string panelName = null)
        {
            PanelBase panel = AddPanel<PanelBase>(prefab, panelName);
            return panel;
        }

        /// <summary>
        /// 添加页面到Close容器
        /// </summary>
        /// <param name="prefab">页面预制件</param>
        /// <param name="panelName">页面名</param>
        /// <returns>页面</returns>
        public virtual T AddPanel<T>(GameObject prefab, string panelName = null) where T : PanelBase
        {
            if (prefab.GetComponent<T>() == null) {

                Debug.LogError($"{prefab.name} 中未发现 Panel 组件！");
                return null;
            }

            if (panelName == null) panelName = nameof(T);

            if (IsContains(panelName))
            {
                Debug.LogWarning($"有相同名称的页面加入 {panelName}！");
                return null;
            }
            else
            {
                GameObject obj = GameObject.Instantiate(prefab, contentClose);
                if (panelName != null) obj.name = panelName;
                PanelBase panel = obj.GetComponent<PanelBase>();

                loadedUI.Add(panel);
                Debug.Log($"PanelPool AddPanel [prefab,{prefab.name}] [name,{panelName}] [index,{loadedUI.Count-1}]");

                panel.RectFull();

                panel.gameObject.SetActive(false);

                panel.Initalize(this);

                return (T)panel;
            }
        }

        public static void RectFull(RectTransform rect) {

            rect.anchoredPosition = Vector2.zero;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;
        }

        [ContextMenu("ToString")]
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"loadedUI [count,{loadedUI.Count}]");

            for (var i = 0; i < loadedUI.Count; i++)
            {
                if (loadedUI[i] == null)
                {
                    sb.AppendLine($"{i.ToString("00")} [{loadedUI[i].IsActive}] [id,{loadedUI[i].ID}] [name,{loadedUI[i].name}]");
                }
                else
                {
                    sb.AppendLine($"{i.ToString("00")} is null");
                }
            }

            sb.AppendLine($"listOpenUI [count,{loadedUI.Count}]");

            for (var i = 0; i < listOpenUI.Count; i++)
            {
                sb.AppendLine($"{i.ToString("00")} [id,{listOpenUI[i]}]");
            }
            return sb.ToString();
        }
    }
}
