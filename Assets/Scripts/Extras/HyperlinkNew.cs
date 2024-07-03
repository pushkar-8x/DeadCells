using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HyperlinkNew : MonoBehaviour, IPointerClickHandler
{
#if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void OpenURLInExternalWindow(string url);
#else
    private static void OpenURLInExternalWindow(string url) { }
#endif

    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text urlText;
    private void Awake()
    {
        if(_text == null)
        {
            _text = GetComponent<TMP_Text>();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int _linkIndex = TMP_TextUtilities.FindIntersectingLink(_text, eventData.position, null);
        urlText.text = "Text got clicked!";
        if (_linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = _text.textInfo.linkInfo[_linkIndex];
            OnLinkPressed(linkInfo.GetLinkID());
        }
    }

    public void OnLinkPressed(string url)
    {
        urlText.text = "Link got clicked!";
#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
        Application.OpenURL(url);
#endif

#if UNITY_WEBGL
        OpenURLInExternalWindow(url);
#endif
    }
}
