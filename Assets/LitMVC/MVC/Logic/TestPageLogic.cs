namespace LitMVC {
	public partial class TestPageView : UIView {
		private TestPageService _service = new TestPageService();
		private void Awake() {
			BindView();
			Init();
		}

		private async void Init() {
			await _service.Init();
			Refresh();
		} 

		public void Refresh() {
			m_test1_text_TMP_Text.text = _service.data[0].text1;
			m_test2_text_TMP_Text.text = _service.data[0].text2;
		}
	}
}
