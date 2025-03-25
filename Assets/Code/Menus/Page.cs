using UnityEngine;

namespace Code.Menus
{
    public abstract class Page : MonoBehaviour
    {
        protected virtual void OnOpen()
        {
        }

        protected virtual void OnClose()
        {
        }

        public void Open()
        {
            gameObject.SetActive(true);
            OnOpen();
        }

        public void Close()
        {
            gameObject.SetActive(false);
            OnClose();
        }
    }
}