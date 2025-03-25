using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace LitMVC {
	public partial class SubPage_VariantView : LitMVC.SubPageView {
		public const string VIEW_NAME = "SubPage_Variant";
		[HideInInspector] public LitMVC.SubOfSubView m_SubOfSub_SubView;
		[HideInInspector] public LitMVC.SubOfSub_VariantView m_SubOfSub_Variant_SubView;
		[HideInInspector] public Image m_bg2_img_Image;
		private void BindView() {
			m_SubOfSub_SubView = transform.Find("SubOfSub").GetComponent<LitMVC.SubOfSubView>();
			m_SubOfSub_Variant_SubView = transform.Find("SubOfSub_Variant").GetComponent<LitMVC.SubOfSub_VariantView>();
			m_bg2_img_Image = transform.Find("SubOfSub_Variant/bg2_img").GetComponent<Image>();
		}
	}
}
