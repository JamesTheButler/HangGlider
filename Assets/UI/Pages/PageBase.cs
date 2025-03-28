using UnityEngine;

namespace UI.Pages
{
    public abstract class PageBase : MonoBehaviour
    {
        private bool _isOpen;

        protected virtual void OnOpen()
        {
        }

        protected virtual void OnClose()
        {
        }

        public void Open()
        {
            if (_isOpen) return;

            gameObject.SetActive(true);
            OnOpen();
            _isOpen = true;
        }

        public void Close()
        {
            gameObject.SetActive(false);

            if (!_isOpen) return;

            OnClose();
            _isOpen = false;
        }
    }
}