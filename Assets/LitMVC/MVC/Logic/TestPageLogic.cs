using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LitMVC {
	public partial class TestPageView : UIView {
		private TestService _service = new TestService();

		private void Awake() {
			Init();
		}

		public async void Init() {
			await _service.Init();
			Refresh();
		}

		public void Refresh() {
			m_test1_text_TMP_Text.text = _service.testDatas[0].text1;
			m_test2_text_TMP_Text.text = _service.testDatas[0].text2;
		}
	}
}
