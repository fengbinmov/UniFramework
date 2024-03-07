using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace UniFramework.Uipanel
{
    [RequireComponent(typeof(Canvas))]
    public class PanelPool : MonoBehaviour
    {
        private Canvas _canvas;
        private GraphicRaycaster _raycaster;

        private Transform contentShow;
        private Transform contentClose;

        protected List<PanelBase> loadedUI = new List<PanelBase>();     //页面容器 -- 存放所有实例化的页面
        protected List<int> listOpenUI = new List<int>();               //记录已经现实的页面ID

        public static PanelPool Initalize(GameObject desktop) {

            if (desktop.TryGetComponent<PanelPool>(out var mgr)) {
                mgr.OnInit();
                return mgr;
            }
            return null;
        }

        /// <summary>
        /// 初始化界面系统
        /// </summary>
        public virtual void OnInit()
        {
            _canvas = GetComponent<Canvas>();
            _raycaster = GetComponent<GraphicRaycaster>();

            if (contentClose == null) contentClose = transform.Find("- Close Layer");
            if (contentShow == null) contentShow = transform.Find("- Show Layer");

            if (contentClose == null) 
            {
                GameObject obj = new GameObject("- Close Layer");
                obj.transform.SetParent(transform);
                RectFull(obj.AddComponent<RectTransform>());
                contentClose = obj.transform;
            }
            if (contentShow == null)
            {
                GameObject obj = new GameObject("- Show Layer");
                obj.transform.SetParent(transform);
                RectFull(obj.AddComponent<RectTransform>());
                contentShow = obj.transform;
            }

            loadedUI.Clear();
            listOpenUI.Clear();
        }

        /// <summary>
        /// 打开页面
        /// </summary>
        /// <typeparam name="T">页面类型</typeparam>
        /// <param name="param">传入参数</param>
        /// <returns>页面</returns>
        public T ShowPanel<T>(System.Object param = null) where T : PanelBase
        {
            return ShowPanel<T>(nameof(T),param);
        }

        /// <summary>
        /// 打开页面
        /// </summary>
        /// <typeparam name="T">页面类型</typeparam>
        /// <param name="panelName">页面名称</param>
        /// <param name="param">传入参数</param>
        /// <returns>页面</returns>
        public T ShowPanel<T>(string panelName, System.Object param = null) where T : PanelBase {

            PanelBase panel = ShowPanel(panelName, param);
            return panel == null ? null : (T)panel;
        }

        /// <summary>
        /// 打开页面
        /// </summary>
        /// <param name="panelName">页面名称</param>
        /// <param name="param">传入参数</param>
        /// <returns>页面</returns>
        public PanelBase ShowPanel(string panelName, System.Object param = null)
        {
            PanelBase panel = GetPanel(panelName);
            return ShowPanel(panel); ;
        }

        /// <summary>
        /// 打开页面
        /// </summary>
        /// <param name="panel">页面</param>
        /// <param name="param">传入参数</param>
        /// <returns>页面</returns>
        public virtual PanelBase ShowPanel(PanelBase panel, System.Object param = null)
        {
            if (panel == null) return null;

            if (listOpenUI.Contains(panel.ID)) return panel;

            panel.transform.SetParent(contentShow);
            panel.transform.SetAsLastSibling();

            listOpenUI.Add(panel.ID);
            panel.ShowPanel(param);

            return panel;
        }

        /// <summary>
        /// 关闭页面
        /// </summary>
        /// <typeparam name="T">页面类型</typeparam>
        public void ClosePanel<T>()
        {
            ClosePanel(nameof(T));
        }

        /// <summary>
        /// 关闭页面
        /// </summary>
        /// <param name="panelName">页面名称</param>
        public void ClosePanel(string panelName)
        {
            PanelBase panel = GetPanel(panelName);
            ClosePanel(panel);
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
        /// 获取页面
        /// </summary>
        /// <param name="name">页面名称</param>
        /// <returns>页面</returns>
        public PanelBase GetPanel(string name) {

            for (int i = 0; loadedUI.Count > i; i++) 
            {
                if (loadedUI[i].name.Equals(name))
                    return loadedUI[i];
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
                PanelBase panel = loadedUI[i];
                if (panel.name == name)
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
                Debug.LogError($"有相同名称的页面加入 {panelName}！");
                return null;
            }
            else
            {
                GameObject obj = GameObject.Instantiate(prefab, contentClose);
                if (panelName != null) obj.name = panelName;
                PanelBase panel = obj.GetComponent<PanelBase>();

                loadedUI.Add(panel);

                panel.transform.localPosition = Vector3.zero;
                RectTransform rect = (RectTransform)panel.transform;
                rect.anchoredPosition = Vector2.zero;
                rect.anchorMax = Vector2.zero;
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.one;

                panel.Initalize(this);

                panel.gameObject.SetActive(false);

                return (T)panel;
            }
        }

        public static void RectFull(RectTransform rect) {

            rect.anchoredPosition = Vector2.zero;
            rect.anchorMax = Vector2.zero;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;
        }
    }
}