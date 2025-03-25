using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace LitMVC {
	public partial class SubPageView : UIView {
		public const string VIEW_NAME = "SubPage";
		[HideInInspector] public Image m_img_Bg_Image;
		private void BindView() {
			m_img_Bg_Image = transform.Find("img_Bg").GetComponent<Image>();
		}
	}
}
