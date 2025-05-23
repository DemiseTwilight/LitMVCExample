using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace LitMVC {
	public partial class Page3View : UIView {
		public const string VIEW_NAME = "Page3";
		[HideInInspector] public Button m_btn_back_Button;
		[HideInInspector] public Image m_btn_back_Image;
		[HideInInspector] public TMP_Text m_text_t_TMP_Text;
		[HideInInspector] public Button m_btn_left_Button;
		[HideInInspector] public Image m_btn_left_Image;
		[HideInInspector] public Button m_btn_right_Button;
		[HideInInspector] public Image m_btn_right_Image;
		[HideInInspector] public LitMVC.SubPageView m_SubPage_SubView;
		private void BindView() {
			m_btn_back_Button = transform.Find("btn_back").GetComponent<Button>();
			m_btn_back_Image = transform.Find("btn_back").GetComponent<Image>();
			m_text_t_TMP_Text = transform.Find("text_t").GetComponent<TMP_Text>();
			m_btn_left_Button = transform.Find("btn_left").GetComponent<Button>();
			m_btn_left_Image = transform.Find("btn_left").GetComponent<Image>();
			m_btn_right_Button = transform.Find("btn_right").GetComponent<Button>();
			m_btn_right_Image = transform.Find("btn_right").GetComponent<Image>();
			m_SubPage_SubView = transform.Find("SubPage").GetComponent<LitMVC.SubPageView>();
		}
	}
}
