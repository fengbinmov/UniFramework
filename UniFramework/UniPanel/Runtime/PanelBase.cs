using UnityEngine;

namespace Uni.UniPanel
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class PanelBase : MonoBehaviour
    {
        private PanelPool _PanelPool;
        private bool _isInitialize = false;

        protected PanelPool PanelPool => _PanelPool;

        public int ID => GetInstanceID();
        public bool IsActive => gameObject.activeInHierarchy;
        public bool isAutoRectFull = true;

        #region internal
        internal void Initalize(PanelPool PanelPool_) {
            _PanelPool = PanelPool_;

            if (!_isInitialize)
            {
                _isInitialize = true;
                Debug.Log($"Panel OnInit {name}");
                OnInit();
            }
        }

        internal void ShowPanel(System.Object param = null) {

            Debug.Log($"Panel ShowPanel {name}");

            gameObject.SetActive(true);

            OnShow(param);
        }

        internal void ClosePanel() {

            Debug.Log($"Panel ClosePanel {name}");

            StopAllCoroutines();

            gameObject.SetActive(false);

            OnClose();
        }

        #endregion

        #region 公开方法
        [ContextMenu("Show")]
        public void Show() {
            if (_PanelPool != null) _PanelPool.ShowPanel(this);
        }

        [ContextMenu("Close")]
        public void Close() {

            if (_PanelPool != null) _PanelPool.ClosePanel(this);
        }

        [ContextMenu("Destroy")]
        public void Destroy() {

            if (_PanelPool != null) _PanelPool.DestroyPanel(this);
        }
        #endregion

        /// <summary>
        /// 初始化界面
        /// </summary>
        protected virtual void OnInit()
        {
        }

        /// <summary>
        /// 显示界面
        /// </summary>
        /// <param name="param"></param>
        protected virtual void OnShow(System.Object param = null)
        {
        }

        /// <summary>
        /// 关闭界面/隐藏
        /// </summary>
        protected virtual void OnClose()
        {
        }

        public void RectFull()
        {
            if (!isAutoRectFull) return;

            var rect = (RectTransform)transform;
            rect.anchoredPosition = Vector2.zero;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;
        }
    }
}
