using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace LitMVC {
	public partial class TestPageView : UIView {
		public const string VIEW_NAME = "TestPage";
		public Image m_img_bg_Image;
		public Button m_btn_1_Button;
		public Image m_btn_1_Image;
		public Button m_btn_2_Button;
		public Image m_btn_2_Image;
		public Button m_btn_3_Button;
		public Image m_btn_3_Image;
		public LitMVC.SubPageView m_SubPage_SubView;
		public LitMVC.SubPage_VariantView m_SubPage_Variant_SubView;
		public Image m_bg_img_Image;
		public TMP_Text m_test1_text_TMP_Text;
		public TMP_Text m_test2_text_TMP_Text;
	}
}
