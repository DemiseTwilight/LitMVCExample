using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace LitMVC {
	public partial class SubPageView : UIView {
		public const string VIEW_NAME = "SubPage";
		[HideInInspector] public Image m_img_Bg_Image;
		private void Awake() {
			m_img_Bg_Image = transform.Find("SubPage/img_Bg").GetComponent<Image>();
		}
	}
}
