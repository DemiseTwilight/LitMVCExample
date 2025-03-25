using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace LitMVC {
	public partial class Page1View : UIView {
		public const string VIEW_NAME = "Page1";
		[HideInInspector] public TMP_Text m_text_t_TMP_Text;
		[HideInInspector] public Button m_btn_back_Button;
		[HideInInspector] public Image m_btn_back_Image;
		[HideInInspector] public Button m_btn_left_Button;
		[HideInInspector] public Image m_btn_left_Image;
		[HideInInspector] public Button m_btn_right_Button;
		[HideInInspector] public Image m_btn_right_Image;
		[HideInInspector] public LitMVC.SubPageView m_SubPage_SubView;
		private void BindView() {
			m_text_t_TMP_Text = transform.Find("Page1/text_t").GetComponent<TMP_Text>();
			m_btn_back_Button = transform.Find("Page1/btn_back").GetComponent<Button>();
			m_btn_back_Image = transform.Find("Page1/btn_back").GetComponent<Image>();
			m_btn_left_Button = transform.Find("Page1/btn_left").GetComponent<Button>();
			m_btn_left_Image = transform.Find("Page1/btn_left").GetComponent<Image>();
			m_btn_right_Button = transform.Find("Page1/btn_right").GetComponent<Button>();
			m_btn_right_Image = transform.Find("Page1/btn_right").GetComponent<Image>();
			m_SubPage_SubView = transform.Find("Page1/SubPage").GetComponent<LitMVC.SubPageView>();
		}
	}
}
