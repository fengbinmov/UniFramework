using UnityEngine;

namespace UniFramework.Uipanel {

    public abstract class PanelBase : MonoBehaviour
    {
        private PanelPool _PanelPool;
        private bool _isInitialize = false;

        protected PanelPool PanelPool => _PanelPool;

        public int ID => GetInstanceID();
        public bool IsActive => gameObject.activeInHierarchy;

        #region internal
        internal void Initalize(PanelPool PanelPool_) {
            _PanelPool = PanelPool_;

            if (!_isInitialize)
            {
                _isInitialize = true;
                OnInit();
            }
        }

        internal void ShowPanel(System.Object param = null) {

            gameObject.SetActive(true);

            OnShow(param);
        }

        internal void ClosePanel() {

            OnClear();

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

        protected virtual void OnShow(System.Object param = null)
        {
        }

        /// <summary>
        /// 关闭界面
        /// </summary>
        protected virtual void OnClose()
        {
        }

        /// <summary>
        /// 清理界面
        /// </summary>
        protected virtual void OnClear()
        {
        }
    }
}
