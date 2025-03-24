using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace LitMVC {
	public partial class SubPage_VariantView : LitMVC.SubPageView {
		public const string VIEW_NAME = "SubPage_Variant";
		[HideInInspector] public RectTransform m_SubOfSub_Variant;
		[HideInInspector] public Image m_bg2_img_Image;
		private void Awake() {
			m_SubOfSub_Variant = transform.Find("SubPage_Variant/SubOfSub_Variant").GetComponent<RectTransform>();
			m_bg2_img_Image = transform.Find("SubOfSub_Variant/bg2_img").GetComponent<Image>();
		}
	}
}
