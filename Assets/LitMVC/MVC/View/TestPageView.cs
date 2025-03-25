using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace LitMVC {
	public partial class TestPageView : UIView {
		public const string VIEW_NAME = "TestPage";
		[HideInInspector] public Image m_img_bg_Image;
		[HideInInspector] public Button m_btn_1_Button;
		[HideInInspector] public Image m_btn_1_Image;
		[HideInInspector] public Button m_btn_2_Button;
		[HideInInspector] public Image m_btn_2_Image;
		[HideInInspector] public Button m_btn_3_Button;
		[HideInInspector] public Image m_btn_3_Image;
		[HideInInspector] public LitMVC.SubPageView m_SubPage_SubView;
		[HideInInspector] public LitMVC.SubPage_VariantView m_SubPage_Variant_SubView;
		[HideInInspector] public Image m_bg_img_Image;
		[HideInInspector] public TMP_Text m_test1_text_TMP_Text;
		[HideInInspector] public TMP_Text m_test2_text_TMP_Text;
		private void BindView() {
			m_img_bg_Image = transform.Find("img_bg").GetComponent<Image>();
			m_btn_1_Button = transform.Find("btn_1").GetComponent<Button>();
			m_btn_1_Image = transform.Find("btn_1").GetComponent<Image>();
			m_btn_2_Button = transform.Find("btn_2").GetComponent<Button>();
			m_btn_2_Image = transform.Find("btn_2").GetComponent<Image>();
			m_btn_3_Button = transform.Find("btn_3").GetComponent<Button>();
			m_btn_3_Image = transform.Find("btn_3").GetComponent<Image>();
			m_SubPage_SubView = transform.Find("SubPage").GetComponent<LitMVC.SubPageView>();
			m_SubPage_Variant_SubView = transform.Find("SubPage_Variant").GetComponent<LitMVC.SubPage_VariantView>();
			m_bg_img_Image = transform.Find("SubPage_Variant/SubOfSub/bg_img").GetComponent<Image>();
			m_test1_text_TMP_Text = transform.Find("test1_text").GetComponent<TMP_Text>();
			m_test2_text_TMP_Text = transform.Find("test2_text").GetComponent<TMP_Text>();
		}
	}
}
