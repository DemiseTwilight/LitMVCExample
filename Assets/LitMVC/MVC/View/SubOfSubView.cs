using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace LitMVC {
	public partial class SubOfSubView : UIView {
		public const string VIEW_NAME = "SubOfSub";
		[HideInInspector] public Image m_bg_img_Image;
		private void BindView() {
			m_bg_img_Image = transform.Find("SubOfSub/bg_img").GetComponent<Image>();
		}
	}
}
